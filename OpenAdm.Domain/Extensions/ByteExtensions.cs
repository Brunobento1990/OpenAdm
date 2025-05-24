using System.Text;

namespace OpenAdm.Domain.Extensions;

public static class ByteExtensions
{
    public static string? ParaString(this byte[]? value)
    {
        if (value == null)
        {
            return null;
        }

        return Encoding.UTF8.GetString(value);
    }
}
