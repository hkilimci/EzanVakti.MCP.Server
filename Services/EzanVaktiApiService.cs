using System.Text.Json;
using Microsoft.Extensions.Logging;
using EzanVakti.MCP.Server.Models;

namespace EzanVakti.MCP.Server.Services;

/// <summary>
/// Service for interacting with the EzanVakti API
/// </summary>
public class EzanVaktiApiService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<EzanVaktiApiService> _logger;

    public EzanVaktiApiService(HttpClient httpClient, ILogger<EzanVaktiApiService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<List<Country>> GetCountriesAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("/ulkeler");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var countries = JsonSerializer.Deserialize<List<Country>>(json);

            return countries ?? new List<Country>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching countries from EzanVakti API");
            throw;
        }
    }

    public async Task<List<City>> GetCitiesAsync(string countryId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"/sehirler/{countryId}");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var cities = JsonSerializer.Deserialize<List<City>>(json);

            return cities ?? new List<City>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching cities from EzanVakti API for country {CountryId}", countryId);
            throw;
        }
    }

    public async Task<List<District>> GetDistrictsAsync(string cityId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"/ilceler/{cityId}");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var districts = JsonSerializer.Deserialize<List<District>>(json);

            return districts ?? new List<District>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching districts from EzanVakti API for city {CityId}", cityId);
            throw;
        }
    }

    public async Task<List<PrayerTime>> GetPrayerTimesAsync(string districtId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"/vakitler/{districtId}");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var prayerTimes = JsonSerializer.Deserialize<List<PrayerTime>>(json);

            return prayerTimes ?? new List<PrayerTime>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching prayer times from EzanVakti API for district {DistrictId}", districtId);
            throw;
        }
    }
}