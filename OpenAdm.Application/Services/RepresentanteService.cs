using OpenAdm.Application.Dtos.Representantes;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models;
using OpenAdm.Application.Models.Representantes;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Extensions;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model;
using OpenAdm.Domain.PaginateDto;

namespace OpenAdm.Application.Services;

public sealed class RepresentanteService(IRepresentanteRepository representanteRepository) : IRepresentanteService
{
    private const string RepresentanteNaoEncontrado = "Não foi possível localizar o representante";

    public async Task<ResultPartner<RepresentanteViewModel>> CriarAsync(CriarRepresentanteDto dto)
    {
        var erro = dto.Validar();
        if (erro != null)
            return (ResultPartner<RepresentanteViewModel>)erro;

        var cpf = dto.Cpf.LimparMascaraCpf();
        var email = dto.Email.NullSeVazio()?.ToLowerInvariant();
        var erroDuplicidade = await ValidarDuplicidadeAsync(cpf, email);
        if (erroDuplicidade != null)
            return (ResultPartner<RepresentanteViewModel>)erroDuplicidade;

        var representante = dto.ToEntity();
        await representanteRepository.AddAsync(representante);
        return (ResultPartner<RepresentanteViewModel>)RepresentanteViewModel.FromEntity(representante);
    }

    public async Task<ResultPartner<RepresentanteViewModel>> EditarAsync(EditarRepresentanteDto dto)
    {
        var erro = dto.Validar();
        if (erro != null)
            return (ResultPartner<RepresentanteViewModel>)erro;

        var representante = await representanteRepository.ObterPorIdAsync(dto.Id, tracking: true);
        if (representante == null)
            return (ResultPartner<RepresentanteViewModel>)RepresentanteNaoEncontrado;

        var cpf = dto.Cpf.LimparMascaraCpf();
        var email = dto.Email.NullSeVazio()?.ToLowerInvariant();
        var erroDuplicidade = await ValidarDuplicidadeAsync(cpf, email, dto.Id);
        if (erroDuplicidade != null)
            return (ResultPartner<RepresentanteViewModel>)erroDuplicidade;

        representante.Editar(dto.Nome.Trim(), cpf, email,
            dto.Telefone.NullSeVazio());
        await representanteRepository.UpdateAsync(representante);
        return (ResultPartner<RepresentanteViewModel>)RepresentanteViewModel.FromEntity(representante);
    }

    public async Task<ResultPartner<RepresentanteViewModel>> VisualizarAsync(Guid id)
    {
        var representante = await representanteRepository.ObterPorIdAsync(id);
        return representante == null
            ? (ResultPartner<RepresentanteViewModel>)RepresentanteNaoEncontrado
            : (ResultPartner<RepresentanteViewModel>)RepresentanteViewModel.FromEntity(representante);
    }

    public async Task<ResultPartner<ResultadoPadraoViewModel>> InativarAtivarAsync(Guid id, bool ativo)
    {
        var representante = await representanteRepository.ObterPorIdAsync(id, tracking: true);
        if (representante == null)
            return (ResultPartner<ResultadoPadraoViewModel>)RepresentanteNaoEncontrado;

        representante.InativarAtivar(ativo);
        await representanteRepository.UpdateAsync(representante);
        return (ResultPartner<ResultadoPadraoViewModel>)new ResultadoPadraoViewModel { Resultado = true };
    }

    public async Task<ResultPartner<PaginacaoViewModel<RepresentanteViewModel>>> PaginarAsync(
        FilterModel<Representante> filtro)
    {
        var pagina = await representanteRepository.PaginacaoAsync(filtro);
        return (ResultPartner<PaginacaoViewModel<RepresentanteViewModel>>)new PaginacaoViewModel<RepresentanteViewModel>
        {
            TotalDeRegistros = pagina.TotalDeRegistros,
            TotalPaginas = pagina.TotalPaginas,
            Values = pagina.Values.Select(RepresentanteViewModel.FromEntity).ToList()
        };
    }

    private async Task<string?> ValidarDuplicidadeAsync(string? cpf, string? email, Guid? ignorarId = null)
    {
        if (cpf == null && email == null)
            return null;

        var duplicidades = await representanteRepository.ObterDuplicidadesAsync(cpf, email, ignorarId);
        if (duplicidades.CpfDuplicado)
            return "Já existe um representante cadastrado com este CPF";
        if (duplicidades.EmailDuplicado)
            return "Já existe um representante cadastrado com este e-mail";
        return null;
    }
}
