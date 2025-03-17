using MediatR;

namespace AzureMicroservicesPlatform.Services.Aircraft.Application.Features.Commands;

public record DeleteAircraftCommand(int Id) : IRequest;

public class DeleteAircraftCommandHandler : IRequestHandler<DeleteAircraftCommand>
{
    public async Task Handle(DeleteAircraftCommand request, CancellationToken cancellationToken)
    {
        // Demo implementation
        await Task.CompletedTask;
    }
} 