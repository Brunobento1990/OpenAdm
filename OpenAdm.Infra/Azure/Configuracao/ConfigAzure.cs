namespace OpenAdm.Infra.Azure.Configuracao;

public static class ConfigAzure
{
    public static string Key { get; private set; } = string.Empty;
    public static string Container { get; private set; } = string.Empty;

    public static void Configure(string key, string container)
    {
        Key = key;
        Container = container;
    }
}
