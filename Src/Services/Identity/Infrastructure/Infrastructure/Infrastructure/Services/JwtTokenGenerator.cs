using Application.Commons.DTO;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class JwtTokenGenerator : IJwtService
{
    private readonly IConfiguration _configuration;

    public JwtTokenGenerator(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateToken(UserDTO user, IList<string> roles, IList<string> permissions)
    {
        // ۱. خواندن تنظیمات از appsettings.json
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var secretKey = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("Jwt SecretKey is missing in configuration.");
        var issuer = jwtSettings["Issuer"];
        var audience = jwtSettings["Audience"];
        var expiryMinutes = double.Parse(jwtSettings["ExpiryMinutes"] ?? "60");

        // ۲. ساخت کلید امنیتی و امضا
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // ۳. ساخت لیست Claimها با اطلاعات پایه کاربر
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), // جهت سازگاری با CurrentUserService
            new Claim(JwtRegisteredClaimNames.Name, user.Username ?? string.Empty),
            new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // شناسه یکتا برای هر توکن
        };

        // ۴. اضافه کردن نقش‌ها (Roles)
        if (roles != null && roles.Any())
        {
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
               
            }
        }

        // ۵. اضافه کردن دسترسی‌ها (Permissions)
        if (permissions != null && permissions.Any())
        {
            foreach (var permission in permissions)
            {
                claims.Add(new Claim("permission", permission));
            }
        }

        // ۶. ایجاد توکن JWT
        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
            signingCredentials: creds
        );

        // ۷. بازگرداندن رشته توکن
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}