using ResourceManager.Domain.Common;

namespace ResourceManager.Domain.Resources.Exceptions;

public class ResourceIsAlreadyWithdrawnException : DomainException
{
    public ResourceIsAlreadyWithdrawnException() : base("This resource is already withdrawn")
    {
    }
}