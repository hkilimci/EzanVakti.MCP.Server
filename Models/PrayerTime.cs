namespace EzanVakti.MCP.Server.Models;

/// <summary>
/// Represents prayer times for a specific date and location
/// </summary>
public class PrayerTime
{
    public string HicriTarihKisa { get; set; } = string.Empty;
    public string? HicriTarihKisaIso8601 { get; set; }
    public string HicriTarihUzun { get; set; } = string.Empty;
    public string? HicriTarihUzunIso8601 { get; set; }
    public string AyinSekliURL { get; set; } = string.Empty;
    public string MiladiTarihKisa { get; set; } = string.Empty;
    public string MiladiTarihKisaIso8601 { get; set; } = string.Empty;
    public string MiladiTarihUzun { get; set; } = string.Empty;
    public string MiladiTarihUzunIso8601 { get; set; } = string.Empty;
    public double GreenwichOrtalamaZamani { get; set; }
    public string Aksam { get; set; } = string.Empty;
    public string Gunes { get; set; } = string.Empty;
    public string GunesBatis { get; set; } = string.Empty;
    public string GunesDogus { get; set; } = string.Empty;
    public string Ikindi { get; set; } = string.Empty;
    public string Imsak { get; set; } = string.Empty;
    public string KibleSaati { get; set; } = string.Empty;
    public string Ogle { get; set; } = string.Empty;
    public string Yatsi { get; set; } = string.Empty;
}
