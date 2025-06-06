﻿using OpenAdm.Application.Models.Estoques;
using OpenAdm.Application.Models.MovimentacaoDeProdutos;
using OpenAdm.Application.Models.ParcelasModel;
using OpenAdm.Application.Models.TopUsuarios;

namespace OpenAdm.Application.Models.Home;

public class HomeAdmViewModel
{
    public IList<EstoqueViewModel> PosicaoDeEstoques { get; set; } = [];
    public IList<TopUsuariosViewModel> TopUsuariosTotalCompra { get; set; } = [];
    public IList<TopUsuariosViewModel> TopUsuariosTotalPedido { get; set; } = [];
    public IList<MovimentoDeProdutoDashBoardModel> Movimentos { get; set; } = [];
    public IList<ParcelaPagaDashBoardModel> Faturas { get; set; } = [];
    public decimal TotalAReceber { get; set; }
    public int PedidosEmAberto { get; set; }
    public long QuantidadeDeAcessoEcommerce { get; set; }
    public long QuantidadeDeUsuarioCnpj { get; set; }
    public long QuantidadeDeUsuarioCpf { get; set; }
}
