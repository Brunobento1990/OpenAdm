using Microsoft.EntityFrameworkCore;
using OpenAdm.Application.Interfaces.Ecommerce;
using OpenAdm.Application.Queries;
using OpenAdm.Data.Context;

namespace OpenAdm.Infra.QueryService;

public class BannerEcommerceService : IBannerEcommerceService
{
    private readonly AppDbContext _context;

    public BannerEcommerceService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ICollection<BannerEcommerceQuery>> ListarTodosAsync(Guid parceiroId)
    {
        return await _context
            .Banners
            .AsNoTracking()
            .Where(b => b.ParceiroId == parceiroId && b.Ativo)
            .Select(x => new BannerEcommerceQuery()
            {
                Foto = x.Foto,
            }).ToListAsync();
    }
}