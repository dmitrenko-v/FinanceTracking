using System.Text.Json.Serialization;

namespace Application;

public class GoogleResponse
{
    [JsonPropertyName("id_token")]
    public string IdToken { get; set; } = null!;
}