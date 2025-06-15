using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Exceptions;
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Infra.Model;

public class ParceiroAutenticado : IParceiroAutenticado
{
    private readonly IParceiroRepository _parceiroRepository;

    public ParceiroAutenticado(IParceiroRepository parceiroRepository)
    {
        _parceiroRepository = parceiroRepository;
    }
    private Parceiro? _parceiro;
    public Guid Id { get; set; }
    public string ConnectionString { get; set; } = string.Empty;

    public async Task<Parceiro> ObterParceiroAutenticadoAsync()
    {
        _parceiro ??= await _parceiroRepository.ObterPorEmpresaOpenAdmIdAsync(Id)
            ?? throw new ExceptionApi("Não foi possível localizar o cadastro da empresa de acesso, tente novamente");

        return _parceiro;
    }
}
