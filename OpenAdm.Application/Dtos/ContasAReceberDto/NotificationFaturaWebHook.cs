namespace OpenAdm.Application.Dtos.ContasAReceberDto;

public class NotificationFaturaWebHook
{
    public string Action { get; set; } = string.Empty;
    public string Api_version { get; set; } = string.Empty;
    public Data Data { get; set; } = null!;
    public DateTime Date_created { get; set; }
    public long Id { get; set; }
    public bool Live_mode { get; set; }
    public string Type { get; set; } = string.Empty;
    public string User_id { get; set; } = string.Empty;
}

public class Data
{
    public string Id { get; set; } = string.Empty;
}
