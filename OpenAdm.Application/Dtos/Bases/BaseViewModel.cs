namespace OpenAdm.Application.Dtos.Bases;

public abstract class BaseViewModel
{
    public Guid Id { get; set; }
    public DateTime DataDeCriacao { get; set; }
    public DateTime DataDeAtualizacao { get; set; }
    public long Numero { get; set; }
}
