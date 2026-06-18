using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.ConfiguracoesDePedidos;
using OpenAdm.Domain.Entities.OpenAdm;
using OpenAdm.Domain.Enuns;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model.Eventos;

namespace OpenAdm.Application.Services;

public class ProcessarPedidoService : IProcessarPedidoService
{
    private readonly IPedidoRepository _pedidoRepository;
    private readonly IParceiroAutenticado _parceiroAutenticado;
    private readonly IEventoAplicacaoRepository _eventoAplicacaoRepository;

    public ProcessarPedidoService(
        IPedidoRepository pedidoRepository,
        IParceiroAutenticado parceiroAutenticado,
        IEventoAplicacaoRepository eventoAplicacaoRepository)
    {
        _pedidoRepository = pedidoRepository;
        _parceiroAutenticado = parceiroAutenticado;
        _eventoAplicacaoRepository = eventoAplicacaoRepository;
    }

    public async Task ProcessarCreateAsync(Guid pedidoId, ConfiguracoesDePedidoViewModel configuracoesDePedido)
    {
        var pedido = await _pedidoRepository.GetPedidoCompletoByIdAsync(pedidoId);

        if (pedido == null)
        {
            return;
        }

        var dados = new NovoPedidoEvento()
        {
            PedidoId = pedidoId,
        }.ToString();

        var evento = EventoAplicacao
            .Criar(
                dados: dados,
                tipoEventoAplicacao: TipoEventoAplicacaoEnum.EnviarPedidoWhatsApp,
                empresaOpenAdmId: _parceiroAutenticado.Id);

        await _eventoAplicacaoRepository.AddAsync(evento);
        await _eventoAplicacaoRepository.SaveChangesAsync();
    }
}