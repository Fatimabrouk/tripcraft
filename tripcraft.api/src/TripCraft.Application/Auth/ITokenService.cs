namespace TripCraft.Application.Auth;

public interface ITokenService
{
    string GenerateAccessToken(string userId, string email);
    string GenerateRefreshToken();
}