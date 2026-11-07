using OpenAdm.Domain.Entities.Bases;

namespace OpenAdm.Domain.Entities;

public sealed class ConfiguracaoDoSistema : BaseEntityParceiro
{
    public ConfiguracaoDoSistema(
        Guid id,
        DateTime dataDeCriacao,
        DateTime dataDeAtualizacao,
        long numero,
        string emailParaEnvioNotificacao,
        bool ativo,
        decimal? pedidoMinimoAtacado,
        decimal? pedidoMinimoVarejo,
        Guid parceiroId,
        string? whatsAppParaEnvioNotificacao,
        bool vendaDeProdutoComEstoque,
        bool listarProdutoSomenteComEstoque = false,
        string? mensagemPedidoFinalizado = null,
        bool notificarCadastroNovoCliente = false)
            : base(id, dataDeCriacao, dataDeAtualizacao, numero, parceiroId)
    {
        EmailParaEnvioNotificacao = emailParaEnvioNotificacao;
        Ativo = ativo;
        PedidoMinimoAtacado = pedidoMinimoAtacado;
        PedidoMinimoVarejo = pedidoMinimoVarejo;
        WhatsAppParaEnvioNotificacao = whatsAppParaEnvioNotificacao;
        VendaDeProdutoComEstoque = vendaDeProdutoComEstoque;
        ListarProdutoSomenteComEstoque = listarProdutoSomenteComEstoque;
        MensagemPedidoFinalizado = mensagemPedidoFinalizado;
        NotificarCadastroNovoCliente = notificarCadastroNovoCliente;
    }

    public void Update(string emailDeEnvio, bool ativo, decimal? pedidoMinimoAtacado, decimal? pedidoMinimoVarejo, string? whatsAppParaEnvioNotificacao, bool vendaDeProdutoComEstoque, bool listarProdutoSomenteComEstoque = false, string? mensagemPedidoFinalizado = null, bool notificarCadastroNovoCliente = false)
    {
        PedidoMinimoAtacado = pedidoMinimoAtacado;
        PedidoMinimoVarejo = pedidoMinimoVarejo;
        EmailParaEnvioNotificacao = emailDeEnvio;
        Ativo = ativo;
        WhatsAppParaEnvioNotificacao = whatsAppParaEnvioNotificacao;
        VendaDeProdutoComEstoque = vendaDeProdutoComEstoque;
        ListarProdutoSomenteComEstoque = listarProdutoSomenteComEstoque;
        MensagemPedidoFinalizado = mensagemPedidoFinalizado;
        NotificarCadastroNovoCliente = notificarCadastroNovoCliente;
    }

    public string EmailParaEnvioNotificacao { get; private set; }
    public string? WhatsAppParaEnvioNotificacao { get; private set; }
    public bool Ativo { get; private set; }
    public bool VendaDeProdutoComEstoque { get; private set; }
    public bool ListarProdutoSomenteComEstoque { get; private set; }
    public bool NotificarCadastroNovoCliente { get; private set; }
    public decimal? PedidoMinimoAtacado { get; private set; }
    public decimal? PedidoMinimoVarejo { get; private set; }
    public string? MensagemPedidoFinalizado { get; private set; }
}
