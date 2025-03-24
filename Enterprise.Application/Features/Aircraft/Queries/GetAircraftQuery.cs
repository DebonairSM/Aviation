using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Enterprise.Application.Features.Aircraft;
using Enterprise.Domain.Aircraft;
using MediatR;

namespace Enterprise.Application.Features.Aircraft.Queries;

public record GetAircraftQuery : IRequest<IEnumerable<AircraftDto>>;

public class GetAircraftQueryHandler : IRequestHandler<GetAircraftQuery, IEnumerable<AircraftDto>>
{
    private readonly IAircraftRepository _aircraftRepository;

    public GetAircraftQueryHandler(IAircraftRepository aircraftRepository)
    {
        _aircraftRepository = aircraftRepository;
    }

    public async Task<IEnumerable<AircraftDto>> Handle(GetAircraftQuery request, CancellationToken cancellationToken)
    {
        var aircraft = await _aircraftRepository.GetAllAsync();
        var dtos = new List<AircraftDto>();

        foreach (var a in aircraft)
        {
            dtos.Add(new AircraftDto
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

        return dtos;
    }
} 