using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Weather.Report.BusinessLogic;
using Weather.Report.Config;
using Weather.Report.DataAccess;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();
builder.Services.AddTransient<IWeatherReportAggregator, WeatherReportAggregator>();
builder.Services.AddOptions();
builder.Services.Configure<WeatherDataConfig>(builder.Configuration.GetSection("WeatherDataConfig"));

builder.Services.AddDbContext<WeatherReportDbContext>(
    options => {
        options.EnableSensitiveDataLogging();
        options.EnableDetailedErrors();
        options.UseNpgsql(builder.Configuration.GetConnectionString("AppDb"));
    }, ServiceLifetime.Transient
);

var app = builder.Build();

app.MapGet(
    "weather-report/{zip}",
    async (string zip, [FromQuery] int? days, IWeatherReportAggregator weatherAgg) => {
        if (days == null || days > 30 || days < 1) {
            return Results.BadRequest("Please provide a 'days' query parameter with a value between 1 and 30");
        }
        var report = weatherAgg.BuildReport(zip, days.Value);

        return Results.Ok(report);
});


app.Run();
