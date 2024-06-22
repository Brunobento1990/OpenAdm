namespace OpenAdm.Domain.Interfaces;

public interface IUsuarioAutenticado
{
    Guid Id { get; set; }
    DateTime DataDeAtualizacao { get; set; }
    bool IsFuncionario { get; set; }
}
