using System.Net;

namespace OpenAdm.Domain.Model;

public class ResultPartner<T> where T : class
{
    public T? Result { get; set; }
    public string? Error { get; set; }
    public bool WithError => !string.IsNullOrWhiteSpace(Error);

    public static explicit operator ResultPartner<T>(T result) => new() { Result = result };
    public static explicit operator ResultPartner<T>(string error) => new() { Error = error };
}
