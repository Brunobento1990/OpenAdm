using OpenAdm.Application.Dtos.Parceiros;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.Fretes;
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Application.Services;

internal class EnderecoUsuarioService : IEnderecoUsuarioService
{
    private readonly IUsuarioAutenticado _usuarioAutenticado;
    private readonly IUsuarioRepository _usuarioRepository;

    public EnderecoUsuarioService(IUsuarioAutenticado usuarioAutenticado, IUsuarioRepository usuarioRepository)
    {
        _usuarioAutenticado = usuarioAutenticado;
        _usuarioRepository = usuarioRepository;
    }

    public async Task<EnderecoViewModel> CriarOuEditarAsync(EnderecoDto enderecoDto)
    {
        enderecoDto.Validar();
        var endereco = await _usuarioRepository.ObterEnderecoAsync(_usuarioAutenticado.Id);

        if (endereco == null)
        {
            endereco = new Domain.Entities.EnderecoUsuario(
                cep: enderecoDto.Cep,
                logradouro: enderecoDto.Logradouro,
                bairro: enderecoDto.Bairro,
                localidade: enderecoDto.Localidade,
                complemento: enderecoDto.Complemento ?? "",
                numero: enderecoDto.Numero,
                uf: enderecoDto.Uf,
                id: Guid.NewGuid(),
                usuarioId: _usuarioAutenticado.Id);

            await _usuarioRepository.AddEnderecoAsync(endereco);
        }
        else
        {
            endereco.Editar(
                cep: enderecoDto.Cep,
                logradouro: enderecoDto.Logradouro,
                bairro: enderecoDto.Bairro,
                localidade: enderecoDto.Localidade,
                complemento: enderecoDto.Complemento ?? "",
                numero: enderecoDto.Numero,
                uf: enderecoDto.Uf);

            _usuarioRepository.EditarEndereco(endereco);
        }

        await _usuarioRepository.SaveChangesAsync();

        return new EnderecoViewModel()
        {
            Bairro = endereco.Bairro,
            Cep = endereco.Cep,
            Complemento = endereco.Complemento,
            Localidade = endereco.Localidade,
            Logradouro = endereco.Logradouro,
            Numero = endereco.Numero,
            Uf = endereco.Uf
        };
    }
}
