using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Infra.Model;

public class ParceiroAutenticado : IParceiroAutenticado
{
    public string StringConnection { get; set; } = string.Empty;
    public string Referer { get; set; } = string.Empty;
    public string KeyParceiro { get; set; } = string.Empty;
    public string NomeFantasia { get; set; } = string.Empty;
}
