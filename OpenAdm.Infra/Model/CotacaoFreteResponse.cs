using OpenAdm.Domain.Exceptions;

namespace OpenAdm.Infra.Model;

public class CotacaoFreteResponse
{
    public string Valorpac { get; set; } = string.Empty;
    public string Prazopac { get; set; } = string.Empty;
    public string Valorsedex { get; set; } = string.Empty;
    public string Prazosedex { get; set; } = string.Empty;

    public void Validar()
    {
        if (!decimal.TryParse(Valorpac, out var _) || !decimal.TryParse(Valorsedex, out var _))
        {
            throw new ExceptionApi("Não foi possível cotar o frete", enviarErroDiscord: true);
        }
    }
}
