using OpenAdm.Domain.Exceptions;

namespace OpenAdm.Domain.Helpers;

public class CepHelper
{
    public static bool ValidarCep(string cep)
    {
        cep = cep.Replace("-", "").Trim();
        if (string.IsNullOrWhiteSpace(cep) || cep.Length != 8)
        {
            return false;
        }

        var posicao1 = cep[0].ToString();
        var posicao2 = cep[1].ToString();
        var posicao3 = cep[2].ToString();
        var posicao4 = cep[3].ToString();
        var posicao5 = cep[4].ToString();
        var posicao6 = cep[5].ToString();
        var posicao7 = cep[6].ToString();
        var posicao8 = cep[7].ToString();

        if (!int.TryParse(posicao1, out var _) ||
            !int.TryParse(posicao2, out var _) ||
            !int.TryParse(posicao3, out var _) ||
            !int.TryParse(posicao4, out var _) ||
            !int.TryParse(posicao5, out var _) ||
            !int.TryParse(posicao6, out var _) ||
            !int.TryParse(posicao7, out var _) ||
            !int.TryParse(posicao8, out var _))
        {
            return false;
        }

        return true;
    }
}
