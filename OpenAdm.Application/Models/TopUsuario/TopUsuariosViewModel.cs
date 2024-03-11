using Domain.Pkg.Entities;

namespace OpenAdm.Application.Models.TopUsuario;

public class TopUsuariosViewModel : BaseModel
{
    public int TotalPedidos { get; set; }
    public decimal TotalCompra { get; set; }
    public string Usuario { get; set; } = string.Empty;

    public static explicit operator TopUsuariosViewModel(TopUsuarios topUsuarios)
    {
        return new TopUsuariosViewModel()
        {
            Id = topUsuarios.Id,
            DataDeAtualizacao = topUsuarios.DataDeAtualizacao,
            DataDeCriacao = topUsuarios.DataDeCriacao,
            Numero = topUsuarios.Numero,
            TotalCompra = topUsuarios.TotalCompra,
            TotalPedidos = topUsuarios.TotalPedidos,
            Usuario = topUsuarios.Usuario
        };
    }
}
