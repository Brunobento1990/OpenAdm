﻿using Domain.Pkg.Entities;
using Domain.Pkg.Errors;
using Domain.Pkg.Exceptions;
using OpenAdm.Application.Dtos.TabelasDePrecos;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.TabelaDePrecos;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model;
using OpenAdm.Infra.Paginacao;

namespace OpenAdm.Application.Services;

public class TabelaDePrecoService : ITabelaDePrecoService
{
    private readonly ITabelaDePrecoRepository _tabelaDePrecoRepository;
    private readonly ITamanhoRepository _tamanhoRepository;
    private readonly IPesoRepository _pesoRepository;

    public TabelaDePrecoService(
        ITabelaDePrecoRepository tabelaDePrecoRepository,
        ITamanhoRepository tamanhoRepository,
        IPesoRepository pesoRepository)
    {
        _tabelaDePrecoRepository = tabelaDePrecoRepository;
        _tamanhoRepository = tamanhoRepository;
        _pesoRepository = pesoRepository;
    }

    public async Task<TabelaDePrecoViewModel> CreateTabelaDePrecoAsync(CreateTabelaDePrecoDto createTabelaDePrecoDto)
    {
        var tabelaDePrecoAtiva = await _tabelaDePrecoRepository.GetTabelaDePrecoAtivaAsync();

        if (tabelaDePrecoAtiva != null && createTabelaDePrecoDto.AtivaEcommerce)
            throw new ExceptionApi("Uma tabela de preço já está ativa, desative a mesma, ou inative esta!");

        var tabelaDePreco = createTabelaDePrecoDto.ToEntity();

        await _tabelaDePrecoRepository.AddAsync(tabelaDePreco);

        var tabelaDePrecoViewModel = new TabelaDePrecoViewModel().ToModel(tabelaDePreco);

        var tamanhosIds = createTabelaDePrecoDto
            .ItensTabelaDePreco
            .Where(x => x.TamanhoId != null)
            .Select(x => x.TamanhoId.Value)
            .ToList();

        var pesosIds = createTabelaDePrecoDto
            .ItensTabelaDePreco
            .Where(x => x.PesoId != null)
            .Select(x => x.PesoId.Value)
            .ToList();

        var tamanhos = await _tamanhoRepository.GetTamanhosByIdsAsync(tamanhosIds);
        var pesos = await _pesoRepository.GetPesosByIdsAsync(pesosIds);

        tabelaDePrecoViewModel.ItensTabelaDePreco = tabelaDePreco
                .ItensTabelaDePreco.Select(x => new ItensTabelaDePrecoViewModel().ToModel(x, pesos, tamanhos))
                .ToList();

        return tabelaDePrecoViewModel;
    }

    public async Task DeleteTabelaDePrecoAsync(Guid id)
    {
        var tabelaDePreco = await GetTabelaAsync(id);

        var count = await _tabelaDePrecoRepository.GetCountTabelaDePrecoAsync();

        if (count <= 1)
            throw new ExceptionApi("Não é possível excluir a única tabela de preço cadastrada!");

        await _tabelaDePrecoRepository.DeleteAsync(tabelaDePreco);
    }

    public async Task<PaginacaoViewModel<TabelaDePrecoViewModel>> GetPaginacaoTabelaViewModelAsync(
        PaginacaoTabelaDePrecoDto paginacaoTabelaDePrecoDto)
    {
        var paginacao = await _tabelaDePrecoRepository.GetPaginacaoAsync(paginacaoTabelaDePrecoDto);

        return new PaginacaoViewModel<TabelaDePrecoViewModel>()
        {
            TotalPage = paginacao.TotalPage,
            Values = paginacao.Values.Select(x => new TabelaDePrecoViewModel().ToModel(x)).ToList()
        };
    }

    public async Task<TabelaDePrecoViewModel> GetPrecoTabelaViewModelAsync(Guid id)
    {
        var tabelaDePreco = await GetTabelaAsync(id);

        var tamanhosIds = tabelaDePreco
            .ItensTabelaDePreco
            .Where(x => x.TamanhoId != null)
            .Select(x => x.TamanhoId.Value)
            .ToList();

        var pesosIds = tabelaDePreco
            .ItensTabelaDePreco
            .Where(x => x.PesoId != null)
            .Select(x => x.PesoId.Value)
            .ToList();

        var tamanhos = await _tamanhoRepository.GetTamanhosByIdsAsync(tamanhosIds);
        var pesos = await _pesoRepository.GetPesosByIdsAsync(pesosIds);

        var tabelaDePrecoViewModel = new TabelaDePrecoViewModel().ToModel(tabelaDePreco);

        tabelaDePrecoViewModel.ItensTabelaDePreco = tabelaDePreco
            .ItensTabelaDePreco.Select(x => new ItensTabelaDePrecoViewModel().ToModel(x, pesos, tamanhos))
            .ToList();

        return tabelaDePrecoViewModel;
    }

    public async Task<TabelaDePrecoViewModel> UpdateTabelaDePrecoAsync(UpdateTabelaDePrecoDto updateTabelaDePrecoDto)
    {
        var tabelaDePreco = await _tabelaDePrecoRepository.GetTabelaDePrecoByIdUpdateAsync(updateTabelaDePrecoDto.Id)
            ?? throw new ExceptionApi(CodigoErrors.RegistroNotFound);

        var tabelaDePrecoAtiva = await _tabelaDePrecoRepository.GetTabelaDePrecoAtivaAsync();

        if (tabelaDePrecoAtiva == null && !updateTabelaDePrecoDto.AtivaEcommerce 
            || (!updateTabelaDePrecoDto.AtivaEcommerce && updateTabelaDePrecoDto.Id == tabelaDePrecoAtiva?.Id))
            throw new ExceptionApi("Não é possível ficar sem tabela de preço para o e-commerce, ativa a mesma e efetue o processo novamente!");

        tabelaDePreco.Update(updateTabelaDePrecoDto.Descricao, updateTabelaDePrecoDto.AtivaEcommerce);

        await _tabelaDePrecoRepository.UpdateAsync(tabelaDePreco);

        return new TabelaDePrecoViewModel().ToModel(tabelaDePreco);
    }

    private async Task<TabelaDePreco> GetTabelaAsync(Guid id)
    {
        return await _tabelaDePrecoRepository.GetTabelaDePrecoByIdAsync(id)
            ?? throw new ExceptionApi(CodigoErrors.RegistroNotFound);
    }
}
