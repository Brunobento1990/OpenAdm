using OpenAdm.Application.Adapters;
using OpenAdm.Application.Attributes;
using OpenAdm.Domain.Constants;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Extensions;

namespace OpenAdm.Application.Dtos.Representantes;

public sealed class CriarRepresentanteDto : ValidarBaseDTO
{
    [ValidaString(erro: "Informe o nome", maxLength: RepresentanteConstantes.NomeMaxLength)]
    public string Nome { get; set; } = string.Empty;

    [ValidaStringLength(maxLength: RepresentanteConstantes.CpfDtoMaxLength)]
    [ValidaCpf]
    public string? Cpf { get; set; }

    [ValidaStringLength(maxLength: RepresentanteConstantes.EmailMaxLength)]
    [ValidaEmail]
    public string? Email { get; set; }

    [ValidaStringLength(maxLength: RepresentanteConstantes.TelefoneMaxLength)]
    public string? Telefone { get; set; }

    [ValidaStringLength(maxLength: RepresentanteConstantes.SenhaDtoMaxLength)]
    public string? Senha { get; set; }

    [ValidaStringLength(maxLength: RepresentanteConstantes.SenhaDtoMaxLength)]
    public string? ConfirmarSenha { get; set; }

    public override string? Validar()
    {
        var erro = base.Validar();
        if (erro != null)
            return erro;

        return Senha.NullSeVazio() != ConfirmarSenha.NullSeVazio()
            ? "As senhas não conferem"
            : null;
    }

    public Representante ToEntity()
    {
        var data = DateTime.UtcNow;
        return new Representante(Guid.NewGuid(), data, data, 0, Nome.Trim(), Cpf.LimparMascaraCpf(),
            Email.NullSeVazio()?.ToLowerInvariant(), Telefone.NullSeVazio(),
            string.IsNullOrWhiteSpace(Senha) ? null : PasswordAdapter.GenerateHash(Senha), true);
    }
}
