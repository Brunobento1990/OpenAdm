using System.Text.Json;
using OpenAdm.Domain.Enuns;
using OpenAdm.Domain.Extensions;

namespace OpenAdm.Domain.Entities.OpenAdm;

public class EventoAplicacao
{
    public EventoAplicacao(
        Guid id,
        DateTime dataDeCriacao,
        DateTime dataDeAtualizacao,
        JsonDocument dados,
        TipoEventoAplicacaoEnum tipoEventoAplicacao,
        bool finalizado,
        string? mensagem,
        short quantidadeDeTentativa,
        Guid empresaOpenAdmId)
    {
        Id = id;
        DataDeAtualizacao = dataDeAtualizacao;
        Dados = dados;
        TipoEventoAplicacao = tipoEventoAplicacao;
        Finalizado = finalizado;
        Mensagem = mensagem;
        QuantidadeDeTentativa = quantidadeDeTentativa;
        EmpresaOpenAdmId = empresaOpenAdmId;
        DataDeCriacao = dataDeCriacao;
    }

    public Guid Id { get; private set; }
    public DateTime DataDeCriacao { get; private set; }
    public DateTime DataDeAtualizacao { get; private set; }
    public JsonDocument Dados { get; private set; }
    public TipoEventoAplicacaoEnum TipoEventoAplicacao { get; private set; }
    public bool Finalizado { get; private set; }
    public string? Mensagem { get; private set; }
    public short QuantidadeDeTentativa { get; private set; }
    public Guid EmpresaOpenAdmId { get; private set; }
    public EmpresaOpenAdm EmpresaOpenAdm { get; set; } = null!;

    public bool PodeExecutar => !Finalizado && QuantidadeDeTentativa < 3;

    public T? DadosParse<T>()
    {
        return JsonSerializer.Deserialize<T>(Dados.RootElement.GetRawText());
    }

    public void AdicionarMensagem(string mensagem)
    {
        Mensagem = mensagem.Limitar(1000);
        DataDeAtualizacao = DateTime.Now;
    }

    public void AdicionarTentativa()
    {
        QuantidadeDeTentativa++;
        DataDeAtualizacao = DateTime.Now;
    }

    public void Finalizada(string? mensagem)
    {
        Mensagem = string.IsNullOrWhiteSpace(mensagem) ? mensagem : mensagem.Limitar(1000);
        Finalizado = true;
        DataDeAtualizacao = DateTime.Now;
    }

    public static EventoAplicacao Criar(
        string dados,
        TipoEventoAplicacaoEnum tipoEventoAplicacao,
        Guid empresaOpenAdmId)
    {
        return new EventoAplicacao(
            id: Guid.NewGuid(),
            dataDeCriacao: DateTime.Now,
            dataDeAtualizacao: DateTime.Now,
            dados: JsonDocument.Parse(dados),
            tipoEventoAplicacao, false, null, 0, empresaOpenAdmId);
    }
}