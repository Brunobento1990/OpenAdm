using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.ConfiguracoesDePedidos;
using OpenAdm.Domain.Entities.OpenAdm;
using OpenAdm.Domain.Enuns;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model.Eventos;

namespace OpenAdm.Application.Services;

public class ProcessarPedidoService : IProcessarPedidoService
{
    private readonly IParceiroAutenticado _parceiroAutenticado;
    private readonly IFilaService _filaService;


    public ProcessarPedidoService(
        IParceiroAutenticado parceiroAutenticado,
        IFilaService filaService)
    {
        _parceiroAutenticado = parceiroAutenticado;
        _filaService = filaService;
    }

    public async Task ProcessarCreateAsync(Guid pedidoId, ConfiguracoesDePedidoViewModel configuracoesDePedido)
    {
        var dados = new NovoPedidoEvento()
        {
            PedidoId = pedidoId,
        }.ToString();

        var evento = EventoAplicacao
            .Criar(
                dados: dados,
                tipoEventoAplicacao: TipoEventoAplicacaoEnum.EnviarPedidoWhatsApp,
                empresaOpenAdmId: _parceiroAutenticado.Id);

        await _filaService.PublicarAsync(evento);
    }
}