using OpenAdm.Application.Interfaces;
using OpenAdm.Domain.Enuns;
using OpenAdm.Domain.Exceptions;

namespace OpenAdm.Application.Services;

public class PagamentoFactory : IPagamentoFactory
{
    private readonly IDictionary<MeioDePagamentoEnum, IPagamentoService> _pagamentos;

    public PagamentoFactory(PagamentoPix pagamentoPix)
    {
        _pagamentos = new Dictionary<MeioDePagamentoEnum, IPagamentoService>()
        {
            { MeioDePagamentoEnum.Pix, pagamentoPix }
        };
    }

    public IPagamentoService GetPagamento(MeioDePagamentoEnum meioDePagamentoEnum)
    {
        if (!_pagamentos.TryGetValue(meioDePagamentoEnum, out var pagamentoService))
        {
            throw new ExceptionApi("Não foi possivel acessar os meios de pagamentos.", enviarErroDiscord: true);
        }

        return pagamentoService;
    }
}
