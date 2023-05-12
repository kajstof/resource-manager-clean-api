using ResourceManager.Domain.Common;

namespace ResourceManager.Domain.Resource;

public class Lock : IEntity
{
    public Guid Id { get; private set; }

    public bool IsValid { get; private set; }

    public DateTimeOffset ValidTo { get; private set; }

    public string Username { get; private set; } = default!;

    private Lock()
    {
    }

    public static Lock CreateNew(string username, DateTimeOffset validTo)
    {
        return new Lock
        {
            Id = Guid.NewGuid(),
            IsValid = true,
            ValidTo = validTo,
            Username = username
        };
    }

    public void Invalidate()
    {
        IsValid = false;
    }
}
