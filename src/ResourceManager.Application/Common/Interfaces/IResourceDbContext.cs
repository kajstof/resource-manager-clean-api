using Microsoft.EntityFrameworkCore;
using ResourceManager.Domain.Resources;

namespace ResourceManager.Application.Common.Interfaces;

public interface IResourceDbContext
{
    DbSet<Resource> Resources { get; set; }

    Task<int> SaveChangesAsync(CancellationToken ct);
}
