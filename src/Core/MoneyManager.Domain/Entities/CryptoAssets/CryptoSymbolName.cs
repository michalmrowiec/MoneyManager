using System.Text.Json.Serialization;

public class CryptoSymbolName
{
    [JsonPropertyName("id")]
    public string CoinGeckoId { get; set; }
    [JsonPropertyName("symbol")]
    public string Symbol { get; set; }
    [JsonPropertyName("name")]
    public string Name { get; set; }
}