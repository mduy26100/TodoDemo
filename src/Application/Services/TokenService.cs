using Application.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Application.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateRefreshToken()
        {
            var randomBytes = new byte[32];
            using (var rgn = RandomNumberGenerator.Create())
            {
                rgn.GetBytes(randomBytes);
            }
            return Convert.ToBase64String(randomBytes);
        }

        public string GenerateToken(List<Claim> authClaims)
        {
            var secretKey = _configuration["JWT:Secret"];
            if (string.IsNullOrEmpty(secretKey))
            {
                throw new Exception("JWT Secret is not configured properly.");
            }

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(authClaims),
                Issuer = _configuration["JWT:ValidIssuer"],
                Audience = _configuration["JWT:ValidAudience"],
                Expires = DateTime.UtcNow.AddMinutes(3),
                IssuedAt = DateTime.UtcNow,
                NotBefore = DateTime.UtcNow, // Token chỉ có hiệu lực ngay sau khi tạo
                SigningCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
