using System.Reflection;
using OpenAdm.Application.Attributes;

namespace OpenAdm.Application.Dtos;

public abstract class ValidarBaseDTO
{
    public virtual string? Validar()
    {
        ICollection<string> erros = [];

        Validar(erros);

        if (erros.Count > 0)
        {
            return string.Join(", ", erros);
        }

        return null;
    }

    private void Validar(ICollection<string> erros)
    {
        var propertiesString = GetType().GetProperties();

        foreach (var property in propertiesString)
        {
            var atributos = property.GetCustomAttributes<ValidaBaseAttribute>();

            var valor = property.GetValue(this);

            foreach (var atributo in atributos)
            {
                var erro = atributo.Validar(valor);

                if (!string.IsNullOrWhiteSpace(erro))
                {
                    erros.Add(erro);
                }
            }
        }
    }
}