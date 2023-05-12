using ResourceManager.Domain.Common;

namespace ResourceManager.Domain.Resources.Exceptions;

public class LockingDateWeakerThanCurrentLockException : DomainException
{
    public LockingDateWeakerThanCurrentLockException() : base("This resource is locked for longer period")
    {
    }
}
