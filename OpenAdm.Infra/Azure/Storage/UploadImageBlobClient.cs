using Azure.Storage.Blobs;
using OpenAdm.Application.HttpClient.Interfaces;
using OpenAdm.Domain.Exceptions;
using OpenAdm.Infra.Azure.Configuracao;

namespace OpenAdm.Infra.Azure.Storage;

public class UploadImageBlobClient : IUploadImageBlobClient
{
    public async Task<bool> DeleteImageAsync(string nomeFoto)
    {
        try
        {
            var blobCliente = new BlobClient(ConfigAzure.Key, ConfigAzure.Container, nomeFoto);

            await blobCliente.DeleteIfExistsAsync();

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
    }

    public async Task<string> UploadImageAsync(string base64, string nomeFoto)
    {
        if (string.IsNullOrWhiteSpace(base64))
        {
            throw new ExceptionApi("A imagem selecionada é inválida!");
        }

        var fotoBytes = Convert.FromBase64String(base64);

        using var foto = new MemoryStream(fotoBytes);

        var blobCliente = new BlobClient(ConfigAzure.Key, ConfigAzure.Container, nomeFoto);

        await blobCliente.UploadAsync(foto);

        return blobCliente.Uri.AbsoluteUri;
    }
}
