using OpenAdm.Application.Dtos.Bases;
using OpenAdm.Domain.Entities.OpenAdm;
using OpenAdm.Domain.Enuns;

namespace OpenAdm.Application.Models.Parceiros;

public class RedeSocialViewModel : BaseViewModel
{
    public string Link { get; set; } = string.Empty;
    public RedeSocialEnum RedeSocialEnum { get; set; }

    public static explicit operator RedeSocialViewModel(RedeSocial redeSocial)
    {
        return new RedeSocialViewModel()
        {
            Id = redeSocial.Id,
            RedeSocialEnum = redeSocial.RedeSocialEnum,
            Link = redeSocial.Link,
            DataDeAtualizacao = redeSocial.DataDeAtualizacao,
            DataDeCriacao = redeSocial.DataDeCriacao,
            Numero = redeSocial.Numero
        };
    }
}
