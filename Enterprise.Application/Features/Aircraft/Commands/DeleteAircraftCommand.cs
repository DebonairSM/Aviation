using MediatR;

namespace EnterpriseApiIntegration.Application.Features.Aircraft.Commands;

public record DeleteAircraftCommand : IRequest
{
    public int Id { get; init; }
}

public class DeleteAircraftCommandHandler : IRequestHandler<DeleteAircraftCommand>
{
    public async Task Handle(DeleteAircraftCommand request, CancellationToken cancellationToken)
    {
        // Demo implementation
        await Task.CompletedTask;
    }
} 