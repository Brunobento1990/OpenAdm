using OpenAdm.Application.Models.Categorias;

namespace OpenAdm.Application.Interfaces;

public interface ICategoriaService
{
    Task<IList<CategoriaViewModel>> GetCategoriasAsync();
}
