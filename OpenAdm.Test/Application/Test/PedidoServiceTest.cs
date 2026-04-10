using OpenAdm.Application.Interfaces;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Application.Services.Pedidos;
using OpenAdm.Domain.Model.Pedidos;
using OpenAdm.Application.Dtos.Pedidos;
using OpenAdm.Application.Dtos.Parceiros;
using OpenAdm.Application.Models.ConfiguracoesDePagamentos;
using OpenAdm.Application.Models.ConfiguracoesDePedidos;
using OpenAdm.Domain.Entities;

namespace OpenAdm.Test.Application.Test;

public class PedidoServiceTest
{
    private readonly Mock<IPedidoRepository> _pedidoRepositoryMock;
    private readonly Mock<IProcessarPedidoService> _processarPedidoServiceMock;
    private readonly Mock<IItemTabelaDePrecoRepository> _itemTabelaDePrecoRepositoryMock;
    private readonly Mock<ICarrinhoRepository> _carrinhoRepositoryMock;
    private readonly Mock<IFaturaService> _contasAReceberServiceMock;
    private readonly Mock<IConfiguracaoDePagamentoService> _configuracaoDePagamentoServiceMock;
    private readonly Mock<IConfiguracoesDePedidoService> _configuracoesDePedidoServiceMock;
    private readonly Mock<IUsuarioAutenticado> _usuarioAutenticadoMock;
    private readonly Mock<IEstoqueRepository> _estoqueRepositoryMock;
    private readonly Mock<IConfiguracaoDeFreteService> _configuracaoDeFreteServiceMock;

    private readonly CreatePedidoService _createPedidoService;

    public PedidoServiceTest()
    {
        _pedidoRepositoryMock = new Mock<IPedidoRepository>();
        _processarPedidoServiceMock = new Mock<IProcessarPedidoService>();
        _itemTabelaDePrecoRepositoryMock = new Mock<IItemTabelaDePrecoRepository>();
        _carrinhoRepositoryMock = new Mock<ICarrinhoRepository>();
        _contasAReceberServiceMock = new Mock<IFaturaService>();
        _configuracaoDePagamentoServiceMock = new Mock<IConfiguracaoDePagamentoService>();
        _configuracoesDePedidoServiceMock = new Mock<IConfiguracoesDePedidoService>();
        _usuarioAutenticadoMock = new Mock<IUsuarioAutenticado>();
        _estoqueRepositoryMock = new Mock<IEstoqueRepository>();
        _configuracaoDeFreteServiceMock = new Mock<IConfiguracaoDeFreteService>();

        _createPedidoService = new CreatePedidoService(
            _pedidoRepositoryMock.Object,
            _processarPedidoServiceMock.Object,
            _itemTabelaDePrecoRepositoryMock.Object,
            _carrinhoRepositoryMock.Object,
            _contasAReceberServiceMock.Object,
            _configuracaoDePagamentoServiceMock.Object,
            _configuracoesDePedidoServiceMock.Object,
            _usuarioAutenticadoMock.Object,
            _estoqueRepositoryMock.Object,
            _configuracaoDeFreteServiceMock.Object);
    }

    private static ItemPedidoModel BuildItemValido() => new()
    {
        ProdutoId = Guid.NewGuid(),
        Quantidade = 1,
        ValorUnitario = 10
    };

    [Fact]
    public async Task CreatePedidoAsync_DeveRetornarErro_QuandoEnderecoEntregaForNulo()
    {
        var dto = new PedidoCreateDto
        {
            EnderecoEntrega = null!,
            Itens = [BuildItemValido()]
        };

        var result = await _createPedidoService.CreatePedidoAsync(dto);

        Assert.Equal("Informe o endereço de entrega", result.Error);
    }

