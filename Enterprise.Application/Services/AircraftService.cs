using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Enterprise.Application.Features.Aircraft;
using Enterprise.Application.Interfaces;
using Enterprise.Domain.Aircraft;
using Enterprise.Domain.Aircraft.ValueObjects;

namespace Enterprise.Application.Services;

public class AircraftService : IAircraftService
{
    private readonly IAircraftRepository _aircraftRepository;

    public AircraftService(IAircraftRepository aircraftRepository)
    {
        _aircraftRepository = aircraftRepository;
    }

    public async Task<AircraftDto> GetByIdAsync(Guid id)
    {
        var aircraft = await _aircraftRepository.GetByIdAsync(id);
        if (aircraft == null)
            return null;

        return new AircraftDto
        {
            Id = aircraft.Id,
            Registration = aircraft.Registration,
            Type = aircraft.Type,
            Manufacturer = aircraft.Manufacturer,
            Model = aircraft.Model,
            SerialNumber = aircraft.SerialNumber,
            Status = aircraft.Status.ToString()
        };
    }

    public async Task<IEnumerable<AircraftDto>> GetAllAsync()
    {
        var aircraft = await _aircraftRepository.GetAllAsync();
        return aircraft.Select(a => new AircraftDto
        {
            Id = a.Id,
            Registration = a.Registration,
            Type = a.Type,
            Manufacturer = a.Manufacturer,
            Model = a.Model,
            SerialNumber = a.SerialNumber,
            Status = a.Status.ToString()
        });
    }

    public async Task<AircraftDto> CreateAsync(AircraftDto aircraftDto)
    {
        var aircraft = new Aircraft(
            aircraftDto.Registration,
            aircraftDto.Type,
            aircraftDto.Manufacturer,
            aircraftDto.Model,
            aircraftDto.SerialNumber);

        await _aircraftRepository.AddAsync(aircraft);

        return new AircraftDto
        {
            Id = aircraft.Id,
            Registration = aircraft.Registration,
            Type = aircraft.Type,
            Manufacturer = aircraft.Manufacturer,
            Model = aircraft.Model,
            SerialNumber = aircraft.SerialNumber,
            Status = aircraft.Status.ToString()
        };
    }

    public async Task<AircraftDto> UpdateAsync(AircraftDto aircraftDto)
    {
        var aircraft = await _aircraftRepository.GetByIdAsync(aircraftDto.Id);
        if (aircraft == null)
            return null;

        aircraft.Update(
            aircraftDto.Registration,
            aircraftDto.Type,
            aircraftDto.Manufacturer,
            aircraftDto.Model,
            aircraftDto.SerialNumber);

        await _aircraftRepository.UpdateAsync(aircraft);

        return new AircraftDto
        {
            Id = aircraft.Id,
            Registration = aircraft.Registration,
            Type = aircraft.Type,
            Manufacturer = aircraft.Manufacturer,
            Model = aircraft.Model,
            SerialNumber = aircraft.SerialNumber,
            Status = aircraft.Status.ToString()
        };
    }

    public async Task DeleteAsync(Guid id)
    {
        await _aircraftRepository.DeleteAsync(id);
    }
} 