using System;
using System.Collections.Generic;
using UnityEngine;

public class ChatManager : MonoBehaviour
{
    public static ChatManager Instance;

    private List<Chat> _chatList;
    public List<ChatDTO> ChatList => _chatList.ConvertAll(x => new ChatDTO(x));

    public event Action<ChatDTO> OnReceiveMessage;
    public event Action<ChatDTO> OnChatListChanged;

    private ChatRepository _repo;

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

        Init();
    }

    private void Init()
    {
        _chatList = new List<Chat>();
        _repo = new ChatRepository();
    }

    public async void SendMessage(ChatDTO userChat)
    {
        Debug.Log("SendMessage");
        ChatDTO responseMessage = await _repo.OnSendMessage(userChat);
        Debug.Log("Done");
        OnReceiveMessage?.Invoke(responseMessage);

        ChatList.Add(responseMessage);
        OnChatListChanged?.Invoke(responseMessage);
    }
}