    [Fact]
    public async Task CreatePedidoAsync_DeveRetornarErro_QuandoItensForVazio()
    {
        var dto = new PedidoCreateDto
        {
            EnderecoEntrega = new EnderecoDto
            {
                Cep = "01310100",
                Logradouro = "Avenida Paulista",
                Bairro = "Bela Vista",
                Localidade = "São Paulo",
                Numero = "1578",
                Uf = "SP"
            },
            Itens = []
        };

        var result = await _createPedidoService.CreatePedidoAsync(dto);

        Assert.Equal("Informe os itens do pedido", result.Error);
    }

    [Fact]
    public async Task CreatePedidoAsync_DeveRetornarErro_QuandoCepForVazio()
    {
        var dto = new PedidoCreateDto
        {
            EnderecoEntrega = new EnderecoDto
            {
                Cep = "",
                Logradouro = "Avenida Paulista",
                Bairro = "Bela Vista",
                Localidade = "São Paulo",
                Numero = "1578",
                Uf = "SP"
            },
            Itens = [BuildItemValido()]
        };

        var result = await _createPedidoService.CreatePedidoAsync(dto);

        Assert.NotNull(result.Error);
        Assert.Contains("Informe o CEP", result.Error);
    }

    [Fact]
    public async Task CreatePedidoAsync_DeveRetornarErro_QuandoCepExcederOitoCaracteres()
    {
        var dto = new PedidoCreateDto
        {
            EnderecoEntrega = new EnderecoDto
            {
                Cep = "013101001",
                Logradouro = "Avenida Paulista",
                Bairro = "Bela Vista",
                Localidade = "São Paulo",
                Numero = "1578",
                Uf = "SP"
            },
            Itens = [BuildItemValido()]
        };

        var result = await _createPedidoService.CreatePedidoAsync(dto);

        Assert.NotNull(result.Error);
        Assert.Contains("O CEP deve conter no máximo 8 caracteres", result.Error);
    }

    [Fact]
    public async Task CreatePedidoAsync_DeveRetornarErro_QuandoLogradouroForVazio()
    {
        var dto = new PedidoCreateDto
        {
            EnderecoEntrega = new EnderecoDto
            {
                Cep = "01310100",
                Logradouro = "",
                Bairro = "Bela Vista",
                Localidade = "São Paulo",
                Numero = "1578",
                Uf = "SP"
            },
            Itens = [BuildItemValido()]
        };

        var result = await _createPedidoService.CreatePedidoAsync(dto);

        Assert.NotNull(result.Error);
        Assert.Contains("Informe a rua", result.Error);
    }

    [Fact]
    public async Task CreatePedidoAsync_DeveRetornarErro_QuandoLogradouroExceder255Caracteres()
    {
        var dto = new PedidoCreateDto
        {
            EnderecoEntrega = new EnderecoDto
            {
                Cep = "01310100",
                Logradouro = new string('A', 256),
                Bairro = "Bela Vista",
                Localidade = "São Paulo",
                Numero = "1578",
                Uf = "SP"
            },
            Itens = [BuildItemValido()]
        };

        var result = await _createPedidoService.CreatePedidoAsync(dto);

        Assert.NotNull(result.Error);
        Assert.Contains("A rua deve conter no máximo 255 caracteres", result.Error);
    }

    [Fact]
    public async Task CreatePedidoAsync_DeveRetornarErro_QuandoBairroForVazio()
    {
        var dto = new PedidoCreateDto
        {
            EnderecoEntrega = new EnderecoDto
            {
                Cep = "01310100",
                Logradouro = "Avenida Paulista",
                Bairro = "",
                Localidade = "São Paulo",
                Numero = "1578",
                Uf = "SP"
            },
            Itens = [BuildItemValido()]
        };

        var result = await _createPedidoService.CreatePedidoAsync(dto);

        Assert.NotNull(result.Error);
        Assert.Contains("Informe o bairro", result.Error);
    }

