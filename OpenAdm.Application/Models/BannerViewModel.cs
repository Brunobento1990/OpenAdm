﻿using OpenAdm.Domain.Entities;
using System.Text;

namespace OpenAdm.Application.Models;

public class BannerViewModel : BaseModel
{
    public string Foto { get; set; } = string.Empty;
    public bool Ativo { get; set; }

    public BannerViewModel ToEntity(Banner entity)
    {
        Id = entity.Id;
        DataDeCriacao = entity.DataDeCriacao;
        DataDeAtualizacao = entity.DataDeAtualizacao;
        Numero = entity.Numero;
        Foto = Encoding.UTF8.GetString(entity.Foto);
        Ativo = entity.Ativo;

        return this;
    }
}
