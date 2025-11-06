using OpenAdm.Domain.Entities;
using OpenAdm.Application.Adapters;
using OpenAdm.Application.Models;
using OpenAdm.Domain.Exceptions;
using OpenAdm.Application.Dtos.Parceiros;

namespace OpenAdm.Application.Dtos.Usuarios;

public class CreateUsuarioAdminDto : BaseModel
{
    public string Nome { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Senha { get; set; }
    public string? ReSenha { get; set; }
    public string Telefone { get; set; } = string.Empty;
    public string? Cnpj { get; set; } = string.Empty;
    public string? Cpf { get; set; } = string.Empty;
    public EnderecoDto? EnderecoUsuario { get; set; }

    public void Validar()
    {
        //if (string.IsNullOrWhiteSpace(Cpf) && string.IsNullOrWhiteSpace(Cnpj))
        //{
        //    throw new ExceptionApi("Informe o CNPJ ou o CPF");
        //}

        //if (!string.IsNullOrWhiteSpace(Cpf) && !string.IsNullOrWhiteSpace(Cnpj))
        //{
        //    throw new ExceptionApi("Informe o CNPJ ou o CPF");
        //}

        if (string.IsNullOrWhiteSpace(Nome))
        {
            throw new ExceptionApi("Informe o nome");
        }

        if (string.IsNullOrWhiteSpace(Telefone))
        {
            throw new ExceptionApi("Informe o telefone");
        }

        //if (string.IsNullOrWhiteSpace(Senha) || string.IsNullOrWhiteSpace(ReSenha))
        //{
        //    throw new ExceptionApi("Senhas inválidas");
        //}

        if (!string.IsNullOrWhiteSpace(Senha) && !string.IsNullOrWhiteSpace(ReSenha) && !Senha.Equals(ReSenha))
        {
            throw new ExceptionApi("As senha não conferem!");
        }

        Cpf = string.IsNullOrWhiteSpace(Cpf) ? null : Cpf?.Replace(".", "")?.Replace("-", "");
        Cnpj = string.IsNullOrWhiteSpace(Cnpj) ? null : Cnpj?.Replace(".", "")?.Replace("-", "")?.Replace("/", "");
    }

    public Usuario ToEntity()
    {
        var date = DateTime.Now;

        var usuario = new Usuario(
            Guid.NewGuid(),
            date,
            date,
            0,
            Email ?? "",
            senha: string.IsNullOrWhiteSpace(Senha) ? "" : PasswordAdapter.GenerateHash(Senha),
            Nome,
            Telefone,
            Cnpj,
            Cpf,
            true,
            null,
            null);

        usuario.EnderecoUsuario = EnderecoUsuario == null ? null : new EnderecoUsuario(
                cep: EnderecoUsuario.Cep,
                logradouro: EnderecoUsuario.Logradouro,
                bairro: EnderecoUsuario.Bairro,
                localidade: EnderecoUsuario.Localidade,
                complemento: EnderecoUsuario.Complemento ?? "",
                numero: EnderecoUsuario.Numero,
                uf: EnderecoUsuario.Uf,
                id: Guid.NewGuid(),
                usuarioId: usuario.Id);

        return usuario;
    }
}
