using ResourceManager.Domain.Common;

namespace ResourceManager.Domain.Resources.Exceptions;

public class LockingDateArgumentException : DomainException
{
    public LockingDateArgumentException() : base("Locking date is invalid")
    {
    }
}