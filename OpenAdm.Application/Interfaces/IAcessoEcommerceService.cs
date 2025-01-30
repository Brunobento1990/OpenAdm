namespace OpenAdm.Application.Interfaces;

public interface IAcessoEcommerceService
{
    Task<long> QuantidadeDeAcessoAsync();
    Task AtualizarAcessoAsync();
}
