using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Entities;

namespace OpenAdm.Infra.Context;

public class ParceiroContext(DbContextOptions options) 
    : DbContext(options)
{
    public DbSet<Banner> Banners { get; set; }
}
