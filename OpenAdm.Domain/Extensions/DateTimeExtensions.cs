namespace OpenAdm.Domain.Extensions;

public static class DateTimeExtensions
{
    public static string DateTimeToString(this DateTime dateTime)
    {
        return dateTime.ToString("dd/MM/yyyy HH:mm");
    }

    public static string DateTimeSomenteDataToString(this DateTime dateTime)
    {
        return dateTime.ToString("dd/MM/yyyy");
    }

    public static string FormatarDataJson(this DateTime dateTime)
    {
        return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
    }

    public static bool EhMadrugada(this DateTime dateTime)
    {
        return dateTime.Hour >= 0 && dateTime.Hour < 6;
    }

    public static bool EhFimDeSemana(this DateTime dateTime)
    {
        return dateTime.DayOfWeek == DayOfWeek.Saturday ||
               dateTime.DayOfWeek == DayOfWeek.Sunday;
    }
}