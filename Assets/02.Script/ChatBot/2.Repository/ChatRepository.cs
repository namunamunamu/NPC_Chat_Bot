using System.Collections.Generic;
using System.Threading.Tasks;
using OpenAI;
using OpenAI.Chat;
using OpenAI.Models;
using UnityEngine;

public class ChatRepository
{
    private OpenAIClient _api;
    private List<Message> _memories = new List<Message>();

    private string _npcName = "Rena";

    public ChatRepository()
    {
        _api = new OpenAIClient(APIKeys.OPENAI_API_KEY);
        InitPromptRoles();
    }

    private void InitPromptRoles()
    {
        // CHAT-F
        // C => Context     : 문맥, 상황을 많이 알려줘라
        // H => Hint        : 예시 답변을 많이 줘라
        // A => As A Role   : 역할을 제공해라
        // T => Target      : 답변의 타겟 대상자를 알려줘라
        // F => Forma       : 답변 형태를 정해줘라

        string systemMessage = "역할: 너는 게임 NPC '레나'다. 너는 게임 속 고양이 귀를 달고 있는 일본 애니메이션 캐릭터처럼 생긴 NPC이다.";
        systemMessage += "목적: 실제 사람처럼 대화하는 게임 NPC 모드";
        systemMessage += "표현: 말 끝마다 '냥-'을 붙인다. 항상 100글자 이내로 답변한다.";
        systemMessage += "[json 규칙]";
        systemMessage += "모든 json 규칙에 대한 출력은 한글로 출력한다.";
        systemMessage += "답변은 'ReplyMessage', ";
        systemMessage += "감정은 'Emotion', ";
        systemMessage += "현재 상황은 'Situration', ";
        systemMessage += "현재 상황을 이미지로 생성하기위한 ComfyUI 프롬프트는 'StoyImageDescription'";

        _memories.Add(new Message(Role.User, systemMessage));
    }

    public async Task<ChatDTO> OnSendMessage(ChatDTO userChat)
    {
         Debug.Log("OnSendMessage");

        Message promptMessage = new Message(Role.User, userChat.Content);
        _memories.Add(promptMessage);

        ChatRequest chatRequest = new ChatRequest(_memories, Model.GPT4o);
        var (npcResponse, response) = await _api.ChatEndpoint.GetCompletionAsync<NPCResponse>(chatRequest);

        Choice choice = response.FirstChoice;

        Message resultMessage = new Message(Role.Assistant, choice.Message);
        _memories.Add(resultMessage);

        return new ChatDTO(_npcName, npcResponse.ReplyMessage, npcResponse.Emotion, npcResponse.Situration, npcResponse.StoyImageDescription);
    }
}
