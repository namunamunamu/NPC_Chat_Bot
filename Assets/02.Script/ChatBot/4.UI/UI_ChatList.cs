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
        _chatList = new List<UI_Chat>();
    }

    private void UpdateChat(ChatDTO chatData)
    {
        if (chatData.Owner == "user")
        {
            UI_Chat newChat = Instantiate(UserChatPrefab, transform);
            newChat.InitChat(chatData);
            _chatList.Add(newChat);
        }
        else
        {
            UI_Chat newChat = Instantiate(NPCChatPrefab, transform);
            newChat.InitChat(chatData);
            _chatList.Add(newChat);
        }
    }

    public void OnSendButton()
    {
        ChatDTO userChat = new ChatDTO("user", PromptField.text, "");

        PromptField.text = "";
        UpdateChat(userChat);

        ChatManager.Instance.SendMessage(userChat);
    }
}
