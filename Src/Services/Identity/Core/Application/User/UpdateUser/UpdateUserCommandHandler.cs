using Domain.UserAgregate;
using MediatR;
using Application.User.Exceptions;
using Application.User.CreateUser;
using Domain.Services;
using Domain.RoleAgregate;

namespace Application.User.UpdateUser
{
    public class UpdateUserCommandHandler(IUserRepository _userRepository, IRoleRepository _RoleRepository, IEmailService _EmailService, ICurrentUserService _currentUserService) : IRequestHandler<UpdateUserCommand, UpdateUserResponse>
    {
       

      

        public async Task<UpdateUserResponse> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId
           ?? throw new UnauthorizedAccessException("کاربر معتبر نیست.");

            // Fetch existing user  
            var user = await _userRepository.GetUserByIdAsync(userId, cancellationToken);
            if (user == null)
                throw new UserNotFoundException($"User with ID {userId} not found.");


            string? passwordHash = null;
            if (!string.IsNullOrWhiteSpace(request.Password))
            {
                passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
            }
            if (string.IsNullOrEmpty(request.Password)) throw new UserPasswordInvalidException();
            user.UpdateUser(userId, request.UserName,request.Email, passwordHash, _EmailService);

            if (request.RoleId.Any()) {

                var roles = await _RoleRepository.GetRoleIdsAsync(request.RoleId, cancellationToken);
                user.AddRolesToUser(roles.ToList());

            } 



            await _userRepository.UpdateUserAsync(user, cancellationToken);

            await _userRepository.SaveChangesAsync(cancellationToken);

            // Return response  
            return new UpdateUserResponse(user.Id, user.UserName, user.Email);
        }
    }
}