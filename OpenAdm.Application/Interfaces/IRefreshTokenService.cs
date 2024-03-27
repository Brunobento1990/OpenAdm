namespace OpenAdm.Application.Interfaces;

public interface IRefreshTokenService
{
    Task<string> RefreshTokenAsync(string token, string refreshToken);
}
