using Domain.UserAgregate;
using MediatR;
using Application.services;
using Application.User.Exceptions;
using Application.User.CreateUser;
using Domain.Services;
using Domain.RoleAgregate;

namespace Application.User.UpdateUser
{
    public class UpdateUserCommandHandler(IUserRepository _userRepository, IRoleRepository _RoleRepository, IPasswordHelper _passwordHasher,IEmailService _EmailService) : IRequestHandler<UpdateUserCommand, UpdateUserResponse>
    {
       

      

        public async Task<UpdateUserResponse> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            // Fetch existing user  
            var user = await _userRepository.GetUserByIdAsync(request.UserId, cancellationToken);
            if (user == null)
                throw new UserNotFoundException($"User with ID {request.UserId} not found.");


            if (!string.IsNullOrEmpty(request.Password)) _passwordHasher.EncodePasswordMd5(request.Password);

            user.UpdateUser(request.UserId,request.UserName,request.Email,request.Password, _EmailService);

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