using OpenAdm.Application.Dtos;
using OpenAdm.Application.Models;

namespace OpenAdm.Application.Interfaces;

public interface IRelatorioVendaDeProdutoService
{
    Task<RelatorioVendaDeProdutoViewModel> ListarAsync(RelatorioVendaDeProdutoDTO relatorioVendaDeProdutoDto);
}