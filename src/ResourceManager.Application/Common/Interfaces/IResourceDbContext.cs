using Microsoft.EntityFrameworkCore;
using ResourceManager.Domain.Resource;

namespace ResourceManager.Application.Common.Interfaces;

public interface IResourceDbContext
{
    DbSet<Resource> Resources { get; set; }
}