    [Fact]
    public async Task CreatePedidoAsync_DeveRetornarErro_QuandoBairroExceder255Caracteres()
    {
        var dto = new PedidoCreateDto
        {
            EnderecoEntrega = new EnderecoDto
            {
                Cep = "01310100",
                Logradouro = "Avenida Paulista",
                Bairro = new string('A', 256),
                Localidade = "São Paulo",
                Numero = "1578",
                Uf = "SP"
            },
            Itens = [BuildItemValido()]
        };

        var result = await _createPedidoService.CreatePedidoAsync(dto);

        Assert.NotNull(result.Error);
        Assert.Contains("O bairro deve conter no máximo 255 caracteres", result.Error);
    }

    [Fact]
    public async Task CreatePedidoAsync_DeveRetornarErro_QuandoLocalidadeForVazia()
    {
        var dto = new PedidoCreateDto
        {
            EnderecoEntrega = new EnderecoDto
            {
                Cep = "01310100",
                Logradouro = "Avenida Paulista",
                Bairro = "Bela Vista",
                Localidade = "",
                Numero = "1578",
                Uf = "SP"
            },
            Itens = [BuildItemValido()]
        };

        var result = await _createPedidoService.CreatePedidoAsync(dto);

        Assert.NotNull(result.Error);
        Assert.Contains("Informe a cidade", result.Error);
    }

    [Fact]
    public async Task CreatePedidoAsync_DeveRetornarErro_QuandoLocalidadeExceder255Caracteres()
    {
        var dto = new PedidoCreateDto
        {
            EnderecoEntrega = new EnderecoDto
            {
                Cep = "01310100",
                Logradouro = "Avenida Paulista",
                Bairro = "Bela Vista",
                Localidade = new string('A', 256),
                Numero = "1578",
                Uf = "SP"
            },
            Itens = [BuildItemValido()]
        };

        var result = await _createPedidoService.CreatePedidoAsync(dto);

        Assert.NotNull(result.Error);
        Assert.Contains("A cidade deve conter no máximo 255 caracteres", result.Error);
    }

    [Fact]
    public async Task CreatePedidoAsync_DeveRetornarErro_QuandoComplementoExceder255Caracteres()
    {
        var dto = new PedidoCreateDto
        {
            EnderecoEntrega = new EnderecoDto
            {
                Cep = "01310100",
                Logradouro = "Avenida Paulista",
                Bairro = "Bela Vista",
                Localidade = "São Paulo",
                Complemento = new string('A', 256),
                Numero = "1578",
                Uf = "SP"
            },
            Itens = [BuildItemValido()]
        };

        var result = await _createPedidoService.CreatePedidoAsync(dto);

        Assert.NotNull(result.Error);
        Assert.Contains("O complemento deve conter no máximo 255 caracteres", result.Error);
    }

    [Fact]
    public void CreatePedidoAsync_NaoDeveRetornarErroDeComplemento_QuandoComplementoForNulo()
    {
        var dto = new PedidoCreateDto
        {
            EnderecoEntrega = new EnderecoDto
            {
                Cep = "01310100",
                Logradouro = "Avenida Paulista",
                Bairro = "Bela Vista",
                Localidade = "São Paulo",
                Complemento = null,
                Numero = "1578",
                Uf = "SP"
            },
            Itens = [BuildItemValido()]
        };

        var erro = dto.Validar();

        Assert.Null(erro);
    }

    [Fact]
    public async Task CreatePedidoAsync_DeveRetornarErro_QuandoNumeroForVazio()
    {
        var dto = new PedidoCreateDto
        {
            EnderecoEntrega = new EnderecoDto
            {
                Cep = "01310100",
                Logradouro = "Avenida Paulista",
                Bairro = "Bela Vista",
                Localidade = "São Paulo",
                Numero = "",
                Uf = "SP"
            },
            Itens = [BuildItemValido()]
        };

        var result = await _createPedidoService.CreatePedidoAsync(dto);

        Assert.NotNull(result.Error);
        Assert.Contains("Informe o número", result.Error);
    }

