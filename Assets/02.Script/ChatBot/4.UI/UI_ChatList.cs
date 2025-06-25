using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class UI_ChatList : MonoBehaviour
{
    [Header("[Prefabs]")]
    public UI_Chat UserChatPrefab;
    public UI_Chat NPCChatPrefab;

    public TMP_InputField PromptField;

    private List<UI_Chat> _chatList;


    private void Start()
    {
        Init();
    }

    private void Init()
    {
        ChatManager.Instance.OnChatListChanged += UpdateChat;
    }

    private void UpdateChat(ChatDTO chatData)
    {
        UI_Chat newChat = null;
        if (chatData.Owner == "user")
        {
            newChat = Instantiate(UserChatPrefab, transform);
        }
        else
        {
            newChat = Instantiate(NPCChatPrefab, transform);
        }
        newChat.InitChat(chatData);
        _chatList.Add(newChat);
    }

    public void OnSendButton()
    {
        ChatDTO userChat = new ChatDTO("user", PromptField.text, "");
        ChatManager.Instance.SendMessage(userChat);
    }
}
