using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;
using System.Runtime.Versioning;
using Weather.DataLoader.Models;

IConfiguration config = new ConfigurationBuilder()
    .AddJsonFile("appSettings.json")
    .AddEnvironmentVariables()
    .Build();

var servicesConfig = config.GetSection("Services");

var tempServiceConfig = servicesConfig.GetSection("Temperature");
var tempServiceHost = tempServiceConfig["Host"];
var tempServicePort = tempServiceConfig["port"];

var precipServiceConfig = servicesConfig.GetSection("Precipitation");
var precipServiceHost = precipServiceConfig["Host"];
var precipServicePort = precipServiceConfig["port"];

var zipCodes = new List<string> {
    "68131",
    "68104",
    "04401",
    "54250",
    "68132"
};

Console.WriteLine("Starting Data Load");

var temperatureHttpClient = new HttpClient();
temperatureHttpClient.BaseAddress = new Uri($"http://{tempServiceHost}:{tempServicePort}");

var precipitationHttpClient = new HttpClient();
precipitationHttpClient.BaseAddress = new Uri($"http://{precipServiceHost}:{precipServicePort}");

foreach ( var zip in zipCodes ) {
    Console.WriteLine($"Processing Zip code: {zip}");
    var from = DateTime.Now.AddYears(-2);
    var thru = DateTime.Now;

    for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1) ) {
        var temperatures = PostTemp(zip, day, temperatureHttpClient);
        PostPrecip(temperatures[0], zip, day, precipitationHttpClient);
    }
}

void PostPrecip(int lowTemp, string zip, DateTime day, HttpClient precipitationHttpClient) {
    var rand = new Random();
    var isPrecip = rand.Next(2) < 1;

    PrecipitationModel precipitation;

    if (isPrecip) {
        var precipInches = rand.Next(1, 16);
        if (lowTemp < 32) {
            precipitation = new PrecipitationModel {
                AmountInches = precipInches,
                WeatherType = "snow",
                ZipCode = zip,
                CreatedOn = day
            };
        }
        else {
            precipitation = new PrecipitationModel {
                AmountInches = precipInches,
                WeatherType = "rain",
                ZipCode = zip,
                CreatedOn = day
            };
        }
    }
    else {
        precipitation = new PrecipitationModel {
            AmountInches = 0,
            WeatherType = "none",
            ZipCode = zip,
            CreatedOn = day
        };
    }

    var precipResponse = precipitationHttpClient
        .PostAsJsonAsync("observation", precipitation)
        .Result;

    if (precipResponse.IsSuccessStatusCode) {
        Console.WriteLine($"Posted Precipitation: Date {day:d}" +
                          $"Zip: {zip}" +
                          $"Type: {precipitation.WeatherType}" +
                          $"Amount (in.): {precipitation.AmountInches}");
    }
}

List<int> PostTemp(string zip, DateTime day, HttpClient temperatureHttpClient) {
    var rand = new Random();
    var t1 = rand.Next(-20, 45);
    var t2 = rand.Next(-20, 45);
    var hiLoTemps = new List<int> { t1, t2 };
    hiLoTemps.Sort();

    var temperatureObservation = new TemperatureModel {
        TempLow = hiLoTemps[0],
        TempHigh = hiLoTemps[1],
        ZipCode = zip,
        CreatedOn = day
    };

    var tempResponse = temperatureHttpClient
        .PostAsJsonAsync("observation", temperatureObservation)
        .Result;

    if (tempResponse.IsSuccessStatusCode) {
        Console.WriteLine($"Posted Temperature: Date {day:d}" +
                          $"Zip: {zip}" +
                          $"Temperature High (C): {temperatureObservation.TempHigh}" +
                          $"Temperature low (C): {temperatureObservation.TempLow}");
    }

    return hiLoTemps;

}