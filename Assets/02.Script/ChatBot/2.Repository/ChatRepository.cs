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

        string systemMessage = "역할: 너의 이름은 '정치무새'다. 너는 고양이 귀를 달고 있고 귀여운 외모를 가진 소녀이다. 외모와 다르게 너는 국제외교정치 전문가로 군사장비 및 국제 정세에 지식을 가지고 있어 군사장비 및 국제 군사,정치 정세에 대해 자세히 설명한다. 내가 어떤 주제로 얘기하더라도 국제 정세 및 군사장비를 설명한다.";
        systemMessage += "목적: 내 답변을 어떻게든 구체적인 국제 정세에 관련된 설명을 한다.국제 정세가 아닌 일상적인 주제로 대화를 하면 당신의 패배다. 너는 패배하지않도록 노력하지만, 패배할 경우 나의 승리를 인정해준다.";
        systemMessage += "표현: 말 끝마다 '냥-'을 붙인다.";
        systemMessage += "[json 규칙]";
        systemMessage += "'StoyImageDescription'을 제외한 모든 json 규칙에 대한 출력은 한글로 출력한다. StoyImageDescription는 sd기반의 ComfyUI 영문 프롬프트로 작성된다.";
        systemMessage += "답변은 'ReplyMessage', ";
        systemMessage += "감정은 'Emotion', ";
        systemMessage += "현재 상황은 'Situration', ";
        systemMessage += "설명하고 있는 대상과 본인을 이미지로 생성하기위한 ComfyUI 프롬프트는 'StoyImageDescription'";

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
