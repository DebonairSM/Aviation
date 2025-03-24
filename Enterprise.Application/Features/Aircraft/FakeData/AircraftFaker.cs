using System;
using Bogus;
using Enterprise.Application.Features.Aircraft;

namespace Enterprise.Application.Features.Aircraft.FakeData;

public static class AircraftFaker
{
    private static readonly string[] AircraftTypes = { "Commercial", "Private", "Cargo", "Military" };
    private static readonly string[] Manufacturers = { "Boeing", "Airbus", "Embraer", "Bombardier", "Cessna" };

    public static Faker<AircraftDto> CreateAircraftFaker()
    {
        return new Faker<AircraftDto>()
            .RuleFor(a => a.Id, f => Guid.NewGuid())
            .RuleFor(a => a.Registration, f => $"N{f.Random.Number(10000, 99999)}")
            .RuleFor(a => a.Type, f => f.PickRandom(AircraftTypes))
            .RuleFor(a => a.Manufacturer, f => f.PickRandom(Manufacturers))
            .RuleFor(a => a.Model, f => f.Random.Number(100, 999).ToString())
            .RuleFor(a => a.SerialNumber, f => f.Random.AlphaNumeric(10))
            .RuleFor(a => a.Status, f => "Available");
    }
} 