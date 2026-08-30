using Microsoft.AspNetCore.Mvc;
using OpenAdm.Api.Attributes;
using OpenAdm.Application.Dtos.Tamanhos;
using OpenAdm.Application.Interfaces;
using OpenAdm.Infra.Paginacao;

namespace OpenAdm.Api.Controllers;

[ApiController]
[Route("tamanhos")]
[AcessoParceiro]
public class TamanhoController : ControllerBase
{
    private readonly ITamanhoService _tamanhoService;

    public TamanhoController(ITamanhoService tamanhoService)
    {
        _tamanhoService = tamanhoService;
    }

    [HttpGet("list")]
    public async Task<IActionResult> List()
    {
        var tamanhosViewModel = await _tamanhoService.GetTamanhoViewModelsAsync();
        return Ok(tamanhosViewModel);
    }

    [Autentica]
    [IsFuncionario]
    [HttpPost("paginacao")]
    public async Task<IActionResult> Paginacao(PaginacaoTamanhoDto paginacaoTamanhoDto)
    {
        var paginacao = await _tamanhoService.GetPaginacaoAsync(paginacaoTamanhoDto);
        return Ok(paginacao);
    }

    [Autentica]
    [IsFuncionario]
    [HttpGet("get-tamanho")]
    public async Task<IActionResult> GetById([FromQuery] Guid id)
    {
        var tamanhoViewModel = await _tamanhoService.GetTamanhoViewModelAsync(id);
        return Ok(tamanhoViewModel);
    }

    [Autentica]
    [IsFuncionario]
    [HttpPut("ativar/{id}/{ativo}")]
    public async Task<IActionResult> InativarAtivar(Guid id, bool ativo)
    {
        await _tamanhoService.InativarAtivarAsync(id, ativo);
        return Ok();
    }

    [Autentica]
    [IsFuncionario]
    [HttpPut("update")]
    public async Task<IActionResult> UpdateTamanho(UpdateTamanhoDto updateTamanhoDto)
    {
        var tamanhoViewlModel = await _tamanhoService.UpdateTamanhoAsync(updateTamanhoDto);
        return Ok(tamanhoViewlModel);
    }

    [Autentica]
    [IsFuncionario]
    [HttpPost("create")]
    public async Task<IActionResult> CreateTamanho(CreateTamanhoDto createTamanhoDto)
    {
        var tamanhoViewModel = await _tamanhoService.CreateTamanhoAsync(createTamanhoDto);
        return Ok(tamanhoViewModel);
    }
}
