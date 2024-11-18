using DataService.Services;
using DotNetEnv;

namespace EinfacherKalender
{
	public static class Program
	{
		public static void Main(string[] args)
		{
			// Load environment variables from .env file
			Env.Load();

			var builder = WebApplication.CreateBuilder(args);
			var calendarApiConfig = builder.Configuration.GetSection("CalendarApi");

			// Add services to the container.
			builder.Services.AddRazorPages();

			// Register the Calendar HTTP Client
			builder.Services.AddHttpClient("CalendarApi", client =>
			{
				client.BaseAddress = new Uri(calendarApiConfig["BaseUrl"] ?? string.Empty);
				client.DefaultRequestHeaders.Add("Accept", "application/json");
			});

			// Register the Calendar API client
			builder.Services.AddSingleton<CalendarApiClient>(sp =>
			{
				var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
				var clientId = Environment.GetEnvironmentVariable("CLIENT_ID") ?? string.Empty;
				var clientSecret = Environment.GetEnvironmentVariable("CLIENT_SECRET") ?? string.Empty;
				return new CalendarApiClient(httpClientFactory, clientId, clientSecret);
			});

			// Configure logger to use the EventViewer
			builder.Logging.ClearProviders();
			builder.Logging.AddConsole();
			if (OperatingSystem.IsWindows())
			{
				builder.Logging.AddEventLog(settings =>
				{
					settings.SourceName = "EinfacherKalender";
				});
			}

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Error");  // Configures a middleware to redirect exceptions
				app.UseHsts(); // Adds a Strict-Transport-Security header to the response
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();
			app.UseRouting();
			app.UseAuthorization();
			app.MapRazorPages();
			app.Run();
		}
	}
}
