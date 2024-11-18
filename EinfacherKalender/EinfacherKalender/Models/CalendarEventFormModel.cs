namespace EinfacherKalender.Models;

public class CalendarEventFormModel
{
	public string Title { get; set; }
	public string Description { get; set; }
	public DateTimeOffset StartDate { get; set; }
	public TimeSpan StartTime { get; set; }
	public DateTimeOffset EndDate { get; set; }
	public TimeSpan EndTime { get; set; }
}
