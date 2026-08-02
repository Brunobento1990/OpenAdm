using System.Globalization;

namespace OpenAdm.Domain.Extensions;

public static class DecimalExtension
{
    public static string FormatMoney(this decimal value, bool temSimboloDeDinheiro = false)
    {
        var currency = temSimboloDeDinheiro ? "R$" : "";
        if (value == 0)
        {
            return $"{currency} 0,00";
        }

        bool isNegative = value < 0;
        decimal absValue = Math.Abs(value);
        string formattedValue = absValue.ToString("N2", new CultureInfo("pt-BR"));

        return isNegative
            ? $"{currency} -{formattedValue}"
            : $"{currency} {formattedValue}";
    }

    public static string ConverterPadraoBrasileiro(this decimal value)
    {
        var newValue = value.ToString().Split(".");
        if (newValue?.Length > 0)
        {
            var numero = newValue.ElementAtOrDefault(0)?.Length > 0 ? newValue.ElementAtOrDefault(0) : "0";
            var decimals = newValue.ElementAtOrDefault(1);
            if (!string.IsNullOrWhiteSpace(decimals))
            {
                if (decimals.Length != 2)
                {
                    decimals = decimals.Length > 2 ? decimals[..2] : $"{decimals}0";
                }
            }
            else
            {
                decimals = "00";
            }

            return $"{numero},{decimals}";
        }
        else
        {
            return "0,00";
        }
    }
}
