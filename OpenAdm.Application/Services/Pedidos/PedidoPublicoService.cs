using OpenAdm.Application.Interfaces.Pedidos;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Exceptions;
using OpenAdm.Domain.Helpers;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Pdf.Interfaces;

namespace OpenAdm.Application.Services.Pedidos;

public sealed class PedidoPublicoService : IPedidoPublicoService
{
    private readonly IEmpresaOpenAdmRepository _empresaRepository;
    private readonly IParceiroAutenticado _parceiroAutenticado;
    private readonly IPedidoPublicoRepository _pedidoRepository;
    private readonly IPdfPedidoService _pdfPedidoService;

    public PedidoPublicoService(
        IEmpresaOpenAdmRepository empresaRepository,
        IParceiroAutenticado parceiroAutenticado,
        IPedidoPublicoRepository pedidoRepository,
        IPdfPedidoService pdfPedidoService)
    {
        _empresaRepository = empresaRepository;
        _parceiroAutenticado = parceiroAutenticado;
        _pedidoRepository = pedidoRepository;
        _pdfPedidoService = pdfPedidoService;
    }

    public async Task<byte[]?> GerarPdfAsync(Guid empresaId, Guid idPublico)
    {
        var resultado = await ObterAsync(empresaId, idPublico);
        return resultado == null
            ? null
            : _pdfPedidoService.GeneratePdfPedido(resultado.Value.Pedido, resultado.Value.Parceiro);
    }

    private async Task<(Pedido Pedido, Parceiro Parceiro)?> ObterAsync(Guid empresaId, Guid idPublico)
    {
        if (empresaId == Guid.Empty || idPublico == Guid.Empty)
            return null;

        var empresa = await _empresaRepository.ObterPorIdAsync(empresaId);
        if (empresa == null)
            return null;

        _parceiroAutenticado.Id = empresa.Id;
        _parceiroAutenticado.ConnectionString = Criptografia.Decrypt(empresa.ConnectionString);

        Parceiro parceiro;
        try
        {
            parceiro = await _parceiroAutenticado.ObterParceiroAutenticadoAsync();
        }
        catch (ExceptionApi)
        {
            return null;
        }

        if (parceiro.EmpresaOpenAdmId != empresaId)
            return null;

        var pedido = await _pedidoRepository.GetPedidoCompletoByIdPublicoAsync(idPublico);
        return pedido == null ? null : (pedido, parceiro);
    }

}
