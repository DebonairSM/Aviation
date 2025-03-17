using Bogus;
using EnterpriseApiIntegration.Application.Features.Aircraft;

namespace EnterpriseApiIntegration.Application.Features.Aircraft.FakeData;

public static class AircraftFaker
{
    private static readonly string[] AircraftTypes = { "Commercial", "Private", "Cargo", "Military" };
    private static readonly string[] Manufacturers = { "Boeing", "Airbus", "Embraer", "Bombardier", "Cessna" };
    private static readonly string[] Statuses = { "Active", "Maintenance", "Retired", "Inactive" };

    public static Faker<AircraftDto> CreateAircraftFaker()
    {
        return new Faker<AircraftDto>()
            .RuleFor(a => a.Id, f => f.IndexFaker + 1)
            .RuleFor(a => a.Registration, f => $"N{f.Random.Number(10000, 99999)}")
            .RuleFor(a => a.Type, f => f.PickRandom(AircraftTypes))
            .RuleFor(a => a.Manufacturer, f => f.PickRandom(Manufacturers))
            .RuleFor(a => a.Model, f => f.Random.Number(100, 999).ToString())
            .RuleFor(a => a.YearOfManufacture, f => f.Date.Past(20).Year)
            .RuleFor(a => a.SerialNumber, f => f.Random.AlphaNumeric(10))
            .RuleFor(a => a.Status, f => f.PickRandom(Statuses))
            .RuleFor(a => a.LastMaintenanceDate, f => f.Date.Past(1))
            .RuleFor(a => a.NextMaintenanceDue, f => f.Date.Future(1))
            .RuleFor(a => a.TotalFlightHours, f => f.Random.Number(1000, 50000));
    }
} 