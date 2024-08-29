namespace BlueLagoon.Date.Abstractions;

public interface IDateTimeProvider
{
    DateTime Current();

    DateTime CurrentUtc();
}