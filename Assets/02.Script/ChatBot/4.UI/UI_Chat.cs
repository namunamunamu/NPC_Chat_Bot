using TMPro;
using UnityEngine;

public class UI_Chat : MonoBehaviour
{
    public TextMeshProUGUI NameText;
    public TextMeshProUGUI ContentText;


    public void InitChat(ChatDTO chat)
    {
        NameText.text = chat.Owner;
        ContentText.text = chat.Content;
    }
}
