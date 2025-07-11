﻿using OpenAdm.Domain.Entities;

namespace OpenAdm.Domain.Interfaces;

public interface IUsuarioAutenticado
{
    Guid Id { get; set; }
    Guid ParceiroId { get; set; }
    bool IsFuncionario { get; set; }
    Task<Usuario> GetUsuarioAutenticadoAsync();
    Task<Usuario?> GetUsuarioAutenticadoOrNullAsync();
}
