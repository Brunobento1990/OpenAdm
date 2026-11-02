using System.Globalization;

namespace OpenAdm.Domain.Extensions;

public static class DecimalExtension
{
    public static decimal ArredondarCentavos(this decimal valor) =>
        decimal.Round(valor, 2, MidpointRounding.AwayFromZero);

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
        => value.ToString("N2", new CultureInfo("pt-BR"));
}
