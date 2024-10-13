using OpenAdm.Application.Dtos.ContasAReceberDto;
using OpenAdm.Application.Interfaces;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Enuns;
using OpenAdm.Domain.Exceptions;
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Application.Services;

public sealed class ContasAReceberService : IContasAReceberService
{
    private readonly IContasAReceberRepository _contasAReceberRepository;

    public ContasAReceberService(IContasAReceberRepository contasAReceberRepository)
    {
        _contasAReceberRepository = contasAReceberRepository;
    }

    public async Task CriarContasAReceberAsync(CriarContasAReceberDto contasAReceberDto)
    {
        var contasAReceber = ContasAReceber.NovaContasAReceber(
            usuarioId: contasAReceberDto.UsuarioId,
            pedidoId: contasAReceberDto.PedidoId,
            total: contasAReceberDto.Total,
            quantidadeDeParcelas: contasAReceberDto.QuantidadeDeParcelas,
            primeiroVencimento: contasAReceberDto.DataDoPrimeiroVencimento,
            meioDePagamento: contasAReceberDto.MeioDePagamento,
            desconto: contasAReceberDto.Desconto,
            observacao: contasAReceberDto.Observacao);

        await _contasAReceberRepository.AddAsync(contasAReceber);
    }

    public async Task VerificarFechamentoAsync(Guid id)
    {
        var contasAReceber = await _contasAReceberRepository.GetByIdAsync(id)
            ?? throw new ExceptionApi("Não foi possível localizar a contas a pagar");

        if(contasAReceber
            .Faturas
            .Where(x => x.Status == StatusFaturaContasAReceberEnum.Pago).Count() == contasAReceber.Faturas.Count)
        {
            contasAReceber.Fechar();
            contasAReceber.Faturas = [];
            await _contasAReceberRepository.UpdateAsync(contasAReceber);
            return;
        }

        if(contasAReceber
            .Faturas
            .Where(x => x.Status == StatusFaturaContasAReceberEnum.Pago).Count() > 1)
        {
            contasAReceber.PagaParcialmente();
            contasAReceber.Faturas = [];
            await _contasAReceberRepository.UpdateAsync(contasAReceber);
            return;
        }
    }
}
