using ResourceManager.Domain.Resources;

namespace ResourceManager.Application.DTOs;

public record ResourceDto(Guid Id, string Name, bool IsLocked, DateTimeOffset? LockedTo)
{
    public static ResourceDto FromResource(Resource resource, DateTimeOffset currentDate)
    {
        return new ResourceDto(
            resource.Id,
            resource.Name,
            resource.IsLockedAtTheMoment(currentDate),
            resource.LockedTo(currentDate));
    }
};
