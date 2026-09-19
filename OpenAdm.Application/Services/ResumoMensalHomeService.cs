using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.Home;
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Application.Services;

public sealed class ResumoMensalHomeService : IResumoMensalHomeService
{
    private readonly IResumoMensalPedidoRepository _repository;

    public ResumoMensalHomeService(IResumoMensalPedidoRepository repository)
    {
        _repository = repository;
    }

    public async Task<ResumoMensalHomeViewModel> ObterAsync()
    {
        var agora = DateTime.UtcNow;
        var inicioAtual = new DateTime(agora.Year, agora.Month, 1, 0, 0, 0, DateTimeKind.Utc);
        var fimAtual = inicioAtual.AddMonths(1);
        var inicioAnterior = inicioAtual.AddYears(-1);
        var fimAnterior = fimAtual.AddYears(-1);

        var atual = await _repository.ObterAsync(inicioAtual, fimAtual);
        var anterior = await _repository.ObterAsync(inicioAnterior, fimAnterior);
        var categoriasAtuais = atual.Categorias.ToDictionary(x => x.CategoriaId);
        var categoriasAnteriores = anterior.Categorias.ToDictionary(x => x.CategoriaId);

        var categorias = categoriasAtuais.Keys
            .Union(categoriasAnteriores.Keys)
            .Select(id =>
            {
                categoriasAtuais.TryGetValue(id, out var categoriaAtual);
                categoriasAnteriores.TryGetValue(id, out var categoriaAnterior);

                return new ResumoMensalCategoriaViewModel
                {
                    CategoriaId = id,
                    Categoria = categoriaAtual?.Categoria ?? categoriaAnterior!.Categoria,
                    QuantidadeItensVendidos = CriarVariacao(
                        categoriaAtual?.QuantidadeItensVendidos ?? 0,
                        categoriaAnterior?.QuantidadeItensVendidos ?? 0)
                };
            })
            .OrderByDescending(x => x.QuantidadeItensVendidos.Atual)
            .ThenBy(x => x.Categoria)
            .ToList();

        return new ResumoMensalHomeViewModel
        {
            Mes = inicioAtual.Month,
            AnoAtual = inicioAtual.Year,
            AnoAnterior = inicioAnterior.Year,
            QuantidadePedidos = CriarVariacao(atual.QuantidadePedidos, anterior.QuantidadePedidos),
            ValorTotalVendido = CriarVariacao(atual.ValorTotalVendido, anterior.ValorTotalVendido),
            QuantidadeItensVendidos = CriarVariacao(
                atual.QuantidadeItensVendidos,
                anterior.QuantidadeItensVendidos),
            Categorias = categorias
        };
    }

    private static VariacaoMensalViewModel<int> CriarVariacao(int atual, int anterior) => new()
    {
        Atual = atual,
        AnoAnterior = anterior,
        Variacao = atual - anterior,
        VariacaoPercentual = CalcularPercentual(atual, anterior)
    };

    private static VariacaoMensalViewModel<decimal> CriarVariacao(decimal atual, decimal anterior) => new()
    {
        Atual = atual,
        AnoAnterior = anterior,
        Variacao = atual - anterior,
        VariacaoPercentual = CalcularPercentual(atual, anterior)
    };

    private static decimal CalcularPercentual(decimal atual, decimal anterior)
    {
        if (anterior == 0)
        {
            return atual == 0 ? 0 : 100;
        }

        return Math.Round((atual - anterior) * 100 / anterior, 2);
    }
}
