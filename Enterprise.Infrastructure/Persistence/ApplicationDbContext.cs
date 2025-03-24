using Enterprise.Domain.Aircraft;
using Microsoft.EntityFrameworkCore;

namespace Enterprise.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Domain.Aircraft.Aircraft> Aircraft { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure entity mappings here
        modelBuilder.Entity<Domain.Aircraft.Aircraft>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Registration).IsRequired().HasMaxLength(20);
            entity.Property(e => e.Type).HasMaxLength(50);
            entity.Property(e => e.Manufacturer).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Model).IsRequired().HasMaxLength(100);
            entity.Property(e => e.SerialNumber).HasMaxLength(50);
            entity.Property(e => e.Status).HasConversion<string>().HasMaxLength(20);
            entity.Property(e => e.CreatedAt).IsRequired();
            entity.Property(e => e.UpdatedAt);
            entity.Ignore(e => e.DomainEvents);
        });
    }
} 