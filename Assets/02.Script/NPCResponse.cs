using Newtonsoft.Json;

public class NPCResponse
{
    [JsonProperty("ReplyMessage")]
    public string ReplyMessage { get; set; }

    [JsonProperty("Appearance")]
    public string Appearance { get; set; }

    [JsonProperty("Emotion")]
    public string Emotion { get; set; }

    [JsonProperty("StoyImageDescription")]
    public string StoyImageDescription { get; set; }
}
