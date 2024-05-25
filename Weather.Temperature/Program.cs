using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Weather.Temperature.DataAccess;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TemperatureDbContext>(
    options => {
        options.EnableSensitiveDataLogging();
        options.EnableDetailedErrors();
        options.UseNpgsql(builder.Configuration.GetConnectionString("AppDb"));
    }, ServiceLifetime.Transient
);

var app = builder.Build();

app.MapGet("/observation/{zipcode}", async (string zipcode, [FromQuery] int? days, TemperatureDbContext db) => {
    if (days == null || days < 1 || days > 30) {
        return Results.BadRequest("Please provide a 'days' query param between 1 and 30");
    }

    var startDate = DateTime.UtcNow - TimeSpan.FromDays(days.Value);
    var results = await db.Temperature
    .Where(precip => precip.ZipCode == zipcode && precip.CreatedOn > startDate)
    .ToListAsync();

    return Results.Ok(results);
});

//TODO - separate resource model from the data model (make some kind of DTO)
app.MapPost("/observation", async (Temperature temperature, TemperatureDbContext db) => {
    temperature.CreatedOn = temperature.CreatedOn.ToUniversalTime();
    await db.AddAsync(temperature);
    await db.SaveChangesAsync();
});

app.Run();
