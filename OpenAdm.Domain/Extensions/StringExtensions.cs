using System.Text;
using OpenAdm.Domain.Exceptions;

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

    public static string Limitar(this string value, int length)
    {
        if (string.IsNullOrEmpty(value))
        {
            return string.Empty;
        }

        if (value.Length <= length)
        {
            return value;
        }

        return value[..length];
    }

    public static string RemoverAcentos(this string text)
    {
        string withDiacritics = "ÄÅÁÂÀÃäáâàãÉÊËÈéêëèÍÎÏÌíîïìÖÓÔÒÕöóôòõÜÚÛüúûùÇç";
        string withoutDiacritics = "AAAAAAaaaaaEEEEeeeeIIIIiiiiOOOOOoooooUUUuuuuCc";
        for (int i = 0; i < withDiacritics.Length; i++)
        {
            text = text.Replace(withDiacritics[i].ToString(), withoutDiacritics[i].ToString());
        }
        return text;
    }

    public static string FormatCpf(this string value)
    {
        return Convert.ToUInt64(value).ToString(@"000\.000\.000\-00");
    }

    public static string? LimparMascaraCpf(this string? cpf)
    {
        return string.IsNullOrWhiteSpace(cpf)
            ? null
            : cpf.Replace(".", "").Replace("-", "").Replace(" ", "").Trim();
    }

    public static string? NullSeVazio(this string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }

    public static string LimparMascaraTelefone(this string telefone)
    {
        return telefone.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "").Trim();
    }

    public static string LimparMascaraCnpj(this string cnpj)
    {
        return cnpj.Replace("-", "").Replace(".", "").Replace("/", "").Replace(" ", "").Trim();
    }

    public static string FormatCnpj(this string value)
    {
        return Convert.ToUInt64(value).ToString(@"00\.000\.000\/0000\-00");
    }

    public static string FromBytes(this byte[] value)
    {
        return Encoding.UTF8.GetString(value);
    }

    public static string ValidarNullOrEmpty(this string? value, string erro)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ExceptionApi(erro);
        }

        return value;
    }

    public static string? ValidarLength(this string? value, int length, string? campo = null)
    {
        if (value?.Length > length)
        {
            throw new ExceptionApi($"O campo {campo ?? nameof(value)} deve ter no máximo {length} caracteres");
        }

        return value;
    }
}
