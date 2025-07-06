using OpenAdm.Application.Dtos.Usuarios;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.Usuarios;
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Application.Services;

internal class UsuarioPedidoServico : IUsuarioPedidoServico
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IPedidoRepository _pedidoRepository;

    public UsuarioPedidoServico(
        IUsuarioRepository usuarioRepository,
        IPedidoRepository pedidoRepository)
    {
        _usuarioRepository = usuarioRepository;
        _pedidoRepository = pedidoRepository;
    }

    public async Task<PaginacaoUltimoPedidoUsuarioViewModel> ListarAsync(PaginacaoUltimoPedidoUsuarioDto paginacaoUltimoPedidoUsuarioDto)
    {
        var paginacao = await _usuarioRepository.ListarUltimoPedidoAsync(
            page: paginacaoUltimoPedidoUsuarioDto.Page,
            isJuridico: paginacaoUltimoPedidoUsuarioDto.IsJuridico,
            search: paginacaoUltimoPedidoUsuarioDto.Search);

        var usuariosPedidos = new List<UltimoPedidoUsuarioViewModel>();

        foreach (var usuario in paginacao.Values)
        {
            var ultimoPedido = new UltimoPedidoUsuarioViewModel()
            {
                UsuarioId = usuario.Id,
                CpfCnpj = paginacaoUltimoPedidoUsuarioDto.IsJuridico ? usuario.Cnpj ?? "" : usuario.Cpf ?? "",
                Nome = usuario.Nome,
                Telefone = usuario.Telefone ?? ""
            };

            usuariosPedidos.Add(ultimoPedido);
            var pedido = await _pedidoRepository.GetPedidoByUsuarioIdAsync(usuario.Id);
            if (pedido == null)
            {
                continue;
            }

            ultimoPedido.Total = pedido.ValorTotal;
            ultimoPedido.PedidoId = pedido.Id;
            ultimoPedido.DataDoUltimoPedido = pedido.DataDeCriacao;
            ultimoPedido.NumeroDoPedido = pedido.Numero;
            ultimoPedido.StatusPedido = pedido.StatusPedido;
        }

        return new()
        {
            Dados = usuariosPedidos,
            TotalPagina = paginacao.TotalPaginas,
        };
    }
}
