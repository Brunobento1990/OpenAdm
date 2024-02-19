using Domain.Pkg.Exceptions;
using OpenAdm.Test.Domain.Builder;

namespace OpenAdm.Test.Domain.Test;

public class FuncionarioTest
{
    const string _maisDe255Caracteres = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Nullam feugiat, turpis at pulvinar vulputate, erat libero tristique tellus, nec bibendum odio risus sit amet ante. Aliquam erat volutpat. Nunc auctor. Mauris pretium quam et urna. Fusce nibh. Duis risus. Curabitur";

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(_maisDe255Caracteres)]
    public void NaoDeveCriarFuncionarioSemEmail(string email)
    {
        Assert.Throws<ExceptionApi>(
            () => FuncionarioBuilder
                    .Init()
                    .SemEmail(email)
                    .Build());
    }

    [Fact]
    public void NaoDeveCriarFuncionarioComTelefoneInvalido()
    {
        Assert.Throws<ExceptionApi>(
            () => FuncionarioBuilder
                    .Init()
                    .TelefoneInvalido()
                    .Build());
    }

    [Fact]
    public void NaoDeveCriarFuncionarioComTelefoneComLetras()
    {
        Assert.Throws<ExceptionApi>(
            () => FuncionarioBuilder
                    .Init()
                    .TelefoneInvalidoComLetras()
                    .Build());
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(_maisDe255Caracteres)]
    public void NaoDeveCriarFuncionarioSemNome(string nome)
    {
        Assert.Throws<ExceptionApi>(
            () => FuncionarioBuilder
                    .Init()
                    .SemNome(nome)
                    .Build());
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void NaoDeveCriarFuncionarioSemSenha(string senha)
    {
        Assert.Throws<ExceptionApi>(
            () => FuncionarioBuilder
                    .Init()
                    .SemSenha(senha)
                    .Build());
    }
}
