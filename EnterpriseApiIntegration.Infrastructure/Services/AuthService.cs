using System.Security.Cryptography;
using EnterpriseApiIntegration.Domain.Entities.Auth;
using EnterpriseApiIntegration.Domain.Settings;
using EnterpriseApiIntegration.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace EnterpriseApiIntegration.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _context;
    private readonly JwtTokenService _jwtTokenService;
    private readonly JwtSettings _jwtSettings;
    private readonly ILogger<AuthService> _logger;

    public AuthService(
        ApplicationDbContext context,
        JwtTokenService jwtTokenService,
        IOptions<JwtSettings> jwtSettings,
        ILogger<AuthService> logger)
    {
        _context = context;
        _jwtTokenService = jwtTokenService;
        _jwtSettings = jwtSettings.Value;
        _logger = logger;
    }

    public async Task<(string accessToken, string refreshToken)> AuthenticateAsync(string email, string password)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user == null || !VerifyPassword(password, user.PasswordHash))
        {
            throw new InvalidOperationException("Invalid email or password");
        }

        if (!user.IsActive)
        {
            throw new InvalidOperationException("User account is inactive");
        }

        user.LastLoginDate = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        var accessToken = _jwtTokenService.GenerateAccessToken(user);
        var refreshToken = await GenerateRefreshTokenAsync(user.Id, "127.0.0.1"); // Replace with actual IP

        return (accessToken, refreshToken);
    }

    public async Task<(string accessToken, string refreshToken)> RefreshTokenAsync(string refreshToken)
    {
        var token = await _context.RefreshTokens
            .FirstOrDefaultAsync(t => t.Token == refreshToken && t.RevokedDate == null);

        if (token == null)
        {
            throw new InvalidOperationException("Invalid refresh token");
        }

        if (token.ExpiryDate < DateTime.UtcNow)
        {
            throw new InvalidOperationException("Refresh token has expired");
        }

        var user = await _context.Users.FindAsync(token.UserId);
        if (user == null || !user.IsActive)
        {
            throw new InvalidOperationException("User not found or inactive");
        }

        // Revoke the old refresh token
        token.RevokedDate = DateTime.UtcNow;
        token.ReplacedByToken = await GenerateReshTokenAsync(user.Id, "127.0.0.1"); // Replace with actual IP

        await _context.SaveChangesAsync();

        var accessToken = _jwtTokenService.GenerateAccessToken(user);
        return (accessToken, token.ReplacedByToken!);
    }

    public async Task RevokeTokenAsync(string refreshToken, string ipAddress)
    {
        var token = await _context.RefreshTokens
            .FirstOrDefaultAsync(t => t.Token == refreshToken && t.RevokedDate == null);

        if (token == null)
        {
            throw new InvalidOperationException("Invalid refresh token");
        }

        token.RevokedDate = DateTime.UtcNow;
        token.RevokedByIp = ipAddress;
        token.ReasonRevoked = "Revoked by user";

        await _context.SaveChangesAsync();
    }

    public async Task<bool> ValidateTokenAsync(string token)
    {
        return _jwtTokenService.ValidateToken(token);
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    private async Task<string> GenerateRefreshTokenAsync(string userId, string ipAddress)
    {
        var refreshToken = new RefreshToken
        {
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
            UserId = userId,
            ExpiryDate = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays),
            CreatedDate = DateTime.UtcNow,
            CreatedByIp = ipAddress
        };

        _context.RefreshTokens.Add(refreshToken);
        await _context.SaveChangesAsync();

        return refreshToken.Token;
    }

    private static bool VerifyPassword(string password, string passwordHash)
    {
        // In a real application, use proper password hashing like BCrypt
        // This is just for demo purposes
        return password == passwordHash;
    }
} 