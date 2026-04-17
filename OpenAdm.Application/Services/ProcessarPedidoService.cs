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
    private readonly IEstoqueService _estoqueService;
    private readonly IEventoAplicacaoRepository _eventoAplicacaoRepository;

    public ProcessarPedidoService(
        IPedidoRepository pedidoRepository,
        IParceiroAutenticado parceiroAutenticado,
        IEstoqueService estoqueService, IEventoAplicacaoRepository eventoAplicacaoRepository)
    {
        _pedidoRepository = pedidoRepository;
        _parceiroAutenticado = parceiroAutenticado;
        _estoqueService = estoqueService;
        _eventoAplicacaoRepository = eventoAplicacaoRepository;
    }

    public async Task ProcessarCreateAsync(Guid pedidoId, ConfiguracoesDePedidoViewModel configuracoesDePedido)
    {
        var pedido = await _pedidoRepository.GetPedidoCompletoByIdAsync(pedidoId);

        if (pedido == null)
        {
            return;
        }

        //TODO: adicionar evento de reservar estoque    
        await _estoqueService.ReservarEstoqueNovoPedidoAsync(pedido);

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