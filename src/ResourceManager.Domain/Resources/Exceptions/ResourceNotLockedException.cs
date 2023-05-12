using ResourceManager.Domain.Common;

namespace ResourceManager.Domain.Resources.Exceptions;

public class ResourceNotLockedException : DomainException
{
    public ResourceNotLockedException() : base("This resource is not locked")
    {
    }
}