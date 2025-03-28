using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JWTWithCustomMiddleware.Models;

namespace JWTWithCustomMiddleware.Authentication
{
    public interface IJWTToken
    {
        Task<string> GenerateJWT(UserModel user);
        Task<List<Claim>> ValidateJWT(string token);
    }

    public class JWTToken : IJWTToken
    {
        public JWTSettings _setting;
        public JWTToken(IOptions<JWTSettings> setting)
        {
            _setting = setting.Value;
        }

        public async Task<string> GenerateJWT(UserModel user)
        {
            var key = Encoding.UTF8.GetBytes(_setting.Key);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, user.Role) // Add roles if needed
            };

            return new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken(
                issuer: _setting.Issuer,
                audience: _setting.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(_setting.ExpiryMinutes)),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            ));
        }

        public async Task<List<Claim>> ValidateJWT(string token)
        {
            var key = Encoding.UTF8.GetBytes(_setting.Key); // Get key from appsettings.json
            var tokenHandler = new JwtSecurityTokenHandler();

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = _setting.Issuer,
                ValidateAudience = true,
                ValidAudience = _setting.Audience,
                ValidateLifetime = true, // Checks for expiration
                ClockSkew = TimeSpan.Zero // No leeway for expiration time
            };

            tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);

            var tokenJWT = (JwtSecurityToken)validatedToken;

            return tokenJWT.Claims.ToList();
        }
    }
}
