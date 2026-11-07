using OpenAdm.Application.Dtos.ConfiguracaoDoSistema;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.ConfiguracaoDoSistema;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Extensions;
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Application.Services;

public class ConfiguracaoDoSistemaService : IConfiguracaoDoSistemaService
{
    private readonly IConfiguracaoDoSistemaRepository _configuracaoDoSistemaRepository;
    private readonly IUsuarioAutenticado _usuarioAutenticado;
    private readonly IConfiguracaoDeFreteService _configuracaoDeFreteService;

    public ConfiguracaoDoSistemaService(
        IConfiguracaoDoSistemaRepository configuracoesDePedidoRepository,
        IUsuarioAutenticado usuarioAutenticado,
        IConfiguracaoDeFreteService configuracaoDeFreteService)
    {
        _configuracaoDoSistemaRepository = configuracoesDePedidoRepository;
        _usuarioAutenticado = usuarioAutenticado;
        _configuracaoDeFreteService = configuracaoDeFreteService;
    }

    public async Task<ConfiguracaoDoSistema> ConfiguracaoDoSistemaAsync()
    {
        var configuracaoDoSistema =
            await _configuracaoDoSistemaRepository.GetConfiguracaoDoSistemaAsync(_usuarioAutenticado.ParceiroId);
        if (configuracaoDoSistema == null)
        {
            return new ConfiguracaoDoSistema(
                id: Guid.NewGuid(),
                dataDeCriacao: DateTime.UtcNow,
                dataDeAtualizacao: DateTime.UtcNow,
                numero: 0,
                emailParaEnvioNotificacao: "",
                ativo: true,
                pedidoMinimoAtacado: 0,
                pedidoMinimoVarejo: 0,
                parceiroId: _usuarioAutenticado.ParceiroId,
                whatsAppParaEnvioNotificacao: "",
                vendaDeProdutoComEstoque: false,
                listarProdutoSomenteComEstoque: false,
                mensagemPedidoFinalizado: null,
                notificarCadastroNovoCliente: false);
        }

        return configuracaoDoSistema;
    }

    public async Task<ConfiguracaoDoSistemaViewModel> CreateConfiguracaoDoSistemaAsync(
        UpdateConfiguracaoDoSistemaDto updateConfiguracaoDoSistemaDto)
    {
        var configuracaoDoSistema =
            await _configuracaoDoSistemaRepository.GetConfiguracaoDoSistemaAsync(_usuarioAutenticado.ParceiroId);

        if (configuracaoDoSistema == null)
        {
            var date = DateTime.UtcNow;
            configuracaoDoSistema = new ConfiguracaoDoSistema(
                id: Guid.NewGuid(),
                dataDeCriacao: date,
                dataDeAtualizacao: date,
                numero: 0,
                emailParaEnvioNotificacao: updateConfiguracaoDoSistemaDto.EmailParaEnvioNotificacao,
                ativo: true,
                pedidoMinimoAtacado: updateConfiguracaoDoSistemaDto.PedidoMinimoAtacado,
                pedidoMinimoVarejo: updateConfiguracaoDoSistemaDto.PedidoMinimoVarejo,
                parceiroId: _usuarioAutenticado.ParceiroId,
                whatsAppParaEnvioNotificacao: updateConfiguracaoDoSistemaDto.WhatsAppParaEnvioNotificacao
                    ?.LimparMascaraTelefone(),
                vendaDeProdutoComEstoque: updateConfiguracaoDoSistemaDto.VendaDeProdutoComEstoque,
                listarProdutoSomenteComEstoque: updateConfiguracaoDoSistemaDto.ListarProdutoSomenteComEstoque,
                mensagemPedidoFinalizado: updateConfiguracaoDoSistemaDto.MensagemPedidoFinalizado,
                notificarCadastroNovoCliente: updateConfiguracaoDoSistemaDto.NotificarCadastroNovoCliente);

            await _configuracaoDoSistemaRepository.AddAsync(configuracaoDoSistema);
        }
        else
        {
            configuracaoDoSistema.Update(
                emailDeEnvio: updateConfiguracaoDoSistemaDto.EmailParaEnvioNotificacao,
                ativo: true,
                pedidoMinimoAtacado: updateConfiguracaoDoSistemaDto.PedidoMinimoAtacado,
                pedidoMinimoVarejo: updateConfiguracaoDoSistemaDto.PedidoMinimoVarejo,
                whatsAppParaEnvioNotificacao: updateConfiguracaoDoSistemaDto.WhatsAppParaEnvioNotificacao
                    ?.LimparMascaraTelefone(),
                vendaDeProdutoComEstoque: updateConfiguracaoDoSistemaDto.VendaDeProdutoComEstoque,
                listarProdutoSomenteComEstoque: updateConfiguracaoDoSistemaDto.ListarProdutoSomenteComEstoque,
                mensagemPedidoFinalizado: updateConfiguracaoDoSistemaDto.MensagemPedidoFinalizado,
                notificarCadastroNovoCliente: updateConfiguracaoDoSistemaDto.NotificarCadastroNovoCliente);

            _configuracaoDoSistemaRepository.Update(configuracaoDoSistema);
        }

        await _configuracaoDoSistemaRepository.SaveChangesAsync();

        return new ConfiguracaoDoSistemaViewModel().ToModel(configuracaoDoSistema);
    }

    public async Task<ConfiguracaoDoSistemaViewModel> GetConfiguracaoDoSistemaAsync()
    {
        var configuracaoDoSistema =
            await _configuracaoDoSistemaRepository.GetConfiguracaoDoSistemaAsync(_usuarioAutenticado.ParceiroId);
        if (configuracaoDoSistema == null)
        {
            return new()
            {
                VendaDeProdutoComEstoque = false,
                ListarProdutoSomenteComEstoque = false
            };
        }

        return new ConfiguracaoDoSistemaViewModel().ToModel(configuracaoDoSistema);
    }

    public async Task<PedidoMinimoViewModel> GetPedidoMinimoAsync()
    {
        var configuracaoDoSistema =
            await _configuracaoDoSistemaRepository.GetConfiguracaoDoSistemaAsync(_usuarioAutenticado.ParceiroId);
        if (configuracaoDoSistema is null) return new();

        var usuarioViewModel = await _usuarioAutenticado.GetUsuarioAutenticadoAsync();
        var pedidoMinimo = usuarioViewModel.IsAtacado
            ? configuracaoDoSistema.PedidoMinimoAtacado
            : configuracaoDoSistema.PedidoMinimoVarejo;

        return new PedidoMinimoViewModel()
        {
            PedidoMinimo = pedidoMinimo,
            MensagemPedidoFinalizado = configuracaoDoSistema.MensagemPedidoFinalizado
        };
    }

    public async Task<TodasConfiguracoesViewModel> GetTodasConfiguracoesAsync()
    {
        var configuracaoDoSistema =
            await _configuracaoDoSistemaRepository.GetConfiguracaoDoSistemaAsync(_usuarioAutenticado.ParceiroId);
        var resultadoConfiguracaoDeFrete = await _configuracaoDeFreteService.ObterAsync();

        var config = new TodasConfiguracoesViewModel()
        {
            MensagemPedidoFinalizado = configuracaoDoSistema?.MensagemPedidoFinalizado
        };

        if (configuracaoDoSistema != null)
        {
            var usuarioViewModel = await _usuarioAutenticado.GetUsuarioAutenticadoAsync();
            config.PedidoMinimo = usuarioViewModel.IsAtacado
                ? configuracaoDoSistema.PedidoMinimoAtacado
                : configuracaoDoSistema.PedidoMinimoVarejo;
        }

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
