using OpenAdm.Application.Dtos.Pedidos;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Interfaces.Pedidos;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Enuns;
using OpenAdm.Domain.Exceptions;

using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Application.Services.Pedidos;

public class CreatePedidoAdmService : ICreatePedidoAdmService
{
    private readonly IPedidoRepository _pedidoRepository;
    private readonly IFaturaService _faturaService;
    private readonly IUsuarioService _usuarioService;

    public CreatePedidoAdmService(
        IPedidoRepository pedidoRepository,
        IFaturaService faturaService,
        IUsuarioService usuarioService)
    {
        _pedidoRepository = pedidoRepository;
        _faturaService = faturaService;
        _usuarioService = usuarioService;
    }

    public async Task<bool> CreateAsync(PedidoAdmCreateDto pedidoAdmCreateDto)
    {
        if (pedidoAdmCreateDto.ItensPedido.Count == 0)
        {
            throw new ExceptionApi("Informe os itens do pedido!");
        }
        var usuario = await _usuarioService.GetUsuarioByIdValidacaoAsync(id: pedidoAdmCreateDto.UsuarioId);
        var date = DateTime.Now;
        var pedido = new Pedido(Guid.NewGuid(), date, date, 0, StatusPedido.Aberto, usuario.Id, null);

        pedido.ProcessarItensPedido(pedidoAdmCreateDto.ItensPedido);

        if (pedidoAdmCreateDto.EnderecoEntrega != null)
        {
            pedido.EnderecoEntrega = new EnderecoEntregaPedido(
                cep: pedidoAdmCreateDto.EnderecoEntrega.Cep,
                logradouro: pedidoAdmCreateDto.EnderecoEntrega.Logradouro,
                bairro: pedidoAdmCreateDto.EnderecoEntrega.Bairro,
                localidade: pedidoAdmCreateDto.EnderecoEntrega.Localidade,
                complemento: pedidoAdmCreateDto.EnderecoEntrega.Complemento ?? "",
                numero: pedidoAdmCreateDto.EnderecoEntrega.Numero,
                uf: pedidoAdmCreateDto.EnderecoEntrega.Uf,
                pedidoId: pedido.Id,
                valorFrete: pedidoAdmCreateDto.EnderecoEntrega.ValorFrete ?? 0,
                tipoFrete: pedidoAdmCreateDto.EnderecoEntrega.TipoFrete ?? "",
                id: Guid.NewGuid());
        }

        await _pedidoRepository.AddAsync(pedido);

        await _faturaService.CriarContasAReceberAsync(new()
        {
            DataDoPrimeiroVencimento = DateTime.Now.AddMonths(1),
            Desconto = null,
            MeioDePagamento = null,
            Observacao = $"Pedido: {pedido.Numero}",
            PedidoId = pedido.Id,
            QuantidadeDeParcelas = 1,
            Total = pedido.ValorTotal,
            UsuarioId = pedido.UsuarioId,
            Tipo = TipoFaturaEnum.A_Receber
        });

        return true;
    }
}
