using OpenAdm.Application.Dtos.ConfiguracoesDePedidos;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.ConfiguracoesDePedidos;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Application.Services;

public class ConfiguracoesDePedidoService : IConfiguracoesDePedidoService
{
    private readonly IConfiguracoesDePedidoRepository _configuracoesDePedidoRepository;
    private readonly IUsuarioAutenticado _usuarioAutenticado;

    public ConfiguracoesDePedidoService(
        IConfiguracoesDePedidoRepository configuracoesDePedidoRepository,
        IUsuarioAutenticado usuarioAutenticado)
    {
        _configuracoesDePedidoRepository = configuracoesDePedidoRepository;
        _usuarioAutenticado = usuarioAutenticado;
    }

    public async Task<ConfiguracoesDePedidoViewModel> CreateConfiguracoesDePedidoAsync(
        UpdateConfiguracoesDePedidoDto updateConfiguracoesDePedidoDto)
    {
        var configuracaoDePedido = await _configuracoesDePedidoRepository.GetConfiguracoesDePedidoAsync(_usuarioAutenticado.ParceiroId);

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
                parceiroId: _usuarioAutenticado.ParceiroId);

            await _configuracoesDePedidoRepository.AddAsync(configuracaoDePedido);
        }
        else
        {
            configuracaoDePedido.Update(
                emailDeEnvio: updateConfiguracoesDePedidoDto.EmailDeEnvio,
                ativo: true,
                pedidoMinimoAtacado: updateConfiguracoesDePedidoDto.PedidoMinimoAtacado,
                pedidoMinimoVarejo: updateConfiguracoesDePedidoDto.PedidoMinimoVarejo);

            _configuracoesDePedidoRepository.Update(configuracaoDePedido);
        }

        await _configuracoesDePedidoRepository.SaveChangesAsync();

        return new ConfiguracoesDePedidoViewModel().ToModel(configuracaoDePedido);
    }

    public async Task<ConfiguracoesDePedidoViewModel> GetConfiguracoesDePedidoAsync()
    {
        var configuracaoDePedido = await _configuracoesDePedidoRepository.GetConfiguracoesDePedidoAsync(_usuarioAutenticado.ParceiroId);
        if (configuracaoDePedido == null) return new();
        return new ConfiguracoesDePedidoViewModel().ToModel(configuracaoDePedido);
    }

    public async Task<PedidoMinimoViewModel> GetPedidoMinimoAsync()
    {
        var configuracaoDePedido = await _configuracoesDePedidoRepository.GetConfiguracoesDePedidoAsync(_usuarioAutenticado.ParceiroId);
        if (configuracaoDePedido is null) return new();

        var usuarioViewModel = await _usuarioAutenticado.GetUsuarioAutenticadoAsync();
        var pedidoMinimo = usuarioViewModel.IsAtacado ?
            configuracaoDePedido.PedidoMinimoAtacado :
            configuracaoDePedido.PedidoMinimoVarejo;

        return new PedidoMinimoViewModel()
        {
            PedidoMinimo = pedidoMinimo
        };
    }
}
