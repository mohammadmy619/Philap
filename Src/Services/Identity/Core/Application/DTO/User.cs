namespace Application.DTO;
public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; } // در محیط واقعی هش شده ذخیره کنید
    public string Role { get; set; }
}
