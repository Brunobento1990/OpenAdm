using Domain.Pkg.Entities;
using Domain.Pkg.Exceptions;
using OpenAdm.Test.Domain.Builder;

namespace OpenAdm.Test.Domain.Test;

public class UsuarioTest
{
    [Fact]
    public void DeveCriarUsuario()
    {
        var usuarioDto = UsuarioBuilder.Init().Build();
        var usuario = new Usuario(
            usuarioDto.Id, 
            usuarioDto.DataDeCriacao, 
            usuarioDto.DataDeAtualizacao, 
            usuarioDto.Numero, 
            usuarioDto.Email, 
            usuarioDto.Senha, 
            usuarioDto.Nome, 
            usuarioDto.Telefone, 
            usuarioDto.Cnpj,
            usuarioDto.Cpf);

        Assert.NotNull(usuario);
        Assert.Equal(usuarioDto.Id, usuario.Id);
        Assert.Equal(usuarioDto.DataDeCriacao, usuario.DataDeCriacao);
        Assert.Equal(usuarioDto.DataDeAtualizacao, usuario.DataDeAtualizacao);
        Assert.Equal(usuarioDto.Numero, usuario.Numero);
        Assert.Equal(usuarioDto.Email, usuario.Email);
        Assert.Equal(usuarioDto.Senha, usuario.Senha);
        Assert.Equal(usuarioDto.Nome, usuario.Nome);
        Assert.Equal(usuarioDto.Telefone, usuario.Telefone);
        Assert.Equal(usuarioDto.Cnpj, usuario.Cnpj);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void NaoDeveCriarUsuarioSemEmail(string email)
    {
        Assert.Throws<ExceptionApi>(() => UsuarioBuilder.Init().SemEmail(email).Build());
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void NaoDeveCriarUsuarioSemSenha(string senha)
    {
        Assert.Throws<ExceptionApi>(() => UsuarioBuilder.Init().SemSenha(senha).Build());
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void NaoDeveCriarUsuarioSemNome(string nome)
    {
        Assert.Throws<ExceptionApi>(() => UsuarioBuilder.Init().SemNome(nome).Build());
    }

    [Fact]
    public void NaoDeveCriarUsuarioComTelefoneInvalido()
    {
        Assert.Throws<ExceptionApi>(() => UsuarioBuilder.Init().TelefoneInvalido().Build());
    }
}
