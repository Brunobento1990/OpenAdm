using OpenAdm.Domain.Enuns;

namespace OpenAdm.Application.Interfaces;

public interface IPagamentoFactory
{
    IPagamentoService GetPagamento(MeioDePagamentoEnum meioDePagamentoEnum);
}
