
using OpenAdm.Domain.Errors;
using OpenAdm.Domain.Validations;

namespace OpenAdm.Domain.Entities;

public sealed class Parceiro : BaseEntity
{
    public Parceiro(
        Guid id,
        DateTime dataDeCriacao,
        DateTime dataDeAtualizacao,
        long numero,
        string razaoSocial,
        string nomeFantasia,
        string cnpj)
        : base(id, dataDeCriacao, dataDeAtualizacao, numero)
    {
        ValidationString.ValidateWithLength(cnpj, length: 14, message: CodigoErrors.ErrorCnpj);
        ValidationString.ValidateWithLength(razaoSocial, message: CodigoErrors.ErrorRazaoSocial);
        ValidationString.ValidateWithLength(nomeFantasia, message: CodigoErrors.ErrorRazaoSocial);

        RazaoSocial = razaoSocial;
        NomeFantasia = nomeFantasia;
        Cnpj = cnpj;
    }

    public string RazaoSocial { get; private set; }
    public string NomeFantasia { get; private set; }
    public string Cnpj { get; private set; }
    public ConfiguracaoParceiro ConfiguracaoDbParceiro { get; set; } = null!;
}
