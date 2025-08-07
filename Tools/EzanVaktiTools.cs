using System.ComponentModel;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Server;
using EzanVakti.MCP.Server.Services;
using EzanVakti.MCP.Server.Models;

namespace EzanVakti.MCP.Server.Tools;

/// <summary>
/// MCP Tools for EzanVakti API operations
/// </summary>
[McpServerToolType]
public class EzanVaktiTools
{
    private readonly EzanVaktiApiService _apiService;
    private readonly ILogger<EzanVaktiTools> _logger;

    public EzanVaktiTools(EzanVaktiApiService apiService, ILogger<EzanVaktiTools> logger)
    {
        _apiService = apiService;
        _logger = logger;
    }

    [McpServerTool, Description("Get list of all available countries for prayer times")]
    public async Task<string> GetCountries()
    {
        try
        {
            var countries = await _apiService.GetCountriesAsync();
            return JsonSerializer.Serialize(countries, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetCountries tool");
            return $"Error: {ex.Message}";
        }
    }

    [McpServerTool, Description("Get list of cities/provinces in a specific country")]
    public async Task<string> GetCities(
        [Description("Country ID (e.g., '2' for Turkey)")]
        string countryId)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(countryId))
                return "Error: Country ID is required";

            var cities = await _apiService.GetCitiesAsync(countryId);
            return JsonSerializer.Serialize(cities, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetCities tool for country {CountryId}", countryId);
            return $"Error: {ex.Message}";
        }
    }

    [McpServerTool, Description("Get list of districts in a specific city")]
    public async Task<string> GetDistricts(
        [Description("City ID (e.g., '539' for Istanbul)")]
        string cityId)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(cityId))
                return "Error: City ID is required";

            var districts = await _apiService.GetDistrictsAsync(cityId);
            return JsonSerializer.Serialize(districts, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetDistricts tool for city {CityId}", cityId);
            return $"Error: {ex.Message}";
        }
    }

    [McpServerTool, Description("Get prayer times for a specific district")]
    public async Task<string> GetPrayerTimes(
        [Description("District ID (e.g., '9535' for Arnavutköy, Istanbul)")]
        string districtId)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(districtId))
                return "Error: District ID is required";

            var prayerTimes = await _apiService.GetPrayerTimesAsync(districtId);
            return JsonSerializer.Serialize(prayerTimes, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetPrayerTimes tool for district {DistrictId}", districtId);
            return $"Error: {ex.Message}";
        }
    }

    [McpServerTool, Description("Search for a city by name and get its prayer times")]
    public async Task<string> SearchCityPrayerTimes(
        [Description("City name to search for (e.g., 'Istanbul', 'Ankara')")]
        string cityName,
        [Description("Country ID (default: '2' for Turkey)")]
        string countryId = "2")
    {
        try
        {
            if (string.IsNullOrWhiteSpace(cityName))
                return "Error: City name is required";

            // Get all cities in the country
            var cities = await _apiService.GetCitiesAsync(countryId);

            // Find matching city (case-insensitive)
            var matchingCity = cities.FirstOrDefault(c => 
                c.SehirAdi.Contains(cityName, StringComparison.OrdinalIgnoreCase) ||
                c.SehirAdiEn.Contains(cityName, StringComparison.OrdinalIgnoreCase));

            if (matchingCity == null)
                return $"Error: City '{cityName}' not found";

            // Get districts for the city
            var districts = await _apiService.GetDistrictsAsync(matchingCity.SehirID);

            if (!districts.Any())
                return $"Error: No districts found for {matchingCity.SehirAdi}";

            // Get prayer times for the first district (usually main district)
            var prayerTimes = await _apiService.GetPrayerTimesAsync(districts[0].IlceID);

            var result = new
            {
                City = matchingCity,
                District = districts[0],
                PrayerTimes = prayerTimes.Take(7).ToList() // Get next 7 days
            };

            return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in SearchCityPrayerTimes tool");
            return $"Error: {ex.Message}";
        }
    }

    [McpServerTool, Description("Get today's prayer times for a specific location")]
    public async Task<string> GetTodayPrayerTimes(
        [Description("District ID")]
        string districtId)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(districtId))
                return "Error: District ID is required";

            var prayerTimes = await _apiService.GetPrayerTimesAsync(districtId);
            var today = DateTime.Today;

            var todayPrayer = prayerTimes.FirstOrDefault(pt => 
                DateTime.TryParse(pt.MiladiTarihKisa, out var date) && 
                date.Date == today);

            if (todayPrayer == null)
                return "No prayer times found for today";

            return JsonSerializer.Serialize(todayPrayer, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetTodayPrayerTimes tool");
            return $"Error: {ex.Message}";
        }
    }
}