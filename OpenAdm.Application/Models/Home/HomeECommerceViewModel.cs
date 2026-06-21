using OpenAdm.Domain.Model;

namespace OpenAdm.Application.Models.Home;

public class HomeECommerceViewModel
{
    public IEnumerable<LojaParceiraModel> LojasParceiras { get; set; } = [];
}