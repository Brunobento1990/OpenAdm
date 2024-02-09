namespace OpenAdm.Application.Models;

public class BaseModel
{
    public Guid Id { get; set; }
    public DateTime DataDeCriacao { get; set; }
    public DateTime DataDeAtualizacao { get; set; }
    public long Numero { get; set; }
}
