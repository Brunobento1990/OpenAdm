using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Infra.Cached.Services;

namespace OpenAdm.Test.Infra.Test;

public class CachedServiceTest
{
    private readonly Mock<IDistributedCache> _distributedCache = new();
    private readonly Mock<IParceiroAutenticado> _parceiroAutenticado = new();
    private readonly Mock<IConfiguration> _configuration = new();

    public CachedServiceTest()
    {
        _parceiroAutenticado.SetupGet(x => x.Id).Returns(Guid.NewGuid());
    }

    [Fact]
    public async Task SetItemAsync_DeveUsarExpiracaoConfiguradaPorPadrao()
    {
        _configuration.SetupGet(x => x["Cache:ExpiracaoAbsolutaMinutos"]).Returns("20");
        _configuration.SetupGet(x => x["Cache:ExpiracaoDeslizanteMinutos"]).Returns("8");
        DistributedCacheEntryOptions? optionsUtilizadas = null;
        ConfigurarCapturaDasOptions(options => optionsUtilizadas = options);

        var service = CriarService();
        await service.SetItemAsync("chave", new ItemCache { Valor = "item" });

        Assert.NotNull(optionsUtilizadas);
        Assert.Equal(TimeSpan.FromMinutes(20), optionsUtilizadas.AbsoluteExpirationRelativeToNow);
        Assert.Equal(TimeSpan.FromMinutes(8), optionsUtilizadas.SlidingExpiration);
    }

    [Fact]
    public async Task SetItemAsync_DeveUsarExpiracaoPersonalizadaQuandoInformada()
    {
        _configuration.SetupGet(x => x["Cache:ExpiracaoAbsolutaMinutos"]).Returns("5");
        _configuration.SetupGet(x => x["Cache:ExpiracaoDeslizanteMinutos"]).Returns("3");
        DistributedCacheEntryOptions? optionsUtilizadas = null;
        ConfigurarCapturaDasOptions(options => optionsUtilizadas = options);

        var service = CriarService();
        await service.SetItemAsync("chave", new ItemCache(), TimeSpan.FromHours(2));

        Assert.NotNull(optionsUtilizadas);
        Assert.Equal(TimeSpan.FromHours(2), optionsUtilizadas.AbsoluteExpirationRelativeToNow);
        Assert.Equal(TimeSpan.FromMinutes(3), optionsUtilizadas.SlidingExpiration);
    }

    [Fact]
    public async Task SetItemAsync_DeveUsarExpiracaoDeslizantePersonalizadaQuandoInformada()
    {
        _configuration.SetupGet(x => x["Cache:ExpiracaoAbsolutaMinutos"]).Returns("5");
        _configuration.SetupGet(x => x["Cache:ExpiracaoDeslizanteMinutos"]).Returns("3");
        DistributedCacheEntryOptions? optionsUtilizadas = null;
        ConfigurarCapturaDasOptions(options => optionsUtilizadas = options);

        var service = CriarService();
        await service.SetItemAsync(
            "chave",
            new ItemCache(),
            TimeSpan.FromHours(6),
            TimeSpan.FromMinutes(30));

        Assert.NotNull(optionsUtilizadas);
        Assert.Equal(TimeSpan.FromHours(6), optionsUtilizadas.AbsoluteExpirationRelativeToNow);
        Assert.Equal(TimeSpan.FromMinutes(30), optionsUtilizadas.SlidingExpiration);
    }

    private CachedService<ItemCache> CriarService() => new(
        _distributedCache.Object,
        _parceiroAutenticado.Object,
        _configuration.Object);

    private void ConfigurarCapturaDasOptions(Action<DistributedCacheEntryOptions> capture)
    {
        _distributedCache
            .Setup(x => x.SetAsync(
                It.IsAny<string>(),
                It.IsAny<byte[]>(),
                It.IsAny<DistributedCacheEntryOptions>(),
                It.IsAny<CancellationToken>()))
            .Callback<string, byte[], DistributedCacheEntryOptions, CancellationToken>(
                (_, _, options, _) => capture(options))
            .Returns(Task.CompletedTask);
    }

    public sealed class ItemCache
    {
        public string Valor { get; set; } = string.Empty;
    }
}
