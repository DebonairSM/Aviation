namespace EnterpriseApiIntegration.Domain.Aircraft;

public class Aircraft
{
    public int Id { get; set; }
    public string Registration { get; set; }
    public string Type { get; set; }
    public string Manufacturer { get; set; }
    public string Model { get; set; }
    public int YearOfManufacture { get; set; }
    public string SerialNumber { get; set; }
    public string Status { get; set; }
    public DateTime LastMaintenanceDate { get; set; }
    public DateTime NextMaintenanceDue { get; set; }
    public int TotalFlightHours { get; set; }
} 