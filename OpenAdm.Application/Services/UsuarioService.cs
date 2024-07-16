using OpenAdm.Application.Dtos.Usuarios;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.Logins;
using OpenAdm.Application.Models.Usuarios;
using Domain.Pkg.Errors;
using Domain.Pkg.Exceptions;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model;
using OpenAdm.Infra.Paginacao;

namespace OpenAdm.Application.Services;

public class UsuarioService : IUsuarioService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly ITokenService _tokenService;
    private readonly IPedidoRepository _pedidoRepository;
    private readonly IUsuarioAutenticado _usuarioAutenticado;
    public UsuarioService(
        IUsuarioRepository usuarioRepository,
        ITokenService tokenService,
        IPedidoRepository pedidoRepository,
        IUsuarioAutenticado usuarioAutenticado)
    {
        _usuarioRepository = usuarioRepository;
        _tokenService = tokenService;
        _pedidoRepository = pedidoRepository;
        _usuarioAutenticado = usuarioAutenticado;
    }

    public async Task<ResponseLoginUsuarioViewModel> CreateUsuarioAsync(CreateUsuarioDto createUsuarioDto)
    {
        var usuario = await _usuarioRepository.GetUsuarioByEmailAsync(createUsuarioDto.Email);

        if (usuario != null)
            throw new ExceptionApi(CodigoErrors.UsuarioEcommerceJaCadastrado);

        usuario = createUsuarioDto.ToEntity();

        await _usuarioRepository.AddAsync(usuario);

        var usuarioViewModel = new UsuarioViewModel().ToModel(usuario);
        var token = _tokenService.GenerateToken(usuarioViewModel);
        var refreshToken = _tokenService.GenerateRefreshToken(usuarioViewModel.Id);

        return new ResponseLoginUsuarioViewModel(usuarioViewModel, token, refreshToken);
    }

    public async Task<IList<UsuarioViewModel>> GetAllUsuariosAsync()
    {
        var usuarios = await _usuarioRepository.GetAllUsuariosAsync();
        return usuarios.Select(x => new UsuarioViewModel().ToModel(x)).ToList();
    }

    public async Task<UsuarioViewModel> GetUsuarioByIdAsync()
    {
        var usuario = await _usuarioRepository.GetUsuarioByIdAsync(_usuarioAutenticado.Id)
            ?? throw new ExceptionApi(CodigoErrors.RegistroNotFound);

        var quantidadeDePedidos = await _pedidoRepository.GetQuantidadeDePedidoPorUsuarioAsync(usuario.Id);

        return new UsuarioViewModel().ToModel(usuario, quantidadeDePedidos);
    }

    public async Task<PaginacaoViewModel<UsuarioViewModel>> PaginacaoAsync(PaginacaoUsuarioDto paginacaoUsuarioDto)
    {
        var paginacao = await _usuarioRepository.GetPaginacaoAsync(paginacaoUsuarioDto);

        return new PaginacaoViewModel<UsuarioViewModel>()
        {
            TotalPage = paginacao.TotalPage,
            Values = paginacao.Values.Select(x => new UsuarioViewModel().ToModel(x)).ToList()
        };
    }

    public async Task TrocarSenhaAsync(UpdateSenhaUsuarioDto updateSenhaUsuarioDto)
    {
        var usuario = await _usuarioRepository.GetUsuarioByIdAsync(_usuarioAutenticado.Id);
        if (usuario == null || !updateSenhaUsuarioDto.Senha.Equals(updateSenhaUsuarioDto.ReSenha))
            throw new ExceptionApi(CodigoErrors.AsSenhaNaoConferem);

        usuario.UpdateSenha(updateSenhaUsuarioDto.HashSenha());

        await _usuarioRepository.UpdateAsync(usuario);
    }

    public async Task<ResponseLoginUsuarioViewModel> UpdateUsuarioAsync(UpdateUsuarioDto updateUsuarioDto)
    {
        var usuario = await _usuarioRepository.GetUsuarioByIdAsync(_usuarioAutenticado.Id)
            ?? throw new ExceptionApi(CodigoErrors.RegistroNotFound);

        usuario.Update(updateUsuarioDto.Email, updateUsuarioDto.Nome, updateUsuarioDto.Telefone, updateUsuarioDto.Cnpj, updateUsuarioDto.Cpf);

        await _usuarioRepository.UpdateAsync(usuario);
        var usuarioViewModel = new UsuarioViewModel().ToModel(usuario);
        var token = _tokenService.GenerateToken(usuarioViewModel);
        var refreshToken = _tokenService.GenerateRefreshToken(usuarioViewModel.Id);

        return new(usuarioViewModel, token, refreshToken);
    }
}
