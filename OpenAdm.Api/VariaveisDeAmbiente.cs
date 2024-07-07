using Domain.Pkg.Exceptions;

namespace OpenAdm.Api;

public static class VariaveisDeAmbiente
{
    public static string GetVariavel(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
            throw new ExceptionApi($"Key inválida : {key}");

        return Environment.GetEnvironmentVariable(key) ??
            throw new ExceptionApi($"Variável não encontrada com a Key : {key}");
    }

    public static bool IsDevelopment()
    {
        return Environment.GetEnvironmentVariable("AMBIENTE") == "develop";
    }
}
