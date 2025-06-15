using OpenAdm.Domain.Entities.OpenAdm;

namespace OpenAdm.Application.Models.Parceiros;

public class TelefoneParceiroViewModel
{
    public Guid Id { get; set; }
    public string Telefone { get; set; } = string.Empty;

    public static explicit operator TelefoneParceiroViewModel(TelefoneParceiro telefoneParceiro)
    {
        return new TelefoneParceiroViewModel()
        {
            Id = telefoneParceiro.Id,
            Telefone = telefoneParceiro.Telefone
        };
    }
}
