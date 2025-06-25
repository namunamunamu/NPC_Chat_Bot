using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChatGPTTest : MonoBehaviour
{
    public TextMeshProUGUI ResultTextUI;
    public TMP_InputField PromptField;
    public Button SendButton;
    public AudioSource AudioSource;
    public RawImage StoyImage;

    private string _npcName = "Rena";
}
