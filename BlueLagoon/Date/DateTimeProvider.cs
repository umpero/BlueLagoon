using BlueLagoon.Date.Abstractions;

namespace BlueLagoon.Date;
internal sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTime Current() => DateTime.UtcNow.ConvertDate();

    public DateTime CurrentUtc() => DateTime.UtcNow;
}
