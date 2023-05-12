using ResourceManager.Domain.Common;

namespace ResourceManager.Domain.Resources.Exceptions;

public class ResourceIsWithdrawnException : DomainException
{
    public ResourceIsWithdrawnException() : base("This resource is withdrawn")
    {
    }
}
