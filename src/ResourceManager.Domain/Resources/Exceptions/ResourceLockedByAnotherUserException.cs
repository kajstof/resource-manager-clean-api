using ResourceManager.Domain.Common;

namespace ResourceManager.Domain.Resources.Exceptions;

public class ResourceLockedByAnotherUserException : DomainException
{
    public ResourceLockedByAnotherUserException() : base("This resource is blocked by other user")
    {
    }
}
