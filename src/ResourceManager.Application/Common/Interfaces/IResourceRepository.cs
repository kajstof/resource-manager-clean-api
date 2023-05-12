using ResourceManager.Domain.Resources;

namespace ResourceManager.Application.Common.Interfaces;

// TODO Maybe later some repository instead of dbcontext
public interface IResourceRepository
{
    Task<bool> Create(Resource resource);
    Task<Resource> Get(Guid resourceId);
    Task<IReadOnlyCollection<Resource>> GetAll();
    Task<bool> Update(Resource resource);
    Task<bool> Delete(Guid resourceId);
}
