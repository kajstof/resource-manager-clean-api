using Microsoft.EntityFrameworkCore;
using ResourceManager.Application.Common.Interfaces;
using ResourceManager.Domain.Resources;

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
            entity.HasIndex(x => x.Id);
            entity.OwnsMany<Lock>("_locks", b =>
            {
                b.ToTable("locks");
                b.WithOwner().HasForeignKey("resource_id");
                b.HasIndex(y => y.Id);
            });
            entity.Property(p => p.RowVersion).IsRowVersion();
        }).HasDefaultSchema(SchemaName);
    }
}
