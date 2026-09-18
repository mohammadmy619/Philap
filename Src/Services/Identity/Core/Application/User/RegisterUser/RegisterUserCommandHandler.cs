using Domain.UserAgregate;
using MediatR;
using Domain.Services;
using Domain.RoleAgregate;

namespace Application.User.CreateUser
{
    public class RegisterUserCommandHandler(IUserRepository _userRepository, IRoleRepository _RoleRepository, IEmailService _EmailService) : IRequestHandler<RegisterUserCommand, RegisterUserResponse>
    {

        public async Task<RegisterUserResponse> Handle(
            RegisterUserCommand request,
            CancellationToken cancellationToken)
        {


            

          
            if (string.IsNullOrEmpty(request.Password)) throw new UserPasswordInvalidException();
            string? passwordHash = null;
            if (!string.IsNullOrWhiteSpace(request.Password))
            {
                passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
            }


            // Create User
            var user = new Domain.UserAgregate.User(
                userName: request.UserName,
                email: request.Email,
                passwordHash: passwordHash,
                _EmailService
                );
            //if (request.RoleId.Any()) 
            //{
            //    var roles = await _RoleRepository.GetRoleIdsAsync(request.RoleId, cancellationToken);
            //    user.AddRolesToUser(roles.ToList());

            //} 


            // Persist
            await _userRepository.AddUserAsync(user, cancellationToken);
            await _userRepository.SaveChangesAsync(cancellationToken);
            // Return Response
            return new RegisterUserResponse(
                UserId: user.Id,
                UserName: user.UserName,
                Email: user.Email,
                CreatedAt: DateTime.UtcNow,
                AssignedRoles: user?._RoleIds.ToList() ?? new List<Guid>());
        }
    }
}
