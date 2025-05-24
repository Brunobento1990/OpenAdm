using OpenAdm.Domain.Entities.Bases;
using OpenAdm.Domain.Enuns;

namespace OpenAdm.Domain.Entities.OpenAdm;

public sealed class RedeSocial : BaseEntity
{
    public RedeSocial(
        Guid id,
        DateTime dataDeCriacao,
        DateTime dataDeAtualizacao,
        long numero,
        string link,
        RedeSocialEnum redeSocialEnum,
        Guid parceiroId)
            : base(id, dataDeCriacao, dataDeAtualizacao, numero)
    {
        Link = link;
        RedeSocialEnum = redeSocialEnum;
        ParceiroId = parceiroId;
    }

    public string Link { get; private set; }
    public RedeSocialEnum RedeSocialEnum { get; private set; }
    public Guid ParceiroId { get; private set; }
    public Parceiro Parceiro { get; set; } = null!;

    public void Editar(string link, RedeSocialEnum redeSocialEnum)
    {
        Link = link;
        RedeSocialEnum = redeSocialEnum;
    }

    public static RedeSocial NovaRedeSocial(string link, RedeSocialEnum redeSocialEnum, Guid parceiroId)
    {
        return new RedeSocial(
            id: Guid.NewGuid(),
            dataDeCriacao: DateTime.Now,
            dataDeAtualizacao: DateTime.Now,
            numero: 0,
            link: link,
            redeSocialEnum: redeSocialEnum,
            parceiroId: parceiroId);
    }
}
