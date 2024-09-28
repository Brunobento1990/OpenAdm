using Azure.Storage.Blobs;
using OpenAdm.Domain.Exceptions;
using OpenAdm.Infra.Azure.Configuracao;
using OpenAdm.Infra.Azure.Interfaces;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

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

        var fotoBytes = ComprimirImagem(base64);

        using var foto = new MemoryStream(fotoBytes);

        var blobCliente = new BlobClient(ConfigAzure.Key, ConfigAzure.Container, nomeFoto);

        await blobCliente.UploadAsync(foto);

        return blobCliente.Uri.AbsoluteUri;
    }

    internal static byte[] ComprimirImagem(string base64)
    {

        byte[] imageBytes = Convert.FromBase64String(base64);

        using var inStream = new MemoryStream(imageBytes);

        using var image = Image.Load(inStream);

        int maxWidth = 1200;
        int maxHeight = 1200;
        if (image.Width > maxWidth || image.Height > maxHeight)
        {
            if (image.Width > maxWidth && image.Height > maxHeight)
            {
                image.Mutate(x => x.Resize(maxWidth, maxHeight));
            }

            if(image.Width > maxWidth && image.Height <= maxHeight)
            {
                image.Mutate(x => x.Resize(maxWidth, image.Height));
            }

            if (image.Height > maxHeight && image.Width <= maxWidth)
            {
                image.Mutate(x => x.Resize(image.Width, maxHeight));
            }
        }

        var encoder = new SixLabors.ImageSharp.Formats.Jpeg.JpegEncoder
        {
            Quality = 100
        };

        using var outputStream = new MemoryStream();

        image.Save(outputStream, encoder);

        return outputStream.ToArray();
    }
}
