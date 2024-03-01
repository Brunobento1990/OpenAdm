using Domain.Pkg.Entities;
using Domain.Pkg.Errors;
using Domain.Pkg.Exceptions;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.ConfiguracoesDePedidos;
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Application.Services;

public class ConfiguracoesDePedidoService : IConfiguracoesDePedidoService
{
    private readonly IConfiguracoesDePedidoRepository _configuracoesDePedidoRepository;

    public ConfiguracoesDePedidoService(IConfiguracoesDePedidoRepository configuracoesDePedidoRepository)
    {
        _configuracoesDePedidoRepository = configuracoesDePedidoRepository;
    }

    public async Task<ConfiguracoesDePedidoViewModel> GetConfiguracoesDePedidoAsync()
    {
        var configuracaoDePedido = await GetAsync();

        return new ConfiguracoesDePedidoViewModel().ToModel(configuracaoDePedido);
    }

    private async Task<ConfiguracoesDePedido> GetAsync()
    {
        return await _configuracoesDePedidoRepository.GetConfiguracoesDePedidoAsync()
            ?? throw new ExceptionApi(CodigoErrors.RegistroNotFound);
    }
}
