using Domain.Pkg.Entities;
using Domain.Pkg.Errors;
using Domain.Pkg.Exceptions;
using OpenAdm.Application.Dtos.ConfiguracoesDePedidos;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.ConfiguracoesDePedidos;
using OpenAdm.Domain.Interfaces;
using System.Text;

namespace OpenAdm.Application.Services;

public class ConfiguracoesDePedidoService : IConfiguracoesDePedidoService
{
    private readonly IConfiguracoesDePedidoRepository _configuracoesDePedidoRepository;
    private readonly ITokenService _tokenService;

    public ConfiguracoesDePedidoService(
        IConfiguracoesDePedidoRepository configuracoesDePedidoRepository,
        ITokenService tokenService)
    {
        _configuracoesDePedidoRepository = configuracoesDePedidoRepository;
        _tokenService = tokenService;
    }

    public async Task<ConfiguracoesDePedidoViewModel> CreateConfiguracoesDePedidoAsync(
        UpdateConfiguracoesDePedidoDto updateConfiguracoesDePedidoDto)
    {
        var configuracaoDePedido = await GetAsync();

        var logo = updateConfiguracoesDePedidoDto.Logo == null ?
            null :
            Encoding.UTF8.GetBytes(updateConfiguracoesDePedidoDto.Logo);

        if (configuracaoDePedido == null)
        {
            var date = DateTime.Now;
            configuracaoDePedido = new ConfiguracoesDePedido(
                id: Guid.NewGuid(),
                dataDeCriacao: date,
                dataDeAtualizacao: date,
                numero: 0,
                emailDeEnvio: updateConfiguracoesDePedidoDto.EmailDeEnvio,
                logo: logo,
                ativo: true,
                pedidoMinimoAtacado: updateConfiguracoesDePedidoDto.PedidoMinimoAtacado,
                pedidoMinimoVarejo: updateConfiguracoesDePedidoDto.PedidoMinimoVarejo);

            await _configuracoesDePedidoRepository.AddAsync(configuracaoDePedido);
        }
        else
        {
            configuracaoDePedido.Update(
                emailDeEnvio: updateConfiguracoesDePedidoDto.EmailDeEnvio,
                ativo: true,
                logo: logo,
                pedidoMinimoAtacado: updateConfiguracoesDePedidoDto.PedidoMinimoAtacado,
                pedidoMinimoVarejo: updateConfiguracoesDePedidoDto.PedidoMinimoVarejo);

            await _configuracoesDePedidoRepository.UpdateAsync(configuracaoDePedido);
        }

        return new ConfiguracoesDePedidoViewModel().ToModel(configuracaoDePedido);
    }

    public async Task<ConfiguracoesDePedidoViewModel> GetConfiguracoesDePedidoAsync()
    {
        var configuracaoDePedido = await GetAsync();

        return new ConfiguracoesDePedidoViewModel().ToModel(configuracaoDePedido);
    }

    public async Task<PedidoMinimoViewModel> GetPedidoMinimoAsync()
    {
        var configuracaoDePedido = await _configuracoesDePedidoRepository.GetConfiguracoesDePedidoAsync();
        if (configuracaoDePedido is null) return new();

        var usuarioViewModel = _tokenService.GetTokenUsuarioViewModel();
        var pedidoMinimo = string.IsNullOrWhiteSpace(usuarioViewModel.Cnpj) ? 
            configuracaoDePedido.PedidoMinimoVarejo : 
            configuracaoDePedido.PedidoMinimoAtacado;

        return new PedidoMinimoViewModel()
        {
            PedidoMinimo = pedidoMinimo
        };
    }

    private async Task<ConfiguracoesDePedido> GetAsync()
    {
        return await _configuracoesDePedidoRepository.GetConfiguracoesDePedidoAsync()
            ?? throw new ExceptionApi(CodigoErrors.RegistroNotFound);
    }
}
