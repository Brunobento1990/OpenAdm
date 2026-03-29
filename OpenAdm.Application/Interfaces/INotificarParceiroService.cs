namespace OpenAdm.Application.Interfaces;

public interface INotificarParceiroService
{
    Task NotificarViaWhatsAppAsync(string msg);
}