    [Fact]
    public async Task CreatePedidoAsync_DeveRetornarErro_QuandoNumeroExcederDezCaracteres()
    {
        var dto = new PedidoCreateDto
        {
            EnderecoEntrega = new EnderecoDto
            {
                Cep = "01310100",
                Logradouro = "Avenida Paulista",
                Bairro = "Bela Vista",
                Localidade = "São Paulo",
                Numero = "12345678901",
                Uf = "SP"
            },
            Itens = [BuildItemValido()]
        };

        var result = await _createPedidoService.CreatePedidoAsync(dto);

        Assert.NotNull(result.Error);
        Assert.Contains("O número deve conter no máximo 10 caracteres", result.Error);
    }

    [Fact]
    public async Task CreatePedidoAsync_DeveRetornarErro_QuandoUfForVazio()
    {
        var dto = new PedidoCreateDto
        {
            EnderecoEntrega = new EnderecoDto
            {
                Cep = "01310100",
                Logradouro = "Avenida Paulista",
                Bairro = "Bela Vista",
                Localidade = "São Paulo",
                Numero = "1578",
                Uf = ""
            },
            Itens = [BuildItemValido()]
        };

        var result = await _createPedidoService.CreatePedidoAsync(dto);

        Assert.NotNull(result.Error);
        Assert.Contains("Informe o estado", result.Error);
    }

    [Fact]
    public async Task CreatePedidoAsync_DeveRetornarErro_QuandoUfExcederDoisCaracteres()
    {
        var dto = new PedidoCreateDto
        {
            EnderecoEntrega = new EnderecoDto
            {
                Cep = "01310100",
                Logradouro = "Avenida Paulista",
                Bairro = "Bela Vista",
                Localidade = "São Paulo",
                Numero = "1578",
                Uf = "SPP"
            },
            Itens = [BuildItemValido()]
        };

        var result = await _createPedidoService.CreatePedidoAsync(dto);

        Assert.NotNull(result.Error);
        Assert.Contains("O estado deve conter no máximo 2 caracteres", result.Error);
    }

    // ─── Helpers ────────────────────────────────────────────────────────────────

    private static PedidoCreateDto BuildDtoValido(IList<ItemPedidoModel>? itens = null) => new()
    {
        EnderecoEntrega = new EnderecoDto
        {
            Cep = "01310100",
            Logradouro = "Avenida Paulista",
            Bairro = "Bela Vista",
            Localidade = "São Paulo",
            Numero = "1578",
            Uf = "SP"
        },
        Itens = itens ?? [BuildItemValido()]
    };

    private static Usuario BuildUsuarioComTelefone(bool atacado = true)
    {
        var date = DateTime.Now;
        return new Usuario(
            Guid.NewGuid(), date, date, 0,
            "usuario@teste.com", "senha123", "Usuário Teste",
            "11999999999",
            cnpj: atacado ? "12345678000100" : null,
            cpf: atacado ? null : "12345678901",
            ativo: true,
            tokenEsqueceuSenha: null,
            dataExpiracaoTokenEsqueceuSenha: null,
            forcarLogin: null);
    }

    private void SetupFreteNaoCobra() =>
        _configuracaoDeFreteServiceMock
            .Setup(x => x.CobrarAsync())
            .ReturnsAsync(new EfetuarCobrancaViewModel { Cobrar = false });

    private void SetupFreteCobra() =>
        _configuracaoDeFreteServiceMock
            .Setup(x => x.CobrarAsync())
            .ReturnsAsync(new EfetuarCobrancaViewModel { Cobrar = true });

    private void SetupConfiguracaoDePedido(
        decimal? pedidoMinimoAtacado = null,
        decimal? pedidoMinimoVarejo = null,
        bool vendaDeProdutoComEstoque = false) =>
        _configuracoesDePedidoServiceMock
            .Setup(x => x.GetConfiguracoesDePedidoAsync())
            .ReturnsAsync(new ConfiguracoesDePedidoViewModel
            {
                PedidoMinimoAtacado = pedidoMinimoAtacado,
                PedidoMinimoVarejo = pedidoMinimoVarejo,
                VendaDeProdutoComEstoque = vendaDeProdutoComEstoque
            });

