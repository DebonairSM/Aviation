namespace AzureMicroservicesPlatform.Services.Aircraft.Application.Models;

public class AircraftDto
{
    public int Id { get; set; }
    public string Registration { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Manufacturer { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public int YearOfManufacture { get; set; }
    public string SerialNumber { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime LastMaintenanceDate { get; set; }
    public DateTime NextMaintenanceDue { get; set; }
    public int TotalFlightHours { get; set; }
} 