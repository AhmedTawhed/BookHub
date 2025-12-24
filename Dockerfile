FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build 
WORKDIR /src

COPY ["BookHub.Api/BookHub.Api.csproj", "BookHub.Api/"]
COPY ["BookHub.Core/BookHub.Core.csproj", "BookHub.Core/"]
COPY ["BookHub.Infrastructure/BookHub.Infrastructure.csproj", "BookHub.Infrastructure/"]

RUN dotnet restore "BookHub.Api/BookHub.Api.csproj"

COPY . .

RUN dotnet publish "BookHub.Api/BookHub.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false


FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
EXPOSE 8080

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "BookHub.Api.dll"]