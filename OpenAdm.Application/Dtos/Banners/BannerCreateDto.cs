﻿using Domain.Pkg.Entities;
using Domain.Pkg.Exceptions;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OpenAdm.Application.Dtos.Banners;

public class BannerCreateDto
{
    [Required(ErrorMessage = "Informe a imagem do banner!")]
    public string Foto { get; set; } = string.Empty;

    public Banner ToEntity()
    {
        var date = DateTime.Now;
        return new Banner(Guid.NewGuid(), date, date, 0, Foto, true);
    }
}
