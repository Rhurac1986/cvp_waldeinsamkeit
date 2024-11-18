using System.Text.Json.Serialization;

namespace DataService.DTO;

public class CalendarEvent
{
	[JsonPropertyName("id")]
	public string Id { get; set; } = string.Empty;

	[JsonPropertyName("title")]
	public string Title { get; set; } = string.Empty;

	[JsonPropertyName("description")]
	public string Description { get; set; } = string.Empty;

	[JsonPropertyName("startDate")]
	public DateTimeOffset Start { get; set; }

	[JsonPropertyName("endDate")]
	public DateTimeOffset End { get; set; }
}
