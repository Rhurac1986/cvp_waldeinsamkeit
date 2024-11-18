using System.Text.Json.Serialization;

namespace DataService.DTO;

public class TokenResponse
{
	[JsonPropertyName("access_token")]
	public string AccessToken { get; init; } = string.Empty;

	[JsonPropertyName("expires_in")]
	public int ExpiresIn { get; init; }
}