    private static ItemTabelaDePreco BuildItemTabelaDePreco(
        Guid produtoId, Guid? pesoId = null, Guid? tamanhoId = null,
        bool vendaSomenteComEstoque = false)
    {
        var date = DateTime.Now;
        var item = new ItemTabelaDePreco(
            Guid.NewGuid(), date, date, 0,
            produtoId,
            valorUnitarioAtacado: 10,
            valorUnitarioVarejo: 10,
            tabelaDePrecoId: Guid.NewGuid(),
            tamanhoId: tamanhoId,
            pesoId: pesoId);
        item.Produto = new Produto(
            produtoId, date, date, 0,
            "Produto Teste", null,
            Guid.NewGuid(), null, null, null,
            inativoEcommerce: false,
            vendaSomenteComEstoqueDisponivel: vendaSomenteComEstoque);
        return item;
    }

    // ─── Testes dos ifs do método ────────────────────────────────────────────────

    [Fact]
    public async Task CreatePedidoAsync_DeveRetornarErro_QuandoUsuarioNaoTemTelefone()
    {
        var usuario = new Usuario(
            Guid.NewGuid(), DateTime.Now, DateTime.Now, 0,
            "usuario@teste.com", "senha123", "Usuário Teste",
            telefone: null,
            cnpj: "12345678000100", cpf: null,
            ativo: true, tokenEsqueceuSenha: null, dataExpiracaoTokenEsqueceuSenha: null, forcarLogin: null);
        _usuarioAutenticadoMock.Setup(x => x.GetUsuarioAutenticadoAsync()).ReturnsAsync(usuario);

        var result = await _createPedidoService.CreatePedidoAsync(BuildDtoValido());

        Assert.Equal("Seu cadastro esta incompleto, acesse sua conta e cadastre seu telefone!", result.Error);
    }

    [Fact]
    public async Task CreatePedidoAsync_DeveRetornarErro_QuandoCobraFreteEFreteIdNaoInformado()
    {
        _usuarioAutenticadoMock.Setup(x => x.GetUsuarioAutenticadoAsync()).ReturnsAsync(BuildUsuarioComTelefone());
        SetupFreteCobra();
        var dto = BuildDtoValido();
        dto.FreteId = null;
        dto.ValorFrete = 20;

        var result = await _createPedidoService.CreatePedidoAsync(dto);

        Assert.Equal("Seu pedido não foi selecionado um frete", result.Error);
    }

    [Fact]
    public async Task CreatePedidoAsync_DeveRetornarErro_QuandoCobraFreteEFreteIdZero()
    {
        _usuarioAutenticadoMock.Setup(x => x.GetUsuarioAutenticadoAsync()).ReturnsAsync(BuildUsuarioComTelefone());
        SetupFreteCobra();
        var dto = BuildDtoValido();
        dto.FreteId = 0;
        dto.ValorFrete = 20;

        var result = await _createPedidoService.CreatePedidoAsync(dto);

        Assert.Equal("Seu pedido não foi selecionado um frete", result.Error);
    }

    [Fact]
    public async Task CreatePedidoAsync_DeveRetornarErro_QuandoCobraFreteEValorFreteNaoInformado()
    {
        _usuarioAutenticadoMock.Setup(x => x.GetUsuarioAutenticadoAsync()).ReturnsAsync(BuildUsuarioComTelefone());
        SetupFreteCobra();
        var dto = BuildDtoValido();
        dto.FreteId = 1;
        dto.ValorFrete = null;

        var result = await _createPedidoService.CreatePedidoAsync(dto);

        Assert.Equal("Seu pedido não foi selecionado um frete", result.Error);
    }

