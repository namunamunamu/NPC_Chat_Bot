using System;


public class Chat
{
    public readonly string Owner;
    public readonly string Content;
    public readonly string Info;

    public Chat(string owner, string content, string info)
    {
        if (string.IsNullOrEmpty(owner))
        {
            throw new Exception("Owner는 비어있을 수 없습니다.");
        }

        if (string.IsNullOrEmpty(content))
        {
            throw new Exception("Content는 비어있을 수 없습니다.");
        }

        if (string.IsNullOrEmpty(info))
        {
            throw new Exception("Info는 비어있을 수 없습니다.");
        }

        Owner = owner;
        Content = content;
        Info = info;
    }
}
