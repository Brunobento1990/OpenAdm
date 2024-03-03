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

    public ConfiguracoesDePedidoService(IConfiguracoesDePedidoRepository configuracoesDePedidoRepository)
    {
        _configuracoesDePedidoRepository = configuracoesDePedidoRepository;
    }

    public async Task<ConfiguracoesDePedidoViewModel> CreateConfiguracoesDePedidoAsync(
        UpdateConfiguracoesDePedidoDto updateConfiguracoesDePedidoDto)
    {
        var configuracaoDePedido = await GetAsync();

        var logo = updateConfiguracoesDePedidoDto.Logo == null ?
            null :
            Encoding.UTF8.GetBytes(updateConfiguracoesDePedidoDto.Logo);

        if(configuracaoDePedido == null)
        {
            var date = DateTime.Now;
            configuracaoDePedido = new ConfiguracoesDePedido(
                Guid.NewGuid(),
                date,
                date,
                0,
                updateConfiguracoesDePedidoDto.EmailDeEnvio,
                logo,
                true);

            await _configuracoesDePedidoRepository.AddAsync(configuracaoDePedido);
        }
        else
        {
            configuracaoDePedido.Update(
                updateConfiguracoesDePedidoDto.EmailDeEnvio,
                true,
                logo);

            await _configuracoesDePedidoRepository.UpdateAsync(configuracaoDePedido);
        }

        return new ConfiguracoesDePedidoViewModel().ToModel(configuracaoDePedido);
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
