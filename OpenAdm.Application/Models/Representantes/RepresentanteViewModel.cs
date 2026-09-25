using OpenAdm.Domain.Entities;

namespace OpenAdm.Application.Models.Representantes;

public sealed class RepresentanteViewModel : BaseModel
{
    public string Nome { get; set; } = string.Empty;
    public string? Cpf { get; set; }
    public string? Email { get; set; }
    public string? Telefone { get; set; }
    public bool Ativo { get; set; }

    public static RepresentanteViewModel FromEntity(Representante representante) => new()
    {
        Id = representante.Id,
        Numero = representante.Numero,
        DataDeCriacao = representante.DataDeCriacao,
        DataDeAtualizacao = representante.DataDeAtualizacao,
        Nome = representante.Nome,
        Cpf = representante.Cpf,
        Email = representante.Email,
        Telefone = representante.Telefone,
        Ativo = representante.Ativo
    };
}
