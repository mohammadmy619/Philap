using Application.DTO;

public interface IJwtService
{
    string GenerateToken(UserDTO user, IList<string> roles, IList<string> permissions);
}

