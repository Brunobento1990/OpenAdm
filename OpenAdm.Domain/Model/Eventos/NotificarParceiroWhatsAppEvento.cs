namespace OpenAdm.Domain.Model.Eventos;

public class NotificarParceiroWhatsAppEvento : EventoBase
{
    public string Mensagem { get; set; } = string.Empty;
}