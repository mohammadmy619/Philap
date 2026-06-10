namespace Application.DTO;
public class UserDTO
{
    public Guid Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; } // در محیط واقعی هش شده ذخیره کنی
    public string Email { get; set; }
    public string Role { get; set; }
}
