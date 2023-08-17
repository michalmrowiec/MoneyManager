using System.Text.Json.Serialization;

public class CryptoSymbolName
{
    [JsonPropertyName("id")]
    public string Id { get; set; }
    [JsonPropertyName("symbol")]
    public string Symbol { get; set; }
    [JsonPropertyName("name")]
    public string Name { get; set; }
}