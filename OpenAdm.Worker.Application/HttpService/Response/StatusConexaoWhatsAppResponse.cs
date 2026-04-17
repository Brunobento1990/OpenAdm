namespace OpenAdm.Worker.Application.HttpService.Response;

public class StatusConexaoWhatsAppResponse
{
    public InstanciaStatusConexaoWhatsAppResponse? Instance { get; set; }
}

public class InstanciaStatusConexaoWhatsAppResponse
{
    public string? State { get; set; }
    public bool Conectado => State?.ToLower() == "connected" || State?.ToLower() == "open";
}