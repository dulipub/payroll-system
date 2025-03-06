using System.IdentityModel.Tokens.Jwt;

namespace PayrollSystem.ApiService.Core;

public static class HttpRequestExtensions
{
    public static string GetClaim(this HttpRequest request, string claimType)
    {
        var authHeader = request.Headers["Authorization"].FirstOrDefault();
        var token = authHeader?.Substring("Bearer ".Length).Trim();
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        var claim = jwtToken.Claims.FirstOrDefault(c => c.Type == claimType);
        return claim?.Value ?? string.Empty;
    }
}
