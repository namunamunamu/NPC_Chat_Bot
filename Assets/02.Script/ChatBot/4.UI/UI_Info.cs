using TMPro;
using UnityEngine;

public class UI_Info : MonoBehaviour
{
    public TextMeshProUGUI EmotionText;
    public TextMeshProUGUI SiturationText;

    public void Start()
    {
        ChatManager.Instance.OnChatListChanged += Refresh;
    }

    public void Refresh(ChatDTO chat)
    {
        EmotionText.gameObject.SetActive(true);
        SiturationText.gameObject.SetActive(true);
        
        EmotionText.text = $"현재감정: {chat.Emotion}";
        SiturationText.text = $"현재상황: {chat.Situration}";
    }
}
