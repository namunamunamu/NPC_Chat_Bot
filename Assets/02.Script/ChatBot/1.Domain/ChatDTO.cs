
public class ChatDTO
{
    public readonly string Owner;
    public readonly string Content;
    public readonly string Info;

    public ChatDTO(Chat chat)
    {
        Owner = chat.Owner;
        Content = chat.Content;
        Info = chat.Info;
    }

    public ChatDTO(string owner, string content, string info)
    {
        Owner = owner;
        Content = content;
        Info = info;
    }
}
