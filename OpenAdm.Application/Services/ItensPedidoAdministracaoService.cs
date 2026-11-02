using OpenAdm.Application.Dtos.ItensPedidos;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models;
using OpenAdm.Application.Models.Pedidos;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Enuns;
using OpenAdm.Domain.Extensions;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model;

namespace OpenAdm.Application.Services;

public sealed class ItensPedidoAdministracaoService(
    IItensPedidoRepository itensPedidoRepository,
    IPedidoRepository pedidoRepository,
    IProdutoRepository produtoRepository)
    : IItensPedidoAdministracaoService
{
    public async Task<ResultPartner<ItensPedidoViewModel>> AdicionarAsync(AddItemPedidoDto dto)
    {
        var pedido = await pedidoRepository.GetPedidoByIdAsync(dto.PedidoId);
        if (pedido == null)
            return (ResultPartner<ItensPedidoViewModel>)"Não foi possível localizar o pedido!";

        var erro = ValidarPedidoAberto(pedido) ?? ValidarValoresDoItem(dto.Quantidade, dto.ValorUnitario);
        if (erro != null)
            return (ResultPartner<ItensPedidoViewModel>)erro;

        var descricaoProduto = await produtoRepository.GetDescricaoProdutoAsync(dto.ProdutoId);
        if (descricaoProduto == null)
            return (ResultPartner<ItensPedidoViewModel>)"Não foi possível localizar o produto!";

        var novoTotal = pedido.ValorTotalCobrar + dto.ValorUnitario * dto.Quantidade;
        erro = ValidarFinanceiro(pedido, novoTotal);
        if (erro != null)
            return (ResultPartner<ItensPedidoViewModel>)erro;

        var item = new ItemPedido(Guid.NewGuid(), DateTime.UtcNow, DateTime.UtcNow, 0,
            dto.PesoId, dto.TamanhoId, dto.ProdutoId, pedido.Id,
            dto.ValorUnitario, dto.Quantidade, ativo: true);

        await itensPedidoRepository.AdicionarAsync(item);
        await AtualizarFinanceiroEHistoricoAsync(pedido,
            $"Produto \"{descricaoProduto}\" adicionado ao pedido com quantidade {dto.Quantidade.ConverterPadraoBrasileiro()} e valor unitário de {dto.ValorUnitario.FormatMoney(true)}.");

        return (ResultPartner<ItensPedidoViewModel>)new ItensPedidoViewModel().ToModel(item);
    }

    public async Task<ResultPartner<ItensPedidoViewModel>> EditarQuantidadeAsync(UpdateQuantidadeItemPedidoDto dto)
    {
        var item = await itensPedidoRepository.ObterParaAdministracaoAsync(dto.Id);
        if (item == null)
            return (ResultPartner<ItensPedidoViewModel>)"Não foi possível localizar o item do pedido!";

        var pedido = item.Pedido;

        var erro = ValidarPedidoAberto(pedido) ?? ValidarValoresDoItem(dto.Quantidade, item.ValorUnitario);
        if (erro != null)
            return (ResultPartner<ItensPedidoViewModel>)erro;

        var diferenca = item.ValorUnitario * (dto.Quantidade - item.Quantidade);
        var novoTotal = pedido.ValorTotalCobrar + diferenca;
        erro = ValidarFinanceiro(pedido, novoTotal);
        if (erro != null)
            return (ResultPartner<ItensPedidoViewModel>)erro;

        var quantidadeAnterior = item.Quantidade;
        var valorTotalAnterior = item.ValorTotal;
        item.EditarQuantidade(dto.Quantidade);
        itensPedidoRepository.Update(item);
        await AtualizarFinanceiroEHistoricoAsync(pedido,
            $"Quantidade do produto \"{ObterDescricaoProduto(item)}\" alterada de {quantidadeAnterior.ConverterPadraoBrasileiro()} para {dto.Quantidade.ConverterPadraoBrasileiro()}. Valor total do item alterado de {valorTotalAnterior.FormatMoney(true)} para {item.ValorTotal.FormatMoney(true)}.");

        return (ResultPartner<ItensPedidoViewModel>)new ItensPedidoViewModel().ToModel(item);
    }

    public async Task<ResultPartner<ResultadoPadraoViewModel>> InativarAsync(Guid id)
    {
        var item = await itensPedidoRepository.ObterParaAdministracaoAsync(id);
        if (item == null)
            return (ResultPartner<ResultadoPadraoViewModel>)"Não foi possível localizar o item do pedido!";

        var pedido = item.Pedido;

        var erro = ValidarPedidoAberto(pedido);
        if (erro != null)
            return (ResultPartner<ResultadoPadraoViewModel>)erro;
        if (pedido.ItensPedido.Count(x => x.Ativo) == 1)
            return (ResultPartner<ResultadoPadraoViewModel>)"Não é possível inativar o último item do pedido!";

        var novoTotal = pedido.ValorTotalCobrar - item.ValorTotal;
        erro = ValidarFinanceiro(pedido, novoTotal);
        if (erro != null)
            return (ResultPartner<ResultadoPadraoViewModel>)erro;

        item.Inativar();
        itensPedidoRepository.Update(item);
        await AtualizarFinanceiroEHistoricoAsync(pedido,
            $"Produto \"{ObterDescricaoProduto(item)}\" removido do pedido. Quantidade: {item.Quantidade.ConverterPadraoBrasileiro()}; valor total: {(item.ValorUnitario * item.Quantidade).FormatMoney(true)}.");

        return (ResultPartner<ResultadoPadraoViewModel>)new ResultadoPadraoViewModel { Resultado = true };
    }

    private static string? ValidarPedidoAberto(Pedido pedido)
        => pedido.StatusPedido == StatusPedido.Aberto
            ? null
            : "Este pedido não está aberto e não pode ser alterado!";

    private static string? ValidarValoresDoItem(decimal quantidade, decimal valorUnitario)
    {
        if (quantidade <= 0)
            return "A quantidade deve ser maior que zero!";
        if (valorUnitario < 0)
            return "O valor unitário não pode ser negativo!";
        return null;
    }

    private static string? ValidarFinanceiro(Pedido pedido, decimal novoTotal)
    {
        if (pedido.Fatura == null)
            return null;
        if (novoTotal < pedido.Fatura.ValorPagoRecebido)
            return "Não é possível reduzir o pedido para um valor abaixo do recebido!";
        if (novoTotal > pedido.Fatura.ValorPagoRecebido &&
            pedido.Fatura.Parcelas.All(x => !x.Ativo))
            return "Não existem parcelas ativas para ratear o valor a receber!";
        return null;
    }

    private static string ObterDescricaoProduto(ItemPedido item)
        => string.IsNullOrWhiteSpace(item.Produto?.Descricao) ? "não identificado" : item.Produto.Descricao;

    private async Task AtualizarFinanceiroEHistoricoAsync(Pedido pedido, string descricao)
    {
        var novoTotal = pedido.ValorTotalCobrar;
        if (pedido.Fatura != null)
            pedido.Fatura.AjustarValorAReceber(novoTotal);
        else if (pedido.Cobranca != null)
            pedido.Cobranca.AlterarTotal(novoTotal);

        await itensPedidoRepository.AddHistoricoAsync(PedidoHistorico.Novo(pedido.Id, descricao));
        await pedidoRepository.SaveChangesAsync();
    }
}