    [Fact]
    public async Task CreatePedidoAsync_DeveRetornarErro_QuandoCobraFreteEValorFreteZero()
    {
        _usuarioAutenticadoMock.Setup(x => x.GetUsuarioAutenticadoAsync()).ReturnsAsync(BuildUsuarioComTelefone());
        SetupFreteCobra();
        var dto = BuildDtoValido();
        dto.FreteId = 1;
        dto.ValorFrete = 0;

        var result = await _createPedidoService.CreatePedidoAsync(dto);

        Assert.Equal("Seu pedido não foi selecionado um frete", result.Error);
    }

    [Fact]
    public async Task CreatePedidoAsync_DeveRetornarErro_QuandoPedidoNaoAtingeMinimoPedidoAtacado()
    {
        var usuario = BuildUsuarioComTelefone(atacado: true);
        _usuarioAutenticadoMock.Setup(x => x.GetUsuarioAutenticadoAsync()).ReturnsAsync(usuario);
        SetupFreteNaoCobra();
        SetupConfiguracaoDePedido(pedidoMinimoAtacado: 1000);

        // item: quantidade=1, valorUnitario=10 → total=10, abaixo de 1000
        var dto = BuildDtoValido([
            new ItemPedidoModel { ProdutoId = Guid.NewGuid(), Quantidade = 1, ValorUnitario = 10 }
        ]);

        var result = await _createPedidoService.CreatePedidoAsync(dto);

        Assert.NotNull(result.Error);
        Assert.Contains("Seu pedido não atingiu o mínimo configurado", result.Error);
        Assert.Contains("1000", result.Error);
    }

    [Fact]
    public async Task CreatePedidoAsync_DeveRetornarErro_QuandoPedidoNaoAtingeMinimoPedidoVarejo()
    {
        var usuario = BuildUsuarioComTelefone(atacado: false);
        _usuarioAutenticadoMock.Setup(x => x.GetUsuarioAutenticadoAsync()).ReturnsAsync(usuario);
        SetupFreteNaoCobra();
        SetupConfiguracaoDePedido(pedidoMinimoVarejo: 500);

        var dto = BuildDtoValido([
            new ItemPedidoModel { ProdutoId = Guid.NewGuid(), Quantidade = 1, ValorUnitario = 10 }
        ]);

        var result = await _createPedidoService.CreatePedidoAsync(dto);

        Assert.NotNull(result.Error);
        Assert.Contains("Seu pedido não atingiu o mínimo configurado", result.Error);
        Assert.Contains("500", result.Error);
    }

    [Fact]
    public async Task CreatePedidoAsync_DeveRetornarErro_QuandoItemTabelaDePrecoNaoEncontrado()
    {
        _usuarioAutenticadoMock.Setup(x => x.GetUsuarioAutenticadoAsync()).ReturnsAsync(BuildUsuarioComTelefone());
        SetupFreteNaoCobra();
        SetupConfiguracaoDePedido();
        _itemTabelaDePrecoRepositoryMock
            .Setup(x => x.GetItensTabelaDePrecoByIdProdutosAsync(It.IsAny<IList<Guid>>()))
            .ReturnsAsync([]);
        _estoqueRepositoryMock
            .Setup(x => x.GetPosicaoEstoqueDosProdutosAsync(It.IsAny<IList<Guid>>()))
            .ReturnsAsync([]);

        var produtoId = Guid.NewGuid();
        var dto = BuildDtoValido([new ItemPedidoModel { ProdutoId = produtoId, Quantidade = 1, ValorUnitario = 10 }]);

        var result = await _createPedidoService.CreatePedidoAsync(dto);

        Assert.NotNull(result.Error);
        Assert.Contains("Não foi possível localizar o preço do produto", result.Error);
        Assert.Contains(produtoId.ToString(), result.Error);
    }

