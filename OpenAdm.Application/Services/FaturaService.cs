using OpenAdm.Application.Dtos.FaturasDtos;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.Pagamentos;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Enuns;
using OpenAdm.Domain.Exceptions;
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Application.Services;

public sealed class FaturaService : IFaturaService
{
    private readonly IFaturaRepository _contasAReceberRepository;
    private readonly IPagamentoFactory _pagamentoFactory;
    private readonly IUsuarioAutenticado _usuarioAutenticado;

    public FaturaService(
        IFaturaRepository contasAReceberRepository,
        IPagamentoFactory pagamentoFactory,
        IUsuarioAutenticado usuarioAutenticado)
    {
        _contasAReceberRepository = contasAReceberRepository;
        _pagamentoFactory = pagamentoFactory;
        _usuarioAutenticado = usuarioAutenticado;
    }

    public async Task CriarContasAReceberAsync(CriarFaturaDto contasAReceberDto)
    {
        var fatura = Fatura.NovaContasAReceber(
            usuarioId: contasAReceberDto.UsuarioId,
            pedidoId: contasAReceberDto.PedidoId,
            total: contasAReceberDto.Total,
            quantidadeDeParcelas: contasAReceberDto.QuantidadeDeParcelas,
            primeiroVencimento: contasAReceberDto.DataDoPrimeiroVencimento,
            meioDePagamento: contasAReceberDto.MeioDePagamento,
            desconto: contasAReceberDto.Desconto,
            observacao: contasAReceberDto.Observacao,
            idExterno: null,
            tipo: contasAReceberDto.Tipo);

        await _contasAReceberRepository.AddAsync(fatura);
    }

    public async Task<PagamentoViewModel> GerarPagamentoAsync(MeioDePagamentoEnum meioDePagamento, Guid pedidoId)
    {
        var resultPagamento = await _pagamentoFactory
            .GetPagamento(meioDePagamento)
            .GerarPagamentoAsync(pedidoId);

        var fatura = Fatura
            .NovaContasAReceber(
            usuarioId: _usuarioAutenticado.Id,
            pedidoId: pedidoId,
            total: resultPagamento.Total,
            quantidadeDeParcelas: resultPagamento.QuantidadeDeParcelas,
            primeiroVencimento: resultPagamento.PrimeiroVencimento,
            meioDePagamento: meioDePagamento,
            desconto: null,
            observacao: null,
            idExterno: resultPagamento.MercadoPagoId,
            tipo: TipoFaturaEnum.A_Receber);

        await _contasAReceberRepository.AddAsync(fatura);

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
            .Parcelas
            .Where(x => x.Status == StatusParcelaEnum.Pago).Count() == contasAReceber.Parcelas.Count)
        {
            contasAReceber.Fechar();
            contasAReceber.Parcelas = [];
            await _contasAReceberRepository.UpdateAsync(contasAReceber);
            return;
        }

        if (contasAReceber
            .Parcelas
            .Where(x => x.Status == StatusParcelaEnum.Pago).Count() > 1)
        {
            contasAReceber.PagaParcialmente();
            contasAReceber.Parcelas = [];
            await _contasAReceberRepository.UpdateAsync(contasAReceber);
            return;
        }
    }
}
