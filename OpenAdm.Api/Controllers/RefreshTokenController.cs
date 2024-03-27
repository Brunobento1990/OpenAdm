using Microsoft.AspNetCore.Mvc;
using OpenAdm.Application.Interfaces;

namespace OpenAdm.Api.Controllers;

[ApiController]
[Route("token")]
public class RefreshTokenController(IRefreshTokenService refreshTokenService) : ControllerBaseApi
{
    private readonly IRefreshTokenService _refreshTokenService = refreshTokenService;

    [HttpGet("refresh")]
    public async Task<IActionResult> RefreshToken()
    {
        var token = HttpContext.Request.Headers.Authorization.ToString().Replace("Bearer ", "");
        var refreshToken = HttpContext.Request.Headers["RefreshToken"].ToString();
        var newRefreshToken = await _refreshTokenService.RefreshTokenAsync(token, refreshToken);
        if (string.IsNullOrWhiteSpace(newRefreshToken))
        {
            return Ok();
        }

        return Ok(new { newRefreshToken });
    }
}