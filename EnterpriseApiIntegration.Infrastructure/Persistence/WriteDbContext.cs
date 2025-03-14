using System.Threading;
using System.Threading.Tasks;
using EnterpriseApiIntegration.Domain.Customers;
using EnterpriseApiIntegration.Infrastructure.Persistence.Configurations;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EnterpriseApiIntegration.Infrastructure.Persistence;

public class WriteDbContext : DbContext
{
    private readonly IMediator _mediator;

    public WriteDbContext(
        DbContextOptions<WriteDbContext> options,
        IMediator mediator) : base(options)
    {
        _mediator = mediator;
    }

    public DbSet<Customer> Customers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CustomerConfiguration());
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // First, save changes to the database
        var result = await base.SaveChangesAsync(cancellationToken);

        // Then publish domain events
        var entities = ChangeTracker
            .Entries<Domain.Common.AggregateRoot>()
            .Where(e => e.Entity.DomainEvents.Any())
            .Select(e => e.Entity);

        var domainEvents = entities
            .SelectMany(e => e.DomainEvents)
            .ToList();

        // Clear domain events before publishing to prevent re-publishing
        entities.ToList().ForEach(e => e.ClearDomainEvents());

        foreach (var domainEvent in domainEvents)
        {
            await _mediator.Publish(domainEvent, cancellationToken);
        }

        return result;
    }
} 