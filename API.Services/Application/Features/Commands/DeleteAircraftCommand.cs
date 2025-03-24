using System;
using System.Threading;
using System.Threading.Tasks;
using Enterprise.Domain.Aircraft;
using MediatR;

namespace Enterprise.Application.Features.Aircraft.Commands;

public record DeleteAircraftCommand : IRequest
{
    public Guid Id { get; init; }
}

public class DeleteAircraftCommandHandler : IRequestHandler<DeleteAircraftCommand>
{
    private readonly IAircraftRepository _aircraftRepository;

    public DeleteAircraftCommandHandler(IAircraftRepository aircraftRepository)
    {
        _aircraftRepository = aircraftRepository;
    }

    public async Task Handle(DeleteAircraftCommand request, CancellationToken cancellationToken)
    {
        await _aircraftRepository.DeleteAsync(request.Id);
    }
} 