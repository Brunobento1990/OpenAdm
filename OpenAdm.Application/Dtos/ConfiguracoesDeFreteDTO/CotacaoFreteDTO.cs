using OpenAdm.Application.Attributes;

namespace OpenAdm.Application.Dtos.ConfiguracoesDeFreteDTO;

public class CotacaoFreteDTO : ValidarBaseDTO
{
    [ValidaString(erro: "Informe o CEP", erroMaxLength: "O CEP deve conter no máximo 8 caracteres", maxLength: 8)]
    public string Cep { get; set; } = string.Empty;

    public ICollection<ProdutoCotacaoFreteDTO> Produtos { get; set; } = [];

    public override string? Validar()
    {
        var erro = base.Validar();

        if (Produtos.Count == 0)
        {
            return $"Informe os produtos {(string.IsNullOrWhiteSpace(erro) ? "" : $", {erro}")}";
        }

        return erro;
    }
}

public class ProdutoCotacaoFreteDTO
{
    public Guid ProdutoId { get; set; }
    public Guid? PesoId { get; set; }
    public Guid? TamanhoId { get; set; }
    public decimal Quantidade { get; set; }
}