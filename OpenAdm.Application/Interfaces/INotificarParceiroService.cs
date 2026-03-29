namespace OpenAdm.Application.Interfaces;

public interface INotificarParceiroService
{
    Task<bool> NotificarViaWhatsAppAsync(Guid parceiroId, string msg);
}