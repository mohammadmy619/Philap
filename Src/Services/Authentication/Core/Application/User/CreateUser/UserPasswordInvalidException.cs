using BuildingBlocks.Domain;


namespace Application.User.CreateUser
{
    public class UserPasswordInvalidException : DomainException
    {
        public UserPasswordInvalidException(string message = "User Password must be a valid .", string code = "0814021")
            : base(message, code) { }
    }
}
