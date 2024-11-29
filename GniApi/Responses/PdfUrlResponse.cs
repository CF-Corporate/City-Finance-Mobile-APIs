using System.Text.Json.Serialization;

namespace GniApi.Responses;

public class PdfUrlResponse
{
    [JsonPropertyName("url")]
    public string Url { get; set; }
}