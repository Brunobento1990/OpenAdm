using OpenAdm.Application.Dtos.ConfiguracoesDePedidos;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.ConfiguracoesDePedidos;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Extensions;
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Application.Services;

public class ConfiguracoesDePedidoService : IConfiguracoesDePedidoService
{
    private readonly IConfiguracoesDePedidoRepository _configuracoesDePedidoRepository;
    private readonly IUsuarioAutenticado _usuarioAutenticado;
    private readonly IConfiguracaoDeFreteService _configuracaoDeFreteService;

    public ConfiguracoesDePedidoService(
        IConfiguracoesDePedidoRepository configuracoesDePedidoRepository,
        IUsuarioAutenticado usuarioAutenticado,
        IConfiguracaoDeFreteService configuracaoDeFreteService)
    {
        _configuracoesDePedidoRepository = configuracoesDePedidoRepository;
        _usuarioAutenticado = usuarioAutenticado;
        _configuracaoDeFreteService = configuracaoDeFreteService;
    }

    public async Task<ConfiguracoesDePedidoViewModel> CreateConfiguracoesDePedidoAsync(
        UpdateConfiguracoesDePedidoDto updateConfiguracoesDePedidoDto)
    {
        var configuracaoDePedido =
            await _configuracoesDePedidoRepository.GetConfiguracoesDePedidoAsync(_usuarioAutenticado.ParceiroId);

        if (configuracaoDePedido == null)
        {
            var date = DateTime.Now;
            configuracaoDePedido = new ConfiguracoesDePedido(
                id: Guid.NewGuid(),
                dataDeCriacao: date,
                dataDeAtualizacao: date,
                numero: 0,
                emailDeEnvio: updateConfiguracoesDePedidoDto.EmailDeEnvio,
                ativo: true,
                pedidoMinimoAtacado: updateConfiguracoesDePedidoDto.PedidoMinimoAtacado,
                pedidoMinimoVarejo: updateConfiguracoesDePedidoDto.PedidoMinimoVarejo,
                parceiroId: _usuarioAutenticado.ParceiroId,
                whatsApp: updateConfiguracoesDePedidoDto.WhatsApp?.LimparMascaraTelefone(),
                vendaDeProdutoComEstoque: updateConfiguracoesDePedidoDto.VendaDeProdutoComEstoque);

            await _configuracoesDePedidoRepository.AddAsync(configuracaoDePedido);
        }
        else
        {
            configuracaoDePedido.Update(
                emailDeEnvio: updateConfiguracoesDePedidoDto.EmailDeEnvio,
                ativo: true,
                pedidoMinimoAtacado: updateConfiguracoesDePedidoDto.PedidoMinimoAtacado,
                pedidoMinimoVarejo: updateConfiguracoesDePedidoDto.PedidoMinimoVarejo,
                whatsApp: updateConfiguracoesDePedidoDto.WhatsApp?.LimparMascaraTelefone(),
                vendaDeProdutoComEstoque: updateConfiguracoesDePedidoDto.VendaDeProdutoComEstoque);

            _configuracoesDePedidoRepository.Update(configuracaoDePedido);
        }

        await _configuracoesDePedidoRepository.SaveChangesAsync();

        return new ConfiguracoesDePedidoViewModel().ToModel(configuracaoDePedido);
    }

    public async Task<ConfiguracoesDePedidoViewModel> GetConfiguracoesDePedidoAsync()
    {
        var configuracaoDePedido =
            await _configuracoesDePedidoRepository.GetConfiguracoesDePedidoAsync(_usuarioAutenticado.ParceiroId);
        if (configuracaoDePedido == null)
        {
            return new()
            {
                VendaDeProdutoComEstoque = false
            };
        }

        return new ConfiguracoesDePedidoViewModel().ToModel(configuracaoDePedido);
    }

    public async Task<PedidoMinimoViewModel> GetPedidoMinimoAsync()
    {
        var configuracaoDePedido =
            await _configuracoesDePedidoRepository.GetConfiguracoesDePedidoAsync(_usuarioAutenticado.ParceiroId);
        if (configuracaoDePedido is null) return new();

        var usuarioViewModel = await _usuarioAutenticado.GetUsuarioAutenticadoAsync();
        var pedidoMinimo = usuarioViewModel.IsAtacado
            ? configuracaoDePedido.PedidoMinimoAtacado
            : configuracaoDePedido.PedidoMinimoVarejo;

        return new PedidoMinimoViewModel()
        {
            PedidoMinimo = pedidoMinimo
        };
    }

    public async Task<TodasConfiguracoesViewModel> GetTodasConfiguracoesAsync()
    {
        var configuracaoDePedido = await GetPedidoMinimoAsync();
        var resultadoConfiguracaoDeFrete = await _configuracaoDeFreteService.ObterAsync();

        var config = new TodasConfiguracoesViewModel()
        {
            PedidoMinimo = configuracaoDePedido.PedidoMinimo
        };

        if (resultadoConfiguracaoDeFrete.Result == null || !resultadoConfiguracaoDeFrete.Result.Ativo)
        {
            config.CobrarFrete = false;
        }
        else
        {
            var usuarioViewModel = await _usuarioAutenticado.GetUsuarioAutenticadoAsync();

            config.CobrarFrete
                = usuarioViewModel.IsAtacado
                    ? resultadoConfiguracaoDeFrete.Result.CobrarCnpj
                    : resultadoConfiguracaoDeFrete.Result.CobrarCpf;
        }

        return config;
    }
}