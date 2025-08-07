# EzanVakti MCP Server

A Model Context Protocol (MCP) server implementation in C# for accessing Islamic prayer times from the EzanVakti API (Diyanet - Turkey's Directorate of Religious Affairs).

## Features

- **Country Listing**: Get all available countries
- **City Search**: Find cities within a country  
- **District Lookup**: Get districts within a city
- **Prayer Times**: Retrieve monthly prayer schedules for any district
- **Smart Search**: Search cities by name and get their prayer times
- **Today's Times**: Get current day's prayer schedule

## Quick Start

### Prerequisites

- .NET 8.0 SDK or later
- Compatible MCP client (VS Code with Copilot, Claude Desktop, etc.)

### Installation

1. Clone or download the project files
2. Restore dependencies:
   ```bash
   dotnet restore
   ```
3. Build the project:
   ```bash
   dotnet build
   ```

### Running the Server

For development:
```bash
dotnet run
```

For production:
```bash
dotnet publish -c Release
./bin/Release/net8.0/publish/EzanVakti.MCP.Server
```

## MCP Client Configuration

### VS Code Configuration

Add to your `.vscode/mcp.json`:

```json
{
  "mcpServers": {
    "ezanvakti": {
      "command": "dotnet",
      "args": ["run", "--project", "/path/to/EzanVakti.MCP.Server.csproj"],
      "env": {}
    }
  }
}
```

### Claude Desktop Configuration

Add to your Claude Desktop configuration:

```json
{
  "mcpServers": {
    "ezanvakti": {
      "command": "dotnet",
      "args": ["run", "--project", "/path/to/EzanVakti.MCP.Server.csproj"]
    }
  }
}
```

## Available Tools

### 1. GetCountries
Lists all countries available in the EzanVakti database.

**Usage**: "Show me all countries with prayer times"

### 2. GetCities
Get cities/provinces for a specific country.

**Parameters**:
- `countryId`: Country ID (e.g., "2" for Turkey)

**Usage**: "Show me cities in Turkey"

### 3. GetDistricts  
Get districts within a specific city.

**Parameters**:
- `cityId`: City ID (e.g., "539" for Istanbul)

**Usage**: "Show me districts in Istanbul"

### 4. GetPrayerTimes
Get full monthly prayer schedule for a district.

**Parameters**:
- `districtId`: District ID

**Usage**: "Get prayer times for district 9535"

### 5. SearchCityPrayerTimes
Search for a city and get its prayer times.

**Parameters**:
- `cityName`: Name of city to search
- `countryId`: Country ID (defaults to "2" for Turkey)

**Usage**: "Get prayer times for Istanbul"

### 6. GetTodayPrayerTimes
Get today's prayer times for a district.

**Parameters**:
- `districtId`: District ID

**Usage**: "What are today's prayer times for district 9535?"

## Prayer Time Data Structure

Each prayer time entry includes:

- **Date Information**: Both Hijri and Gregorian dates
- **Prayer Times**:
  - `Imsak`: Pre-dawn (Suhoor time)
  - `Gunes`: Sunrise  
  - `Ogle`: Noon (Dhuhr)
  - `Ikindi`: Afternoon (Asr)
  - `Aksam`: Evening (Maghrib)
  - `Yatsi`: Night (Isha)
- **Additional**: 
  - `KibleSaati`: Qibla time (sun at Kaaba direction)
  - `GunesDogus`/`GunesBatis`: Precise sunrise/sunset

## Example Queries

Here are some natural language queries you can use:

- "What are the prayer times in Istanbul?"
- "Show me all cities in Turkey"  
- "Get today's prayer schedule for Ankara"
- "Find districts in Izmir"
- "What time is Maghrib prayer in Istanbul today?"

## Docker Support

Build Docker image:
```bash
dotnet publish --os linux --arch x64 /t:PublishContainer
```

Run in container:
```bash
docker run -it ezanvakti-mcp-server:latest
```

## API Rate Limits

The EzanVakti API has the following limits:
- 30 requests per 5 minutes
- 200 requests per day

Since prayer times are provided monthly, you typically only need one request per month per location.

## Architecture

This MCP server is built with:

- **ModelContextProtocol SDK**: Official C# implementation
- **HttpClient**: For API communication with proper disposal
- **Dependency Injection**: Clean separation of concerns
- **Logging**: Comprehensive error tracking
- **JSON Serialization**: Fast System.Text.Json

## Contributing

1. Fork the repository
2. Create a feature branch
3. Commit your changes
4. Push to the branch  
5. Create a Pull Request

## License

This project is open source. The EzanVakti API is provided by Turkey's Directorate of Religious Affairs.

## Support

For issues or questions:
- Check the logs for error details
- Verify API connectivity 
- Ensure proper MCP client configuration
- Review rate limiting if getting 429 errors