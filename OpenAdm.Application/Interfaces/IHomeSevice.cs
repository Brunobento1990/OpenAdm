using OpenAdm.Application.Models.Home;

namespace OpenAdm.Application.Interfaces;

public interface IHomeSevice
{
    Task<HomeECommerceViewModel> GetHomeEcommerceAsync();
}
