using OpenAdm.Application.Dtos.Usuarios;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.Logins;
using OpenAdm.Application.Models.Usuarios;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Enuns;
using OpenAdm.Domain.Exceptions;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model;
using OpenAdm.Domain.PaginateDto;

namespace OpenAdm.Application.Services;

public class UsuarioService : IUsuarioService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly ITokenService _tokenService;
    private readonly IPedidoRepository _pedidoRepository;
    private readonly IUsuarioAutenticado _usuarioAutenticado;
    private readonly ICnpjConsultaService _cnpjConsultaService;

    public UsuarioService(
        IUsuarioRepository usuarioRepository,
        ITokenService tokenService,
        IPedidoRepository pedidoRepository,
        IUsuarioAutenticado usuarioAutenticado,
        ICnpjConsultaService cnpjConsultaService)
    {
        _usuarioRepository = usuarioRepository;
        _tokenService = tokenService;
        _pedidoRepository = pedidoRepository;
        _usuarioAutenticado = usuarioAutenticado;
        _cnpjConsultaService = cnpjConsultaService;
    }

    public async Task<ResponseLoginUsuarioViewModel> CreateUsuarioAsync(CreateUsuarioDto createUsuarioDto)
    {
        createUsuarioDto.Validar();
        var usuario = await _usuarioRepository.GetUsuarioByEmailAsync(createUsuarioDto.Email);

        if (usuario != null)
            throw new ExceptionApi("Este e-mail já se encontra cadastrado!");

        if (!string.IsNullOrWhiteSpace(createUsuarioDto.Cpf))
        {
            usuario = await _usuarioRepository.GetUsuarioByCpfAsync(createUsuarioDto.Cpf);
            if (usuario != null)
                throw new ExceptionApi("Este CPF já se encontra cadastrado!");
        }

        if (!string.IsNullOrWhiteSpace(createUsuarioDto.Cnpj))
        {
            usuario = await _usuarioRepository.GetUsuarioByCnpjAsync(createUsuarioDto.Cnpj);
            if (usuario != null)
                throw new ExceptionApi("Este CNPJ já se encontra cadastrado!");
        }

        if (createUsuarioDto.TipoPessoa == TipoPessoa.Juridica)
        {
            if (string.IsNullOrWhiteSpace(createUsuarioDto.Cnpj))
            {
                throw new ExceptionApi("Informe o CNPJ");
            }

            var result = await _cnpjConsultaService.ConsultaCnpjAsync(createUsuarioDto.Cnpj);

            if (!result.CnaeFiscalDescricao.Contains("pesca") && !result.CnaesSecundarios.Any(x => x.Descricao.Contains("pesca")))
            {
                throw new ExceptionApi("Seu CNAE deve ser do ramo de pesca");
            }
        }

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
            ?? throw new ExceptionApi("Não foi possível localizar o seu cadastro");

        var quantidadeDePedidos = await _pedidoRepository.GetQuantidadeDePedidoPorUsuarioAsync(usuario.Id);
        var usuarioViewModel = new UsuarioViewModel().ToModel(usuario, quantidadeDePedidos);

        usuarioViewModel.PedidosEmAberto = await _pedidoRepository
            .GetQuantidadePorStatusUsuarioAsync(usuario.Id, StatusPedido.Aberto);

        usuarioViewModel.PedidosFaturado = await _pedidoRepository
            .GetQuantidadePorStatusUsuarioAsync(usuario.Id, StatusPedido.Faturado);

        usuarioViewModel.PedidosEmEntraga = await _pedidoRepository
            .GetQuantidadePorStatusUsuarioAsync(usuario.Id, StatusPedido.RotaDeEntrega);

        usuarioViewModel.PedidosEntregue = await _pedidoRepository
            .GetQuantidadePorStatusUsuarioAsync(usuario.Id, StatusPedido.Entregue);

        usuarioViewModel.PedidosCancelados = await _pedidoRepository
            .GetQuantidadePorStatusUsuarioAsync(usuario.Id, StatusPedido.Cancelado);

        usuarioViewModel.TotalPedido = await _pedidoRepository
            .GetTotalPedidoPorUsuarioAsync(usuario.Id);

        return usuarioViewModel;
    }

    public async Task<UsuarioViewModel> GetUsuarioByIdAdmAsync(Guid id)
    {
        var usuario = await _usuarioRepository.GetUsuarioByIdAsync(id)
            ?? throw new ExceptionApi("Não foi possível localizar o cadastro do usuario");

        //var quantidadeDePedidos = await _pedidoRepository.GetQuantidadeDePedidoPorUsuarioAsync(usuario.Id);
        var usuarioViewModel = new UsuarioViewModel().ToModel(usuario, 0);

        //usuarioViewModel.PedidosEmAberto = await _pedidoRepository
        //    .GetQuantidadePorStatusUsuarioAsync(usuario.Id, StatusPedido.Aberto);

        //usuarioViewModel.PedidosFaturado = await _pedidoRepository
        //    .GetQuantidadePorStatusUsuarioAsync(usuario.Id, StatusPedido.Faturado);

        //usuarioViewModel.PedidosEmEntraga = await _pedidoRepository
        //    .GetQuantidadePorStatusUsuarioAsync(usuario.Id, StatusPedido.RotaDeEntrega);

        //usuarioViewModel.PedidosEntregue = await _pedidoRepository
        //    .GetQuantidadePorStatusUsuarioAsync(usuario.Id, StatusPedido.Entregue);

        //usuarioViewModel.PedidosCancelados = await _pedidoRepository
        //    .GetQuantidadePorStatusUsuarioAsync(usuario.Id, StatusPedido.Cancelado);

        //usuarioViewModel.TotalPedido = await _pedidoRepository
        //    .GetTotalPedidoPorUsuarioAsync(usuario.Id);

        return usuarioViewModel;
    }

    public async Task<PaginacaoViewModel<UsuarioViewModel>> PaginacaoAsync(FilterModel<Usuario> paginacaoUsuarioDto)
    {
        var paginacao = await _usuarioRepository.PaginacaoAsync(paginacaoUsuarioDto);

        return new PaginacaoViewModel<UsuarioViewModel>()
        {
            TotalDeRegistros = paginacao.TotalDeRegistros,
            TotalPaginas = paginacao.TotalPaginas,
            Values = paginacao.Values.Select(x => new UsuarioViewModel().ToModel(x)).ToList()
        };
    }

    public async Task TrocarSenhaAsync(UpdateSenhaUsuarioDto updateSenhaUsuarioDto)
    {
        var usuario = await _usuarioRepository.GetUsuarioByIdAsync(_usuarioAutenticado.Id);
        if (usuario == null || !updateSenhaUsuarioDto.Senha.Equals(updateSenhaUsuarioDto.ReSenha))
            throw new ExceptionApi("As senhas não conferem!");

        usuario.UpdateSenha(updateSenhaUsuarioDto.HashSenha());

        await _usuarioRepository.UpdateAsync(usuario);
    }

    public async Task<ResponseLoginUsuarioViewModel> UpdateUsuarioAsync(UpdateUsuarioDto updateUsuarioDto)
    {
        updateUsuarioDto.Validar();
        var usuario = await _usuarioRepository.GetUsuarioByIdAsync(_usuarioAutenticado.Id)
            ?? throw new ExceptionApi("Não foi possível localizar seu cadastro");

        usuario.Update(updateUsuarioDto.Email, updateUsuarioDto.Nome, updateUsuarioDto.Telefone, updateUsuarioDto.Cnpj, updateUsuarioDto.Cpf);

        await _usuarioRepository.UpdateAsync(usuario);
        var usuarioViewModel = new UsuarioViewModel().ToModel(usuario);
        var token = _tokenService.GenerateToken(usuarioViewModel);
        var refreshToken = _tokenService.GenerateRefreshToken(usuarioViewModel.Id);

        return new(usuarioViewModel, token, refreshToken);
    }

    public async Task<IList<UsuarioViewModel>> PaginacaoDropDownAsync(PaginacaoDropDown<Usuario> paginacaoUsuarioDropDown)
    {
        var usuarios = await _usuarioRepository.PaginacaoDropDownAsync(paginacaoUsuarioDropDown);
        return usuarios.Select(x => new UsuarioViewModel()
        {
            Cnpj = string.IsNullOrWhiteSpace(x.Cnpj) ? x.Cpf : x.Cnpj,
            Id = x.Id,
            Nome = x.Nome
        }).ToList();
    }

    public async Task<bool> TemTelefoneCadastradoAsync()
    {
        var usuario = await _usuarioAutenticado.GetUsuarioAutenticadoAsync();
        return !string.IsNullOrWhiteSpace(usuario.Telefone);
    }

    public async Task<UsuarioViewModel> GetUsuarioByIdValidacaoAsync(Guid id)
    {
        var usuario = await _usuarioRepository.GetUsuarioByIdAsync(id)
            ?? throw new ExceptionApi("Não foi possível localizar o cadastro do usuario");

        var usuarioViewModel = new UsuarioViewModel().ToModel(usuario, 0);

        return usuarioViewModel;
    }

    public async Task<ResponseLoginUsuarioViewModel> CreateUsuarioNoAdminAsync(CreateUsuarioDto createUsuarioDto)
    {
        createUsuarioDto.Validar();
        var usuario = await _usuarioRepository.GetUsuarioByEmailAsync(createUsuarioDto.Email);

        if (usuario != null)
            throw new ExceptionApi("Este e-mail já se encontra cadastrado!");

        if (!string.IsNullOrWhiteSpace(createUsuarioDto.Cpf))
        {
            usuario = await _usuarioRepository.GetUsuarioByCpfAsync(createUsuarioDto.Cpf);
            if (usuario != null)
                throw new ExceptionApi("Este CPF já se encontra cadastrado!");
        }

        if (!string.IsNullOrWhiteSpace(createUsuarioDto.Cnpj))
        {
            usuario = await _usuarioRepository.GetUsuarioByCnpjAsync(createUsuarioDto.Cnpj);
            if (usuario != null)
                throw new ExceptionApi("Este CNPJ já se encontra cadastrado!");
        }

        usuario = createUsuarioDto.ToEntity();

        await _usuarioRepository.AddAsync(usuario);

        var usuarioViewModel = new UsuarioViewModel().ToModel(usuario);

        return new ResponseLoginUsuarioViewModel(usuarioViewModel, "", "");
    }
}
