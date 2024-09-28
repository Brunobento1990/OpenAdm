using OpenAdm.Domain.Entities;

namespace OpenAdm.Application.Models.TopUsuarios;

public class TopUsuariosViewModel : BaseModel
{
    public int TotalPedidos { get; set; }
    public decimal TotalCompra { get; set; }
    public string Usuario { get; set; } = string.Empty;

    public static explicit operator TopUsuariosViewModel(TopUsuario topUsuarios)
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
