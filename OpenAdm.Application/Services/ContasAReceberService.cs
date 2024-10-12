using OpenAdm.Application.Dtos.ContasAReceberDto;
using OpenAdm.Application.Interfaces;
using OpenAdm.Domain.Entities;
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
}
