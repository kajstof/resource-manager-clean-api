using ResourceManager.Domain.Common;

namespace ResourceManager.Domain.Resource;

public class Resource : IAggregateRoot
{
    public Guid Id { get; private set; }

    public string Name { get; private set; } = null!;

    public bool IsWithdrawn { get; private set; }

    public DateTimeOffset? BlockedTo => Locks
        .Where(x => x.IsValid)
        .MaxBy(x => x.ValidTo)?.ValidTo ?? null;

    public ICollection<Lock> Locks { get; private set; } = new List<Lock>();

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

    public bool IsBlockedAtTheMoment(DateTimeOffset currentDateTimeOffset)
    {
        return Locks.Any(x => x.IsValid && x.ValidTo < currentDateTimeOffset);
    }

    public void LockPermanently(string username, DateTimeOffset currentDateTimeOffset)
    {
        ValidateAndThrow(currentDateTimeOffset);
    }
    public void LockTemporary(string username, DateTimeOffset currentDateTimeOffset, DateTimeOffset blockTo)
    {
        ValidateAndThrow(currentDateTimeOffset);
    }

    public void Unlock(string username, DateTimeOffset currentDateTimeOffset)
    {
        if (!IsBlockedAtTheMoment(currentDateTimeOffset))
        {
            throw new DomainException("This resource is not blocked");
        }

        string lockedByUser = Locks.Where(x => x.IsValid).MaxBy(x => x.ValidTo)!.Username;
        if (!username.Equals(lockedByUser))
        {
            throw new DomainException("This resource is not blocked by you");
        }
    }

    public void Withdraw()
    {
        IsWithdrawn = true;
    }

    private void ValidateAndThrow(DateTimeOffset currentDateTimeOffset)
    {
        if (IsWithdrawn)
        {
            throw new DomainException("This resource is withdrawn");
        }

        if (IsBlockedAtTheMoment(currentDateTimeOffset))
        {
            throw new DomainException("This resource is locked");
        }
    }

}