    [Fact]
    public async Task CreatePedidoAsync_DeveRetornarErro_QuandoEstoqueNaoEncontradoPorConfigDePedido()
    {
        var produtoId = Guid.NewGuid();
        _usuarioAutenticadoMock.Setup(x => x.GetUsuarioAutenticadoAsync()).ReturnsAsync(BuildUsuarioComTelefone());
        SetupFreteNaoCobra();
        SetupConfiguracaoDePedido(vendaDeProdutoComEstoque: true);
        _itemTabelaDePrecoRepositoryMock
            .Setup(x => x.GetItensTabelaDePrecoByIdProdutosAsync(It.IsAny<IList<Guid>>()))
            .ReturnsAsync([BuildItemTabelaDePreco(produtoId)]);
        _estoqueRepositoryMock
            .Setup(x => x.GetPosicaoEstoqueDosProdutosAsync(It.IsAny<IList<Guid>>()))
            .ReturnsAsync([]);

        var dto = BuildDtoValido([new ItemPedidoModel { ProdutoId = produtoId, Quantidade = 1, ValorUnitario = 10 }]);

        var result = await _createPedidoService.CreatePedidoAsync(dto);

        Assert.NotNull(result.Error);
        Assert.Contains("Não foi possível localizar o estoque do produto", result.Error);
        Assert.Contains(produtoId.ToString(), result.Error);
    }

    [Fact]
    public async Task CreatePedidoAsync_DeveRetornarErro_QuandoEstoqueNaoEncontradoPorConfigDoProduto()
    {
        var produtoId = Guid.NewGuid();
        _usuarioAutenticadoMock.Setup(x => x.GetUsuarioAutenticadoAsync()).ReturnsAsync(BuildUsuarioComTelefone());
        SetupFreteNaoCobra();
        SetupConfiguracaoDePedido(vendaDeProdutoComEstoque: false);
        _itemTabelaDePrecoRepositoryMock
            .Setup(x => x.GetItensTabelaDePrecoByIdProdutosAsync(It.IsAny<IList<Guid>>()))
            .ReturnsAsync([BuildItemTabelaDePreco(produtoId, vendaSomenteComEstoque: true)]);
        _estoqueRepositoryMock
            .Setup(x => x.GetPosicaoEstoqueDosProdutosAsync(It.IsAny<IList<Guid>>()))
            .ReturnsAsync([]);

        var dto = BuildDtoValido([new ItemPedidoModel { ProdutoId = produtoId, Quantidade = 1, ValorUnitario = 10 }]);

        var result = await _createPedidoService.CreatePedidoAsync(dto);

        Assert.NotNull(result.Error);
        Assert.Contains("Não foi possível localizar o estoque do produto", result.Error);
        Assert.Contains(produtoId.ToString(), result.Error);
    }

    [Fact]
    public async Task CreatePedidoAsync_DeveRetornarErro_QuandoQuantidadeEstoqueInsuficiente()
    {
        var produtoId = Guid.NewGuid();
        _usuarioAutenticadoMock.Setup(x => x.GetUsuarioAutenticadoAsync()).ReturnsAsync(BuildUsuarioComTelefone());
        SetupFreteNaoCobra();
        SetupConfiguracaoDePedido(vendaDeProdutoComEstoque: true);
        _itemTabelaDePrecoRepositoryMock
            .Setup(x => x.GetItensTabelaDePrecoByIdProdutosAsync(It.IsAny<IList<Guid>>()))
            .ReturnsAsync([BuildItemTabelaDePreco(produtoId)]);
        // estoque disponível = 2, pedido pede 10
        var estoque = Estoque.NovoEstoque(quantidade: 2, produtoId: produtoId, tamanhoId: null, pesoId: null);
        _estoqueRepositoryMock
            .Setup(x => x.GetPosicaoEstoqueDosProdutosAsync(It.IsAny<IList<Guid>>()))
            .ReturnsAsync([estoque]);

        var dto = BuildDtoValido([new ItemPedidoModel { ProdutoId = produtoId, Quantidade = 10, ValorUnitario = 10 }]);

        var result = await _createPedidoService.CreatePedidoAsync(dto);

        Assert.NotNull(result.Error);
        Assert.Contains("Não há estoque disponível do produto", result.Error);
    }
}