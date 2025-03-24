using System;

namespace Enterprise.Application.Features.Aircraft;

/// <summary>
/// Data transfer object for aircraft information
/// </summary>
public class AircraftDto
{
    /// <summary>
    /// Unique identifier for the aircraft
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Aircraft registration number
    /// </summary>
    public string Registration { get; set; }

    /// <summary>
    /// Type of aircraft (e.g., Commercial, Private, Military)
    /// </summary>
    public string Type { get; set; }

    /// <summary>
    /// Aircraft manufacturer
    /// </summary>
    public string Manufacturer { get; set; }

    /// <summary>
    /// Aircraft model name
    /// </summary>
    public string Model { get; set; }

    /// <summary>
    /// Year the aircraft was manufactured
    /// </summary>
    public int YearOfManufacture { get; set; }

    /// <summary>
    /// Aircraft serial number
    /// </summary>
    public string SerialNumber { get; set; }

    /// <summary>
    /// Current operational status of the aircraft
    /// </summary>
    public string Status { get; set; }

    /// <summary>
    /// Date of the last maintenance performed
    /// </summary>
    public DateTime LastMaintenanceDate { get; set; }

    /// <summary>
    /// Date when the next maintenance is due
    /// </summary>
    public DateTime NextMaintenanceDue { get; set; }

    /// <summary>
    /// Total number of flight hours accumulated
    /// </summary>
    public int TotalFlightHours { get; set; }
} 