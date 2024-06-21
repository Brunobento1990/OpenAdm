namespace OpenAdm.Application.Dtos.Logs;

public class CreateAppLog
{
    public Guid XApi { get; set; }
    public string Host { get; set; } = string.Empty;
    public string Origem { get; set; } = string.Empty;
    public string? Latitude { get; set; } = string.Empty;
    public string? Longitude { get; set; } = string.Empty;
    public string Ip { get; set; } = string.Empty;
    public string Path { get; set; } = string.Empty;
    public string? Erro { get; set; }
    public int StatusCode { get; set; }
    public int LogLevel { get; set; }
}
