# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

# Copy project files
COPY ["src/API/API.csproj", "src/API/"]
COPY ["src/Application/Application.csproj", "src/Application/"]
COPY ["src/Domain/Domain.csproj", "src/Domain/"]
COPY ["src/Infrastructure/Infrastructure.csproj", "src/Infrastructure/"]

# Restore NuGet packages
RUN dotnet restore "src/API/API.csproj"

# Copy remaining source code
COPY . .

WORKDIR "/src/src/API"

# Build the application
RUN dotnet build "API.csproj" -c Release -o /app/build

# Publish Stage
FROM build AS publish

RUN dotnet publish "API.csproj" -c Release -o /app/publish /p:UseAppHost=false


# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final

WORKDIR /app

COPY --from=publish /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "API.dll"]