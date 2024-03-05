using Azure.Storage.Blobs;
using OpenAdm.Infra.Azure.Configuracao;
using OpenAdm.Infra.Azure.Interfaces;

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
        var fotoBytes = Convert.FromBase64String(base64);

        using var foto = new MemoryStream(fotoBytes);

        var blobCliente = new BlobClient(ConfigAzure.Key, ConfigAzure.Container, nomeFoto);

        await blobCliente.UploadAsync(foto);

        return blobCliente.Uri.AbsoluteUri;
    }
}
