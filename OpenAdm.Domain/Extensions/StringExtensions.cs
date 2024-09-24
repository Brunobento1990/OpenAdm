namespace OpenAdm.Domain.Extensions;

public static class StringExtensions
{
    public static string FormatPhone(this string value)
    {
        if (value.Length != 11)
            return string.Empty;

        return string.Format("({0}) {1}-{2}",
                             value[..2],
                             value.Substring(2, 5),
                             value.Substring(7, 4));
    }

    public static string FormatCpf(this string value)
    {
        return Convert.ToUInt64(value).ToString(@"000\.000\.000\-00");
    }

    public static string FormatCnpj(this string value)
    {
        return Convert.ToUInt64(value).ToString(@"00\.000\.000\/0000\-00");
    }
}
