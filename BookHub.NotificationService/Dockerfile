FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["BookHub.NotificationService/BookHub.NotificationService.csproj", "BookHub.NotificationService/"]
COPY ["BookHub.Contracts/BookHub.Contracts.csproj", "BookHub.Contracts/"]

RUN dotnet restore "BookHub.NotificationService/BookHub.NotificationService.csproj"

COPY . .

RUN dotnet publish "BookHub.NotificationService/BookHub.NotificationService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "BookHub.NotificationService.dll"]
