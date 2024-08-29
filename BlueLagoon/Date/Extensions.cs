using BlueLagoon.Date.Abstractions;
using BlueLagoon.Tools;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.InteropServices;

namespace BlueLagoon.Date;

public static class Extensions
{
    public static DateTime ConvertDate(this DateTime date)
        => TimeZoneInfo.ConvertTimeFromUtc(date, GetTimeZoneInfo());

    public static TimeZoneInfo GetTimeZoneInfo()
        => RuntimeInformation.IsOSPlatform(OSPlatform.Windows) 
                        ? TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time") 
                        : TimeZoneInfo.FindSystemTimeZoneById("CET");

    public static DateTime SetAsMsSqlMinDate(this DateTime date)
    {
        var minDate = date >= DateTime.MinValue.AddYears(1900) 
                                ? date.ToString("yyyy-MM-dd HH:mm:ss") 
                                : date.AddYears(1900).ToString("yyyy-MM-dd HH:mm:ss");

        return Convert.ToDateTime(minDate);
    }

    public static DateTime ConvertToMsSqlFormat(this DateTime date)
        => Convert.ToDateTime(date.ToString("yyyy-MM-dd HH:mm:ss"));

    public static DateTime UnixTimeStampToDateTime(this long unixTimeStamp)
    {
        DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToUniversalTime();

        return dtDateTime;
    }

    public static string ConvertToElapsedTime(this DateTime dateTime)
    {
        DateTime utcNow = DateTime.UtcNow.ConvertDate();

        var subtractionResult = utcNow - dateTime;

        string result = string.Empty;

        if (subtractionResult.Hours > 0)
        {
            result = string.Concat(subtractionResult.Hours, " ", NounsGradator.GradateTheNounAccordingToQuantity(subtractionResult.Hours, "godzina", "godziny", "godzin"));

            if (subtractionResult.Minutes > 0)
            {
                result = string.Concat(result, " i ", subtractionResult.Minutes, " ", NounsGradator.GradateTheNounAccordingToQuantity(subtractionResult.Minutes, "minuta", "minuty", "minut"));

                return result;
            }
        }

        return subtractionResult.Minutes > 0
                                    ? string.Concat(subtractionResult.Minutes, " ", NounsGradator.GradateTheNounAccordingToQuantity(subtractionResult.Minutes, "minuta", "minuty", "minut"))
                                    : string.Concat(subtractionResult.Seconds, " ", NounsGradator.GradateTheNounAccordingToQuantity(subtractionResult.Minutes, "sekunda", "sekundy", "sekund"));
    }

    public static IServiceCollection AddDateTimeProvider(this IServiceCollection services)
        => services.AddTransient<IDateTimeProvider, DateTimeProvider>();
}
