using OpenAdm.Application.Dtos.Parceiros;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.Parceiros;
using OpenAdm.Domain.Entities.OpenAdm;
using OpenAdm.Domain.Exceptions;
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Application.Services;

public class ParceiroServico : IParceiroServico
{
    private readonly IParceiroAutenticado _parceiroAutenticado;
    private readonly IParceiroRepository _parceiroRepository;

    public ParceiroServico(IParceiroAutenticado parceiroAutenticado, IParceiroRepository parceiroRepository)
    {
        _parceiroAutenticado = parceiroAutenticado;
        _parceiroRepository = parceiroRepository;
    }

    public async Task<ParceiroViewModel> EditarAsync(ParceiroDto parceiroDto)
    {
        parceiroDto.Validar();

        var parceiro = await _parceiroRepository.ObterPorEmpresaOpenAdmIdAsync(_parceiroAutenticado.Id)
            ?? throw new ExceptionApi("Não foi possível localizar o cadastro da sua empresa");

        parceiro.Editar(
            razaoSocial: parceiroDto.RazaoSocial,
            nomeFantasia: parceiroDto.NomeFantasia,
            cnpj: parceiroDto.Cnpj,
            logo: parceiroDto.Logo);

        var novasRedesSociais = parceiroDto.RedesSociais.Where(x => !x.Id.HasValue).Select(x => RedeSocial.NovaRedeSocial(x.Link, x.RedeSocialEnum, parceiro.Id)).ToList();
        var novosTelefones = parceiroDto.Telefones.Where(x => !x.Id.HasValue).Select(x => TelefoneParceiro.NovoTelefone(x.Telefone, parceiro.Id)).ToList();
        var telefonesUpdate = new List<TelefoneParceiro>();
        var redesSociaisUpdate = new List<RedeSocial>();

        foreach (var telefone in parceiroDto.Telefones)
        {
            if (!telefone.Id.HasValue)
            {
                continue;
            }

            var telefoneParceiro = parceiro.Telefones.FirstOrDefault(x => x.Id == telefone.Id);
            if (telefoneParceiro == null)
            {
                continue;
            }

            telefoneParceiro.Editar(telefone.Telefone);
            telefonesUpdate.Add(telefoneParceiro);
        }

        foreach (var redeSocial in parceiroDto.RedesSociais)
        {
            if (!redeSocial.Id.HasValue)
            {
                continue;
            }

            var redeSocialParceiro = parceiro.RedesSociais.FirstOrDefault(x => x.Id == redeSocial.Id);
            if (redeSocialParceiro == null)
            {
                continue;
            }

            redeSocialParceiro.Editar(redeSocial.Link, redeSocial.RedeSocialEnum);
            redesSociaisUpdate.Add(redeSocialParceiro);
        }

        if (parceiroDto.EnderecoParceiro == null)
        {
            if (parceiro.EnderecoParceiro != null)
            {
                _parceiroRepository.RemoverEndereco(parceiro.EnderecoParceiro);
            }
        }
        else
        {
            if (parceiro.EnderecoParceiro == null)
            {
                await _parceiroRepository.AddEndereco(new Domain.Entities.EnderecoParceiro(
                    cep: parceiroDto.EnderecoParceiro.Cep,
                    logradouro: parceiroDto.EnderecoParceiro.Logradouro,
                    bairro: parceiroDto.EnderecoParceiro.Bairro,
                    localidade: parceiroDto.EnderecoParceiro.Localidade,
                    complemento: parceiroDto.EnderecoParceiro.Complemento ?? "",
                    numero: parceiroDto.EnderecoParceiro.Numero,
                    uf: parceiroDto.EnderecoParceiro.Uf,
                    id: Guid.NewGuid(),
                    parceiroId: parceiro.Id));
            }
            else
            {
                parceiro.EnderecoParceiro.Editar(
                    cep: parceiroDto.EnderecoParceiro.Cep,
                    logradouro: parceiroDto.EnderecoParceiro.Logradouro,
                    bairro: parceiroDto.EnderecoParceiro.Bairro,
                    localidade: parceiroDto.EnderecoParceiro.Localidade,
                    complemento: parceiroDto.EnderecoParceiro.Complemento ?? "",
                    numero: parceiroDto.EnderecoParceiro.Numero,
                    uf: parceiroDto.EnderecoParceiro.Uf);
            }
        }

        _parceiroRepository.UpdateRedesSociais(redesSociaisUpdate);
        _parceiroRepository.UpdateTelefones(telefonesUpdate);
        _parceiroRepository.Update(parceiro);
        await _parceiroRepository.AdicionarTelefonesAsync(novosTelefones);
        await _parceiroRepository.AdicionarRedesSociaisAsync(novasRedesSociais);
        await _parceiroRepository.SaveChangesAsync();

        return (ParceiroViewModel)parceiro;
    }

    public async Task<bool> ExcluirRedeSocialAsync(Guid redeSocialId)
    {
        var redeSocial = await _parceiroRepository.ObterRedeSocialAsync(redeSocialId)
            ?? throw new ExceptionApi("Não foi possível localizar o cadastro da rede social");

        _parceiroRepository.RemoverRedeSocial(redeSocial);
        await _parceiroRepository.SaveChangesAsync();

        return true;
    }

    public async Task<bool> ExcluirTelefoneAsync(Guid telefoneId)
    {
        var telefone = await _parceiroRepository.ObterTelefoneAsync(telefoneId)
            ?? throw new ExceptionApi("Não foi possível localizar o cadastro do telefone");

        _parceiroRepository.RemoverTelefone(telefone);
        await _parceiroRepository.SaveChangesAsync();

        return true;
    }

    public async Task<ParceiroViewModel> ObterParceiroAutenticadoAsync()
    {
        var parceiro = await _parceiroRepository.ObterPorEmpresaOpenAdmIdAsync(_parceiroAutenticado.Id)
            ?? throw new ExceptionApi("Não foi possível localizar o cadastro da sua empresa");

        return (ParceiroViewModel)parceiro;
    }
}
