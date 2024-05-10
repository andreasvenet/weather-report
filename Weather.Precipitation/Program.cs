using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.MapGet("/observation/{zipcode}", (string zipcode, [FromQuery] int? days) =>
{
    return Results.Ok(zipcode);
});

app.Run();
