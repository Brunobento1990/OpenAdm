namespace OpenAdm.Application.Models.Tokens;

public class ValidaTokenModel
{
    public bool Expirado { get; set; }
    public Guid Id { get; set; }
    public bool EhFuncionario { get; set; }
    public DateTime DataDoLogin { get; set; }
}