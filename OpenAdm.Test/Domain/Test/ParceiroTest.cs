using ExpectedObjects;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Exceptions;
using OpenAdm.Test.Domain.Builder;

namespace OpenAdm.Test.Domain.Test;

public class ParceiroTest
{
    [Fact]
    public void DeveCriarParceiro()
    {
        var dto = new
        {
            RazaoSocial = "Empresa fake",
            NomeFantasia = "Criou Ok",
            Cnpj = "56198156165"
        };

        var parceiro = new Parceiro(
            Guid.NewGuid(), 
            DateTime.Now, 
            DateTime.Now, 
            1, 
            dto.RazaoSocial, 
            dto.NomeFantasia, 
            dto.Cnpj);

        dto.ToExpectedObject().ShouldMatch(parceiro);

    }


    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void NaoDeveCriarParceiroSemCnpj(string cnpj)
    {
        Assert.Throws<ExceptionApi>(
            () => ParceiroBuilder
                    .Init()
                    .NaoDeveCriarSemCnpj(cnpj)
                    .Build());
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void NaoDeveCriarParceiroSemRazaoSocial(string razaoSocial)
    {
        Assert.Throws<ExceptionApi>(
            () => ParceiroBuilder
                    .Init()
                    .NaoDeveCriarSemRazaoSocial(razaoSocial)
                    .Build());
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void NaoDeveCriarParceiroSemNomeFantasia(string nomeFantasia)
    {
        Assert.Throws<ExceptionApi>(
            () => ParceiroBuilder
                    .Init()
                    .NaoDeveCriarSemNomeFantasia(nomeFantasia)
                    .Build());
    }
}
