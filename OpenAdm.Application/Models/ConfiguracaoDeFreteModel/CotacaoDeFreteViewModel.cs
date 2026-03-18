using OpenAdm.Application.HttpClient.Response;

namespace OpenAdm.Application.Models.ConfiguracaoDeFreteModel;

public class CotacaoDeFreteViewModel
{
    public IEnumerable<ItemCotacaoDeFreteViewModel> Itens { get; set; } = [];
}

public class ItemCotacaoDeFreteViewModel
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public decimal Preco { get; set; }
    public int? FaixaDeEntregaMin { get; set; }
    public int? FaixaDeEntregaMaxima { get; set; }
    public EmpresaCotacaoDeFreteViewModel Empresa { get; set; } = null!;

    public static explicit operator ItemCotacaoDeFreteViewModel(CotacaoFreteResponse cotacaoFreteResponse)
    {
        return new()
        {
            Id = cotacaoFreteResponse.Id,
            Empresa = new EmpresaCotacaoDeFreteViewModel()
            {
                Id = cotacaoFreteResponse.Company.Id,
                Nome = cotacaoFreteResponse.Company.Name,
                Logo = cotacaoFreteResponse.Company.Picture
            },
            FaixaDeEntregaMaxima = cotacaoFreteResponse.DeliveryRange?.Max,
            FaixaDeEntregaMin = cotacaoFreteResponse.DeliveryRange?.Min,
            Nome = cotacaoFreteResponse.Name,
            Preco = cotacaoFreteResponse.Price
        };
    }
}

public class EmpresaCotacaoDeFreteViewModel
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Logo { get; set; } = string.Empty;
}