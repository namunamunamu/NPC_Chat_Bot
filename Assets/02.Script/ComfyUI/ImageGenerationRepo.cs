using System;
using System.Text;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;

public class ImageGenerationRepo : MonoBehaviour
{
    public static ImageGenerationRepo Instance;


    [Header("ComfyUI Workflow Setup")]
    public TextAsset comfyUIWorkflowJson;
    public RawImage RawImage;

    private string _prefix = "(masterpiece, best quality, ultra-detailed, 8k, highly detailed, sharp focus, cinematic lighting), 1girl, ";
    private string _filename;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        ChatManager.Instance.OnReceiveMessage += SendPromptToComfyUI;
    }


    public void SendPromptToComfyUI(ChatDTO chatDTO)
    {
        StartCoroutine(SendPromptRequest(chatDTO.ComfyUIPrompt));
    }

    private void GetImagefromComfyUI(string id)
    {
        StartCoroutine(GetImage(id));
    }



    private IEnumerator SendPromptRequest(string promptText)
    {
        if (comfyUIWorkflowJson == null)
        {
            Debug.LogError("ComfyUI Workflow API Json이 없습니다!");
            yield break;
        }

        string originalJson = comfyUIWorkflowJson.text;
        string modifiedJson = originalJson;

        if (!string.IsNullOrEmpty(promptText) && originalJson.Contains("PromptTextPlaceholder"))
        {
            Debug.Log(promptText);
            modifiedJson = originalJson.Replace("PromptTextPlaceholder", _prefix + promptText);
        }
        else
        {
            Debug.LogWarning("입력된 프롬프트가 없거나, 플레이스 홀더가 비어있습니다.");
        }

        Debug.Log("ComfyUI에 전송 중..,");

        UnityWebRequest request = new UnityWebRequest(APIKeys.COMFYUI_API_URL + "/prompt", "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(modifiedJson);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("ComfyUI API 에러: " + request.error);
            Debug.LogError("ComfyUI API Response: " + request.downloadHandler.text);
        }
        else
        {
            Debug.Log("ComfyUI API Response: " + request.downloadHandler.text);
            ComfyUIResponse reuslt = JsonUtility.FromJson<ComfyUIResponse>(request.downloadHandler.text);
            GetImagefromComfyUI(reuslt.prompt_id);
        }
    }

    private IEnumerator GetImage(string id)
    {
        string url = $"{APIKeys.COMFYUI_API_URL}/history/{id}";
        UnityWebRequest request = UnityWebRequest.Get(url);

        string filename = null;

        yield return StartCoroutine(WaitForCompletion(id, (name)=>
        {
            filename = name;
        }));

        string imageUrl = $"{APIKeys.COMFYUI_API_URL}/view?filename={filename}";

        UnityWebRequest imageRequest = UnityWebRequestTexture.GetTexture(imageUrl);
        yield return imageRequest.SendWebRequest();

        if (imageRequest.result == UnityWebRequest.Result.Success)
        {
            Texture2D tex = DownloadHandlerTexture.GetContent(imageRequest);
            RawImage.texture = tex;
            RawImage.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogError("이미지 다운로드 실패: " + imageRequest.error);
        }
    }


    [Serializable] public class ComfyUIResponse { public string prompt_id; public int number; }

    private IEnumerator WaitForCompletion(string promptId, System.Action<string> onComplete)
    {
        int maxWaitTime = 120; // 최대 2분 대기
        int checkInterval = 2;  // 2초마다 체크
        int elapsedTime = 0;

        while (elapsedTime < maxWaitTime)
        {
            yield return new WaitForSeconds(checkInterval);
            elapsedTime += checkInterval;

            // 완료 상태 확인
            bool isComplete = false;
            string filename = null;

            yield return CheckIfComplete(promptId, (complete, name) =>
            {
                isComplete = complete;
                filename = name;
            });

            if (isComplete)
            {
                onComplete?.Invoke(filename);
                yield break;
            }

            Debug.Log($"대기 중... ({elapsedTime}/{maxWaitTime}초)");
        }
    }

    private IEnumerator CheckIfComplete(string promptId, System.Action<bool, string> onComplete)
    {
        using (UnityWebRequest request = UnityWebRequest.Get($"{APIKeys.COMFYUI_API_URL}/history/{promptId}"))
        {
            yield return request.SendWebRequest();

            JObject history = JObject.Parse(request.downloadHandler.text);

            // 히스토리에 해당 ID가 있으면 완료
            if (history.ContainsKey(promptId))
            {
                JToken images = history[promptId]["outputs"]["18"]["images"];
                foreach (JToken image in images)
                {
                    _filename = image["filename"]?.ToString();
                }
                onComplete?.Invoke(true, _filename);
            }
            else
            {
                onComplete?.Invoke(false, null);
            }
        }
    }
}
