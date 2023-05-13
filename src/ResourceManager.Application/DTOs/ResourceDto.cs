using ResourceManager.Domain.Resources;

namespace ResourceManager.Application.DTOs;

public class ResourceDto
{
    public static ResourceDto FromResource(Resource resource, DateTimeOffset currentDate)
    {
        return new ResourceDto
        {
            Id = resource.Id,
            Name = resource.Name,
            IsLocked = resource.IsLockedAtTheMoment(currentDate),
            LockedTo = resource.LockedTo(currentDate),
            IsWithdrawn = resource.IsWithdrawn
        };
    }

    public Guid Id { get; init; }

    public string Name { get; init; } = null!;

    public bool IsLocked { get; init; }

    public DateTimeOffset? LockedTo { get; init; }

    public bool IsWithdrawn { get; init; }
};
