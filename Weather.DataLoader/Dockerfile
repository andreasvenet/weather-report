#Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source
COPY . .
RUN dotnet restore "./Weather.DataLoader.csproj" --disable-parallel
RUN dotnet publish "./Weather.DataLoader.csproj" -c release -o /app --no-restore

#Serve
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app ./

ENTRYPOINT ["dotnet", "Weather.DataLoader.dll"]


