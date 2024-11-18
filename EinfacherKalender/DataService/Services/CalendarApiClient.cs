using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using DataService.DTO;

namespace DataService.Services;

public class CalendarApiClient(IHttpClientFactory httpClientFactory, string clientId, string clientSecret)
{
	private readonly HttpClient _httpClient = httpClientFactory.CreateClient("CalendarApi");
	private string _bearerToken = string.Empty;
	private DateTime _tokenExpiration;

	private async Task SetAuthorizationHeaderAsync()
	{
		var bearerToken = await GetBearerTokenAsync();
		_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
	}

	private async Task<string> GetBearerTokenAsync()
	{
		if (!string.IsNullOrWhiteSpace(_bearerToken) && _tokenExpiration > DateTime.UtcNow)
		{
			return _bearerToken;
		}

		var requestData = new
		{
			clientId = clientId,
			clientSecret = clientSecret
		};

		var jsonContent = new StringContent(JsonSerializer.Serialize(requestData), Encoding.UTF8, "application/json");
		var response = await _httpClient.PostAsync($"{clientId}/api/auth", jsonContent);

		if (!response.IsSuccessStatusCode)
		{
			throw new Exception("Failed to retrieve access token");
		}

		var responseContent = await response.Content.ReadAsStringAsync();
		var responseData = JsonSerializer.Deserialize<TokenResponse>(responseContent);
		_bearerToken = responseData?.AccessToken ?? string.Empty;
		_tokenExpiration = DateTime.UtcNow.AddSeconds(responseData?.ExpiresIn ?? 0);

		return _bearerToken;
	}

	public async Task<List<CalendarEvent>> GetEventsAsync()
	{
		await SetAuthorizationHeaderAsync();
		var response = await _httpClient.GetAsync($"{clientId}/api/events");
		response.EnsureSuccessStatusCode();

		var responseContent = await response.Content.ReadAsStringAsync();
		var responseData = JsonSerializer.Deserialize<EventsResponse>(responseContent);

		return responseData?.Events ?? [];
	}

	public async Task<CalendarEvent?> PostEventAsync(CalendarEvent newEvent)
	{
		await SetAuthorizationHeaderAsync();

		// Workaround - the API only accepts dates in UTC "Zulu" format.
		var eventWithFormattedDates = new
		{
			Title = newEvent.Title,
			Description = newEvent.Description,
			startdate = newEvent.Start.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ"),
			enddate = newEvent.End.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ")
		};

		var jsonContent = new StringContent(JsonSerializer.Serialize(eventWithFormattedDates), Encoding.UTF8, "application/json");
		var response = await _httpClient.PostAsync($"{clientId}/api/events", jsonContent);

		response.EnsureSuccessStatusCode();

		var responseContent = await response.Content.ReadAsStringAsync();
		return JsonSerializer.Deserialize<CalendarEvent>(responseContent);
	}
}
