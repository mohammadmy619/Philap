using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using  Application.DTO;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;


namespace Application.Utils
{
    public class JwtTokenService(IConfiguration _configuration)
    {
        private string GenerateJwtToken(Application.DTO.User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["JwtSettings:Secret"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim("role", user.Role)
            }),
                Expires = DateTime.UtcNow.AddMinutes(double.Parse(_configuration["JwtSettings:ExpiryMinutes"])),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
