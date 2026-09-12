using OpenAdm.Application.Adapters;
using OpenAdm.Application.Dtos.Funcionarios;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model;

namespace OpenAdm.Application.Services;

public class TrocarSenhaFuncionarioService(
    IFuncionarioRepository funcionarioRepository,
    IUsuarioAutenticado usuarioAutenticado,
    IParceiroAutenticado parceiroAutenticado) : ITrocarSenhaFuncionarioService
{
    public async Task<ResultPartner<ResultadoPadraoViewModel>> TrocarAsync(TrocarSenhaFuncionarioDto dto)
    {
        var erro = dto.Validar();

        if (erro != null)
            return (ResultPartner<ResultadoPadraoViewModel>)erro;

        var funcionario = await funcionarioRepository.ObterPorIdAsync(usuarioAutenticado.Id, parceiroAutenticado.Id);

        if (funcionario == null)
        {
            return (ResultPartner<ResultadoPadraoViewModel>)"Não foi possível localizar o funcionário!";
        }

        if (!PasswordAdapter.VerifyPassword(dto.SenhaAtual, funcionario.Senha))
        {
            return (ResultPartner<ResultadoPadraoViewModel>)"Senha atual inválida!";
        }

        if (PasswordAdapter.VerifyPassword(dto.Senha, funcionario.Senha))
        {
            return (ResultPartner<ResultadoPadraoViewModel>)
                "A nova senha deve ser diferente da senha atual!";
        }

        funcionario.AtualizarSenha(PasswordAdapter.GenerateHash(dto.Senha));
        funcionarioRepository.Update(funcionario);
        await funcionarioRepository.SaveChangesAsync();

        return (ResultPartner<ResultadoPadraoViewModel>)new ResultadoPadraoViewModel { Resultado = true };
    }
}
