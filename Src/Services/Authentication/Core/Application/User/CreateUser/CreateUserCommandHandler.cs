﻿using Domain.UserAgregate;
using MediatR;
using Application.services;
using Domain.Domain_Services;

namespace Application.User.CreateUser
{
    public class CreateUserCommandHandler(IUserRepository _userRepository, IRoleValidationService _RoleValidationService, PasswordHelper _passwordHasher) : IRequestHandler<CreateUserCommand, CreateUserResponse>
    {

        public async Task<CreateUserResponse> Handle(
            CreateUserCommand request,
            CancellationToken cancellationToken)
        {


            if (string.IsNullOrEmpty(request.Password)) throw new UserPasswordInvalidException();

            var passwordHash = _passwordHasher.EncodePasswordMd5(request.Password);


            // Create User
            var user = new Domain.UserAgregate.User(
                userName: request.UserName,
                email: request.Email,
                passwordHash: passwordHash
                );
            await _RoleValidationService.ValidateRoleIdsAsync(request.RoleId, user,cancellationToken);


            // Persist
            await _userRepository.AddUserAsync(user, cancellationToken);
            await _userRepository.SaveChangesAsync(cancellationToken);
            // Return Response
            return new CreateUserResponse(
                UserId: user.Id,
                UserName: user.UserName,
                Email: user.Email,
                CreatedAt: DateTime.UtcNow,
                AssignedRoles: user.Roleds?.ToList() ?? new List<Guid>());
        }
    }
}
