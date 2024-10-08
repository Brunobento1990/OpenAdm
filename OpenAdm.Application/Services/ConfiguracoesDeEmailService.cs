﻿using OpenAdm.Application.Dtos.ConfiguracoesDeEmails;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.ConfiguracoesDeEmails;
using OpenAdm.Domain.Helpers;
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Application.Services;

public class ConfiguracoesDeEmailService : IConfiguracoesDeEmailService
{
    private readonly IConfiguracaoDeEmailRepository _configuracaoDeEmailRepository;

    public ConfiguracoesDeEmailService(IConfiguracaoDeEmailRepository configuracaoDeEmailRepository)
    {
        _configuracaoDeEmailRepository = configuracaoDeEmailRepository;
    }

    public async Task<ConfiguracaoDeEmailViewModel> CreateConfiguracoesDeEmailAsync(CreateConfiguracoesDeEmailDto createConfiguracoesDeEmailDto)
    {
        var configuracaoDeEmail = await _configuracaoDeEmailRepository.GetConfiguracaoDeEmailAtivaAsync();

        if(configuracaoDeEmail == null)
        {
            configuracaoDeEmail = createConfiguracoesDeEmailDto.ToEntity();
            await _configuracaoDeEmailRepository.AddAsync(configuracaoDeEmail);
        }
        else
        {
            var newSenha = Criptografia.Encrypt(createConfiguracoesDeEmailDto.Senha);
            configuracaoDeEmail.Update(
                createConfiguracoesDeEmailDto.Email, 
                createConfiguracoesDeEmailDto.Servidor, 
                newSenha, 
                createConfiguracoesDeEmailDto.Porta, 
                createConfiguracoesDeEmailDto.Ativo.HasValue);

            await _configuracaoDeEmailRepository.UpdateAsync(configuracaoDeEmail);
        }

        return new ConfiguracaoDeEmailViewModel().ToModel(configuracaoDeEmail);
    }

    public async Task<ConfiguracaoDeEmailViewModel> GetConfiguracaoDeEmailAsync()
    {
        var configuracaoDeEmail = await _configuracaoDeEmailRepository.GetConfiguracaoDeEmailAtivaAsync();

        if (configuracaoDeEmail == null) return new();

        return new ConfiguracaoDeEmailViewModel().ToModel(configuracaoDeEmail);
    }
}
