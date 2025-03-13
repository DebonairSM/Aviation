using EnterpriseApiIntegration.Domain.Entities.Auth;

namespace EnterpriseApiIntegration.Application.Interfaces;

public interface IAuthService
{
    Task<(string accessToken, string refreshToken)> AuthenticateAsync(string email, string password);
    Task<(string accessToken, string refreshToken)> RefreshTokenAsync(string refreshToken);
    Task RevokeTokenAsync(string refreshToken, string ipAddress);
    Task<bool> ValidateTokenAsync(string token);
    Task<User?> GetUserByEmailAsync(string email);
} 