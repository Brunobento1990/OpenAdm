using OpenAdm.Domain.Exceptions;

namespace OpenAdm.Domain.Helpers;

public static class DominioEmpresaHelper
{
    public static string Normalizar(string origem)
    {
        if (!Uri.TryCreate(origem, UriKind.Absolute, out var uri) || string.IsNullOrWhiteSpace(uri.Host))
            throw new ExceptionApi("Não foi possível identificar o domínio da empresa");

        var partes = uri.Host.Split('.', StringSplitOptions.RemoveEmptyEntries);
        return partes.Length > 3 ? string.Join('.', partes.Skip(1)) : uri.Host.ToLowerInvariant();
    }
}
