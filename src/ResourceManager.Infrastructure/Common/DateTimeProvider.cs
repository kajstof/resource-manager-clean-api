using ResourceManager.Application.Common.Interfaces;

namespace ResourceManager.Infrastructure.Common;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTimeOffset Now => DateTimeOffset.Now;
}
