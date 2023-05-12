using Microsoft.EntityFrameworkCore;
using ResourceManager.Application.Common.Interfaces;
using ResourceManager.Domain.Resource;

namespace ResourceManager.Infrastructure.Database;

public class ResourceDbContext : DbContext, IResourceDbContext
{
    private const string SchemaName = "public";

    public DbSet<Resource> Resources { get; set; } = null!;

    public ResourceDbContext(DbContextOptions<ResourceDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Resource>(entity =>
        {
            entity.HasIndex(e => e.Id);
            entity.OwnsMany(p => p.Locks);
            entity.Ignore(p => p.BlockedTo);
            entity.Property(p => p.RowVersion).IsRowVersion();
        }).HasDefaultSchema(SchemaName);
    }
}
