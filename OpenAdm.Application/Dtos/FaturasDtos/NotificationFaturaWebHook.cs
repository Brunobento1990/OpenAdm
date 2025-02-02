namespace OpenAdm.Application.Dtos.FaturasDtos;

public class NotificationFaturaWebHook
{
    public string? Action { get; set; } = string.Empty;
    public Data? Data { get; set; } = null!;
}

public class Data
{
    public string? Id { get; set; } = string.Empty;
}
