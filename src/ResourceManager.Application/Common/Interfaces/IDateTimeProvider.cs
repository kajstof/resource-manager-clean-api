namespace ResourceManager.Application.Common.Interfaces;

public interface IDateTimeProvider
{
    DateTimeOffset Now { get; }
}
