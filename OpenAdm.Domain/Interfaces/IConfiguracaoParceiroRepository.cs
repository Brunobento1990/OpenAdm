﻿using OpenAdm.Domain.Entities;

namespace OpenAdm.Domain.Interfaces;

public interface IConfiguracaoParceiroRepository
{
    Task<ConfiguracaoParceiro?> GetParceiroByDominioAdmAsync(string dominio);
    Task<ConfiguracaoParceiro?> GetParceiroAutenticadoAdmAsync();
    Task<ConfiguracaoParceiro?> GetParceiroAdmByMercadoPagoAsync(string cliente);
}
