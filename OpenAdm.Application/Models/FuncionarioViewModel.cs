using OpenAdm.Domain.Entities;
using System.Text;

namespace OpenAdm.Application.Models;

public class FuncionarioViewModel : BaseModel
{
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Telefone { get; set; }
    public string? Avatar { get; set; }

    public FuncionarioViewModel ToModel(Funcionario entity)
    {
        Id = entity.Id;
        DataDeCriacao = entity.DataDeCriacao;
        DataDeAtualizacao = entity.DataDeAtualizacao;
        Numero = entity.Numero;
        Nome = entity.Nome;
        Email = entity.Email;
        Telefone = entity.Telefone;
        Avatar = entity.Avatar == null ? null : Encoding.UTF8.GetString(entity.Avatar);

        return this;
    }
}
