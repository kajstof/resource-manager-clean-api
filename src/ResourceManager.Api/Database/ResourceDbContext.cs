using Microsoft.EntityFrameworkCore;
using ResourceManager.Api.Models;

namespace ResourceManager.Api.Database;

public class ResourceDbContext : DbContext
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
            entity.HasIndex(e => e.Id)
        );
    }
}
