namespace ResourceManager.Application.DTOs;

public record ResourceDto(Guid Id, string Name, bool IsBlocked, DateTimeOffset? BlockedTo);
