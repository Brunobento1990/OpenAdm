using OpenAdm.Application.Dtos.FaturasDtos;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.ContasAReceberModel;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Enuns;
using OpenAdm.Domain.Exceptions;
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Application.Services;

public sealed class FaturaService : IFaturaService
{
    private readonly IFaturaRepository _contasAReceberRepository;
    private readonly IUsuarioService _usuarioService;
    public FaturaService(
        IFaturaRepository contasAReceberRepository,
        IUsuarioService usuarioService)
    {
        _contasAReceberRepository = contasAReceberRepository;
        _usuarioService = usuarioService;
    }

    public async Task<FaturaViewModel> CriarAdmAsync(FaturaCriarAdmDto faturaCriarAdmDto)
    {
        _ = await _usuarioService.GetUsuarioByIdAdmAsync(id: faturaCriarAdmDto.UsuarioId);

        var fatura = new Fatura(
            id: Guid.NewGuid(),
            dataDeCriacao: DateTime.Now,
            dataDeAtualizacao: DateTime.Now,
            numero: 0,
            status: StatusFaturaEnum.Aberta,
            usuarioId: faturaCriarAdmDto.UsuarioId,
            pedidoId: faturaCriarAdmDto.PedidoId,
            dataDeFechamento: null,
            tipo: faturaCriarAdmDto.Tipo);

        foreach (var parcelaDto in faturaCriarAdmDto.Parcelas)
        {
            fatura.Parcelas.Add(new Parcela(
                id: Guid.NewGuid(),
                dataDeCriacao: DateTime.Now,
                dataDeAtualizacao: DateTime.Now,
                numero: 0,
                dataDeVencimento: parcelaDto.DataDeVencimento,
                numeroDaParcela: parcelaDto.NumeroDaParcela,
                meioDePagamento: parcelaDto.MeioDePagamento,
                valor: parcelaDto.Valor,
                desconto: parcelaDto.Desconto,
                observacao: parcelaDto.Observacao,
                faturaId: fatura.Id,
                idExterno: null));
        }

        await _contasAReceberRepository.AddAsync(fatura);

        return (FaturaViewModel)fatura;
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

    public async Task<FaturaViewModel> GetByIdAsync(Guid id)
    {
        var fatura = await _contasAReceberRepository.GetByIdAsync(id)
            ?? throw new ExceptionApi("Não foi possível localizar a fatura!");
        return (FaturaViewModel)fatura;
    }

    public async Task<FaturaViewModel> GetCompletaAsync(Guid id)
    {
        var fatura = await _contasAReceberRepository.GetByIdCompletaAsync(id)
            ?? throw new ExceptionApi("Não foi possível localizar a fatura!");

        return (FaturaViewModel)fatura;
    }

    public async Task VerificarFechamentoAsync(Guid id)
    {
        var contasAReceber = await _contasAReceberRepository.GetByIdAsync(id)
            ?? throw new ExceptionApi("Não foi possível localizar a contas a pagar");

        if (contasAReceber
            .Parcelas.Count() == contasAReceber.Parcelas.Count)
        {
            contasAReceber.Fechar();
            contasAReceber.Parcelas = [];
            await _contasAReceberRepository.UpdateAsync(contasAReceber);
            return;
        }

        if (contasAReceber
            .Parcelas
            .Count() > 1)
        {
            contasAReceber.PagaParcialmente();
            contasAReceber.Parcelas = [];
            await _contasAReceberRepository.UpdateAsync(contasAReceber);
            return;
        }
    }
}
