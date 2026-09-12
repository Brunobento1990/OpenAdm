using OpenAdm.Application.Dtos.Funcionarios;
using OpenAdm.Application.Dtos.Usuarios;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models;
using OpenAdm.Application.Models.Emails;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model;
using Microsoft.Extensions.Configuration;

namespace OpenAdm.Application.Services;

public class FuncionarioEsqueceuSenhaService : IFuncionarioEsqueceuSenhaService
{
    private readonly ILoginFuncionarioRepository _loginFuncionarioRepository;
    private readonly IFuncionarioEsqueceuSenhaRepository _funcionarioEsqueceuSenhaRepository;
    private readonly IEmailApiService _emailService;
    private readonly IParceiroAutenticado _parceiroAutenticado;
    private readonly IConfiguration _configuration;

    public FuncionarioEsqueceuSenhaService(
        ILoginFuncionarioRepository loginFuncionarioRepository,
        IFuncionarioEsqueceuSenhaRepository funcionarioEsqueceuSenhaRepository,
        IEmailApiService emailService,
        IParceiroAutenticado parceiroAutenticado,
        IConfiguration configuration)
    {
        _loginFuncionarioRepository = loginFuncionarioRepository;
        _funcionarioEsqueceuSenhaRepository = funcionarioEsqueceuSenhaRepository;
        _emailService = emailService;
        _parceiroAutenticado = parceiroAutenticado;
        _configuration = configuration;
    }

    public async Task<ResultPartner<ResultadoPadraoViewModel>> SolicitarAsync(EsqueceuSenhaDto esqueceuSenhaDto)
    {
        var parceiro = await _parceiroAutenticado.ObterParceiroAutenticadoAsync();
        var funcionario =
            await _loginFuncionarioRepository.GetFuncionarioByEmailAsync(esqueceuSenhaDto.Email.Trim(),
                _parceiroAutenticado.Id);

        if (funcionario == null)
            return (ResultPartner<ResultadoPadraoViewModel>)"Não foi possível localizar o funcionário!";

        var dominio = parceiro.EmpresaOpenAdm.Link?.Url;

        if (string.IsNullOrWhiteSpace(dominio))
            return (ResultPartner<ResultadoPadraoViewModel>)"Não foi possível localizar o domínio da empresa";

        var agora = DateTime.UtcNow;
        var token = Guid.NewGuid();
        var horasParaExpirar = int.TryParse(
                                   _configuration["FuncionarioEsqueceuSenha:ExpiracaoHoras"], out var horasConfiguradas)
                               && horasConfiguradas > 0
            ? horasConfiguradas
            : 1;
        var proximoNumero = await _funcionarioEsqueceuSenhaRepository.ProximoNumeroAsync(_parceiroAutenticado.Id);
        var solicitacao = new FuncionarioEsqueceuSenha(
            Guid.NewGuid(), agora, agora, proximoNumero, funcionario.Id, token,
            agora.AddHours(horasParaExpirar), false, _parceiroAutenticado.Id);

        var htmlEnvio = await File.ReadAllTextAsync(Path.Combine("Htmls", "EsqueceuSenha.html"));
        htmlEnvio = htmlEnvio.Replace(
            "Recebemos uma solicitação para redefinir sua senha para o e-commerce",
            "Recebemos uma solicitação para redefinir sua senha para o admin e-commerce");
        htmlEnvio = htmlEnvio.Replace("***empresa***", parceiro.NomeFantasia);
        htmlEnvio = htmlEnvio.Replace("***ecommerce***", parceiro.NomeFantasia);
        htmlEnvio = htmlEnvio.Replace("***usuario***", funcionario.Nome);
        htmlEnvio = htmlEnvio.Replace("***link***", $"https://adm.{dominio}/recuperar-senha/{token}");

        var enviado = await _emailService.SendEmailAsync(
            new ToEnvioEmailApiModel
            {
                Assunto = "Recuperar senha",
                Email = funcionario.Email,
                Mensagem = "",
                Html = htmlEnvio
            },
            new FromEnvioEmailApiModel
            {
                Email = EmailConfiguracaoModel.Email,
                EnableSsl = true,
                Porta = EmailConfiguracaoModel.Porta,
                Senha = EmailConfiguracaoModel.Senha,
                Servidor = EmailConfiguracaoModel.Servidor
            });

        if (!enviado)
            return (ResultPartner<ResultadoPadraoViewModel>)
                "Não foi possível enviar o e-mail de recuperação de senha, tente novamente!";

        await _funcionarioEsqueceuSenhaRepository.AddAsync(solicitacao);
        await _funcionarioEsqueceuSenhaRepository.SaveChangesAsync();

        return (ResultPartner<ResultadoPadraoViewModel>)new ResultadoPadraoViewModel { Resultado = true };
    }

    public async Task<ResultPartner<ResultadoPadraoViewModel>> RecuperarSenhaAsync(RecuperarSenhaFuncionarioDto dto)
    {
        var erroDto = dto.Validar();

        if (!string.IsNullOrWhiteSpace(erroDto))
        {
            return (ResultPartner<ResultadoPadraoViewModel>)erroDto;
        }

        var solicitacao =
            await _funcionarioEsqueceuSenhaRepository.ObterPorTokenAsync(dto.Token, _parceiroAutenticado.Id);

        if (solicitacao == null)
        {
            return (ResultPartner<ResultadoPadraoViewModel>)"Token de recuperação inválido!";
        }

        var erro = solicitacao.PodeRecuperarSenha();

        if (!string.IsNullOrWhiteSpace(erro))
        {
            return (ResultPartner<ResultadoPadraoViewModel>)erro;
        }

        solicitacao.Funcionario.AtualizarSenha(dto.HashSenha());
        solicitacao.MarcarComoResetado();
        _funcionarioEsqueceuSenhaRepository.Update(solicitacao);
        await _funcionarioEsqueceuSenhaRepository.SaveChangesAsync();

        return (ResultPartner<ResultadoPadraoViewModel>)new ResultadoPadraoViewModel { Resultado = true };
    }
}