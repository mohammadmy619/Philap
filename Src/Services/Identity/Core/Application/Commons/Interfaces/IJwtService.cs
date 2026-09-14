using Application.Commons.DTO;
using Application.Commons.Interfaces;

public interface IJwtService
{
    string GenerateToken(UserDTO user, IList<string> roles, IList<string> permissions);
}

