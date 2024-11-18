using System.Text.Json.Serialization;

namespace DataService.DTO;

public class EventsResponse
{
	[JsonPropertyName("total")]
	public int Total { get; init; }

	[JsonPropertyName("items")]
	public List<CalendarEvent> Events { get; init; } = [];
}
