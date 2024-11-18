using DataService.DTO;
using DataService.Services;
using EinfacherKalender.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EinfacherKalender.Pages
{
	public class IndexModel(ILogger<IndexModel> logger, CalendarApiClient calendarApiClient) : PageModel
	{
		[BindProperty] public CalendarEventFormModel NewEventFormModel { get; set; } = null!;

		/// <summary>
		/// Handles the GET /Events request to retrieve calendar events.
		/// </summary>
		/// <returns>A JSON result containing the list of events.</returns>
		public async Task<IActionResult> OnGetEventsAsync()
		{
			try
			{
				var events = await calendarApiClient.GetEventsAsync();
				return new JsonResult(events);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Failed to retrieve events");
				return StatusCode(500, "Internal Server Error");
			}
		}

		public async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid)
			{
				return Page();
			}

			try
			{
				var newEvent = new CalendarEvent()
				{
					Title = NewEventFormModel.Title,
					Description = NewEventFormModel.Description,
					Start = NewEventFormModel.StartDate.Date + NewEventFormModel.StartTime,
					End = NewEventFormModel.EndDate.Date + NewEventFormModel.EndTime
				};
				_ = await calendarApiClient.PostEventAsync(newEvent);
				return RedirectToPage("/Index");
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Failed to create event");
				return new JsonResult(new { success = false, message = "Failed to create event" });
			}
		}
	}
}
