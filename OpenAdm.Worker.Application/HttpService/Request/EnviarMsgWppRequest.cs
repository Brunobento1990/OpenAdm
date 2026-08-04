namespace OpenAdm.Worker.Application.HttpService.Request;

public class EnviarMsgWppRequest
{
    public string Number { get; set; } = "";
    public string Text { get; set; } = "";
}

public class EnviarMsgWuzApiWppRequest
{
    public string Phone { get; set; } = "";

    public string Body { get; set; } = "";
}