using System;


public class Chat
{
    public readonly string Owner;
    public readonly string Content;
    public readonly string Emotion;
    public readonly string Situration;
    public readonly string ComfyUIPrompt;

    public Chat(string owner, string content, string emotion = "", string situration="", string prompt = "")
    {
        if (string.IsNullOrEmpty(owner))
        {
            throw new Exception("Owner는 비어있을 수 없습니다.");
        }

        if (owner != "user" && string.IsNullOrEmpty(content))
        {
            throw new Exception("Content는 비어있을 수 없습니다.");
        }

        if (owner != "user" && string.IsNullOrEmpty(emotion))
        {
            throw new Exception("Content는 비어있을 수 없습니다.");
        }

        if (owner != "user" && string.IsNullOrEmpty(situration))
        {
            throw new Exception("Content는 비어있을 수 없습니다.");
        }


        if (owner != "user" && string.IsNullOrEmpty(prompt))
        {
            throw new Exception("Info는 비어있을 수 없습니다.");
        }

        Owner = owner;
        Content = content;
        Emotion = emotion;
        Situration = situration;
        ComfyUIPrompt = prompt;
    }
}
