namespace OpenAdm.Domain.Interfaces;

public interface IConfiguracaoParceiroRepository
{
    Task<string> GetConexaoDbByDominioAsync();
}
