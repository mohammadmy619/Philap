using BuildingBlocks.Domain;

public class UserNotFoundException : DomainException
{
    public UserNotFoundException(string message = "User Not Found", string code = "0814023")
        : base(message, code) { }
}