namespace OpenAdm.Application.HttpClient.Interfaces;

public interface IUploadImageBlobClient
{
    Task<string> UploadImageAsync(string base64, string nomeFoto);
    Task<bool> DeleteImageAsync(string url);
}
