#Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source
COPY . .
RUN dotnet restore "./Weather.Temperature.csproj" --disable-parallel
RUN dotnet publish "./Weather.Temperature.csproj" -c release -o /app --no-restore

#Serve
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app ./

EXPOSE 5000

ENTRYPOINT ["dotnet", "Weather.Temperature.dll"]


