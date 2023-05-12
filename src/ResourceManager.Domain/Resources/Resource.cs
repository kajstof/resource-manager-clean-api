using ResourceManager.Domain.Common;
using ResourceManager.Domain.Resources.Exceptions;

namespace ResourceManager.Domain.Resources;

public class Resource : IAggregateRoot
{
    public Guid Id { get; private set; }

    public string Name { get; private set; } = null!;

    public bool IsWithdrawn { get; private set; }

    public ICollection<Lock> Locks { get; } = new List<Lock>();

    public uint RowVersion { get; private set; }

    private Resource()
    {
    }

    public static Resource CreateNew(Guid id, string name)
    {
        return new Resource
        {
            Id = id,
            Name = name,
            IsWithdrawn = false
        };
    }

    public void LockPermanently(string username, DateTimeOffset currentDate)
    {
        LockTemporary(username, currentDate, DateTimeOffset.MaxValue);
    }

    public void LockTemporary(string username, DateTimeOffset currentDate, DateTimeOffset lockingDate)
    {
        // Validation
        if (lockingDate <= currentDate)
        {
            throw new LockingDateArgumentException();
        }

        if (IsWithdrawn)
        {
            throw new ResourceIsWithdrawnException();
        }

        if (IsLockedAtTheMomentByAnotherUser(username, currentDate))
        {
            throw new ResourceLockedByAnotherUserException();
        }

        if (LockedTo(currentDate) >= lockingDate)
        {
            throw new LockingDateWeakerThanCurrentLockException();
        }

        // Logic
        if (LockedTo(currentDate) < lockingDate)
        {
            LatestLock(currentDate)!.Invalidate();
        }

        Locks.Add(Lock.CreateNew(username, lockingDate));
    }

    public void Unlock(string username, DateTimeOffset currentDate)
    {
        // Validation
        if (IsWithdrawn)
        {
            throw new ResourceIsWithdrawnException();
        }
        
        if (!IsLockedAtTheMoment(currentDate))
        {
            throw new ResourceNotLockedException();
        }

        if (IsLockedAtTheMomentByAnotherUser(username, currentDate))
        {
            throw new ResourceLockedByAnotherUserException();
        }

        // Logic
        GetValidLocks(currentDate).MaxBy(x => x.ValidTo)!.Invalidate();
    }

    // TODO: Maybe `Archive` would be a better name?
    public void Withdraw()
    {
        if (IsWithdrawn)
        {
            throw new ResourceIsAlreadyWithdrawnException();
        }

        IsWithdrawn = true;
    }

    public DateTimeOffset? LockedTo(DateTimeOffset currentDate)
        => LatestLock(currentDate)?.ValidTo;

    public bool IsLockedAtTheMoment(DateTimeOffset validationDate)
        => GetValidLocks(validationDate).Any();

    private Lock? LatestLock(DateTimeOffset currentDate)
        => GetValidLocks(currentDate).MaxBy(x => x.ValidTo);

    private bool IsLockedAtTheMomentByAnotherUser(string username, DateTimeOffset validationDate)
        => GetValidLocks(validationDate).Any(x => !x.Username.Equals(username));

    private IEnumerable<Lock> GetValidLocks(DateTimeOffset validationDate)
        => Locks.Where(x => x.IsValid && x.ValidTo > validationDate);
}
