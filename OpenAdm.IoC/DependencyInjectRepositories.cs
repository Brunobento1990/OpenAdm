﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Infra.Azure.Interfaces;
using OpenAdm.Infra.Azure.Storage;
using OpenAdm.Infra.Cached.Cached;
using OpenAdm.Infra.Cached.Interfaces;
using OpenAdm.Infra.Cached.Services;
using OpenAdm.Infra.Repositories;


namespace OpenAdm.IoC;

public static class DependencyInjectRepositories
{
    public static void InjectRepositories(this IServiceCollection services, string connectionString)
    {
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = connectionString;
        });
        services.AddTransient(typeof(ICachedService<>), typeof(CachedService<>));

        services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();

        services.AddScoped<IConfiguracaoParceiroRepository, ConfiguracaoParceiroRepository>();
        services.AddScoped<ILoginUsuarioRepository, LoginUsuarioRepository>();
        services.AddScoped<ICarrinhoRepository, CarrinhoRepository>();

        services.AddScoped<BannerRepository>();
        services.AddScoped<IBannerRepository, BannerCached>();

        services.AddScoped<ILoginFuncionarioRepository, LoginFuncionarioRepository>();
        services.AddScoped<IPedidoRepository, PedidoRepository>();

        services.AddScoped<CategoriaRepository>();
        services.AddScoped<ICategoriaRepository, CategoriaCached>();

        services.AddScoped<ProdutoRepository>();
        services.AddScoped<IProdutoRepository, ProdutoCached>();

        services.AddScoped<UsuarioRepository>();
        services.AddScoped<IUsuarioRepository, UsuarioCached>();

        services.AddScoped<ItensPedidoRepository>();
        services.AddScoped<IItensPedidoRepository, ItensPedidoCached>();

        services.AddScoped<ConfiguracaoDeEmailRepository>();
        services.AddScoped<IConfiguracaoDeEmailRepository, ConfiguracaoDeEmailCached>();

        services.AddScoped<ITabelaDePrecoRepository, TabelaDePrecoRepository>();

        services.AddScoped<ConfiguracoesDePedidoRepository>();
        services.AddScoped<IConfiguracoesDePedidoRepository, ConfiguracoesDePedidoCached>();

        services.AddScoped<PesoRepository>();
        services.AddScoped<IPesoRepository, PesoCached>();

        services.AddScoped<TamanhoRepository>();
        services.AddScoped<ITamanhoRepository, TamanhoCached>();

        services.AddScoped<IPesosProdutosRepository, PesosProdutosRepository>();
        services.AddScoped<ITamanhosProdutoRepository, TamanhosProdutoRepository>();

        services.AddScoped<ItemTabelaDePrecoRepository>();
        services.AddScoped<IItemTabelaDePrecoRepository, ItemTabelaDePrecoCached>();
        services.AddScoped<IUploadImageBlobClient, UploadImageBlobClient>();
        services.AddScoped<IEstoqueRepository, EstoqueRepository>();
        services.AddScoped<IMovimentacaoDeProdutoRepository, MovimentacaoDeProdutoRepository>();
        services.AddScoped<ILojasParceirasRepository, LojasParceirasRepository>();

        services.AddScoped<TopUsuariosRepository>();
        services.AddScoped<ITopUsuariosRepository, TopUsuariosCached>();
    }
}
