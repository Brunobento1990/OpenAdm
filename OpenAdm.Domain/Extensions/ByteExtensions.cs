using System.Text;

namespace OpenAdm.Domain.Extensions;

public static class ByteExtensions
{
    public static string? ParaString(this byte[]? bytes)
    {
        if (bytes == null || bytes.Length == 0)
            return null;

        return Convert.ToBase64String(bytes);
    }

    public static byte[]? ParaBytes(this string? base64)
    {
        if (string.IsNullOrWhiteSpace(base64))
            return null;

        return Convert.FromBase64String(base64);
    }
}
