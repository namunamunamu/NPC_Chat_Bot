
public class ChatDTO
{
    public readonly string Owner;
    public readonly string Content;
    public readonly string Emotion;
    public readonly string Situration;
    public readonly string ComfyUIPrompt;

    public ChatDTO(Chat chat)
    {
        Owner = chat.Owner;
        Content = chat.Content;
        Emotion = chat.Emotion;
        Situration = chat.Situration;
        ComfyUIPrompt = chat.ComfyUIPrompt;
    }

    public ChatDTO(string owner, string content, string emotion = "", string situration="", string prompt = "")
    {
        Owner = owner;
        Content = content;
        Emotion = emotion;
        Situration = situration;
        ComfyUIPrompt = prompt;
    }
}
