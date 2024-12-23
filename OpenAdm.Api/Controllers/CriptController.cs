using Microsoft.AspNetCore.Mvc;
using OpenAdm.Application.Adapters;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Helpers;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Infra.Context;

namespace OpenAdm.Api.Controllers;

[ApiController]
[Route("cript")]
public class CriptController : ControllerBase
{
    private readonly OpenAdmContext _openAdmContext;
    private readonly IParceiroAutenticado _parceiroAutenticado;
    private readonly ParceiroContext _parceiroContext;
    public CriptController(
        OpenAdmContext openAdmContext,
        IParceiroAutenticado parceiroAutenticado,
        ParceiroContext parceiroContext)
    {
        _openAdmContext = openAdmContext;
        _parceiroAutenticado = parceiroAutenticado;
        _parceiroContext = parceiroContext;
    }

    [HttpGet("gerar-empresa-dev")]
    public async Task<IActionResult> GerarEmpresaDev()
    {
        var conexao = "User ID=postgres; Password=1234; Host=192.168.1.199; Port=5432; Database=desenvolvimento; Pooling=true;";
        var parceiro = new Parceiro(
            id: Guid.NewGuid(),
            dataDeCriacao: DateTime.Now,
            dataDeAtualizacao: DateTime.Now,
            numero: 0,
            razaoSocial: "desenvolvimento",
            nomeFantasia: "desenvolvimento",
            cnpj: "123");

        parceiro.ConfiguracaoDbParceiro = new ConfiguracaoParceiro(
            id: Guid.NewGuid(),
            dataDeCriacao: DateTime.Now,
            dataDeAtualizacao: DateTime.Now,
            numero: 0,
            conexaoDb: Criptografia.Encrypt(conexao),
            dominioSiteAdm: "http://localhost:7154/",
            dominioSiteEcommerce: "http://localhost:8081/",
            ativo: true,
            parceiroId: parceiro.Id,
            clienteMercadoPago: null);

        await _openAdmContext.Parceiros.AddAsync(parceiro);
        await _openAdmContext.SaveChangesAsync();

        _parceiroAutenticado.StringConnection = conexao;

        var funcionario = new Funcionario(
            id: Guid.NewGuid(),
            dataDeCriacao: DateTime.Now,
            dataDeAtualizacao: DateTime.Now,
            numero: 0,
            email: "brunobentocaina@gmail.com",
            senha: PasswordAdapter.GenerateHash("1234"),
            nome: "bruno",
            telefone: "123",
            avatar: null,
            ativo: true);

        await _parceiroContext.Funcionarios.AddAsync(funcionario);
        await _parceiroContext.SaveChangesAsync();

        return Ok();
    }

    [HttpPost("Criptar")]
    public IActionResult Criptar(BodyCript bodyCript)
    {
        if (!VariaveisDeAmbiente.IsDevelopment())
        {
            return Unauthorized();
        }
        var conn = Criptografia.Encrypt(bodyCript.Code);
        return Ok(conn);
    }

    [HttpPost("DeCriptar")]
    public IActionResult DeCriptar(BodyCript bodyCript)
    {
        if (!VariaveisDeAmbiente.IsDevelopment())
        {
            return Unauthorized();
        }
        var conn = Criptografia.Decrypt(bodyCript.Code);
        return Ok(conn);
    }
}

public class BodyCript
{
    public string Code { get; set; } = string.Empty;
}
