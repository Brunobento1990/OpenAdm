using OpenAdm.Application.Dtos.ContasAReceberDto;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.Pagamentos;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Enuns;
using OpenAdm.Domain.Exceptions;
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Application.Services;

public sealed class ContasAReceberService : IContasAReceberService
{
    private readonly IContasAReceberRepository _contasAReceberRepository;
    private readonly IPagamentoFactory _pagamentoFactory;
    private readonly IUsuarioAutenticado _usuarioAutenticado;

    public ContasAReceberService(
        IContasAReceberRepository contasAReceberRepository,
        IPagamentoFactory pagamentoFactory,
        IUsuarioAutenticado usuarioAutenticado)
    {
        _contasAReceberRepository = contasAReceberRepository;
        _pagamentoFactory = pagamentoFactory;
        _usuarioAutenticado = usuarioAutenticado;
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
            observacao: contasAReceberDto.Observacao,
            idExterno: null);

        await _contasAReceberRepository.AddAsync(contasAReceber);
    }

    public async Task<PagamentoViewModel> GerarPagamentoAsync(MeioDePagamentoEnum meioDePagamento, Guid pedidoId)
    {
        var resultPagamento = await _pagamentoFactory
            .GetPagamento(meioDePagamento)
            .GerarPagamentoAsync(pedidoId);

        var contasAReceber = ContasAReceber
            .NovaContasAReceber(
            usuarioId: _usuarioAutenticado.Id,
            pedidoId: pedidoId,
            total: resultPagamento.Total,
            quantidadeDeParcelas: resultPagamento.QuantidadeDeParcelas,
            primeiroVencimento: resultPagamento.PrimeiroVencimento,
            meioDePagamento: meioDePagamento,
            desconto: null,
            observacao: null,
            idExterno: resultPagamento.MercadoPagoId);

        await _contasAReceberRepository.AddAsync(contasAReceber);

        return new()
        {
            LinkPagamento = resultPagamento.LinkPagamento,
            QrCodePix = resultPagamento.QrCodePix,
            QrCodePixBase64 = resultPagamento.QrCodePixBase64
        };
    }

    public async Task VerificarFechamentoAsync(Guid id)
    {
        var contasAReceber = await _contasAReceberRepository.GetByIdAsync(id)
            ?? throw new ExceptionApi("Não foi possível localizar a contas a pagar");

        if (contasAReceber
            .Faturas
            .Where(x => x.Status == StatusFaturaContasAReceberEnum.Pago).Count() == contasAReceber.Faturas.Count)
        {
            contasAReceber.Fechar();
            contasAReceber.Faturas = [];
            await _contasAReceberRepository.UpdateAsync(contasAReceber);
            return;
        }

        if (contasAReceber
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
