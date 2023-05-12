using ResourceManager.Domain.Common;

namespace ResourceManager.Domain.Resource;

public class Resource : IAggregateRoot
{
    public Guid Id { get; protected set; }


    private Resource()
    {
    }

    public Resource CreateNew(Guid id)
    {
        return new Resource { Id = id };
    }
}
