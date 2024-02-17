namespace OpenAdm.Application.Services;

public class GenerateSenha
{
    public static string Generate()
    {
        return new Random().Next(80000, 999999).ToString();
    }
}
