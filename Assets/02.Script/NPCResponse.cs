using Newtonsoft.Json;

public class NPCResponse
{
    [JsonProperty("ReplyMessage")]
    public string ReplyMessage { get; set; }

    [JsonProperty("Emotion")]
    public string Emotion { get; set; }

    [JsonProperty("Situration")]
    public string Situration { get; set; }

    [JsonProperty("StoyImageDescription")]
    public string StoyImageDescription { get; set; }
}
