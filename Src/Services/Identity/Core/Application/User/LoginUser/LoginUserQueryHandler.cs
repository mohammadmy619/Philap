using System;
using Application.User.LoginUser;
using Domain.PermissionAgregate;
using Domain.RoleAgregate;
using Domain.UserAgregate;
using MediatR;

public class LoginUserQueryHandler(IUserRepository _userRepository, IRoleRepository _RoleRepository) : IRequestHandler<LoginUserQuery, LoginUserResponse>
{
    public async Task<LoginUserResponse> Handle(LoginUserQuery request, CancellationToken cancellationToken)
    {
        // TODO: Validate credentials (e.g., against a database or auth service)
        // For demo purposes, we'll just return a dummy token.
        // In real apps, you'd hash/verify password, generate JWT, etc.

        var user = await _userRepository.FindUserAsync(
             predicate: u => u.UserName == request.Username && u.PasswordHash==request.Password,
             cancellationToken: cancellationToken );

        if (user == null) { throw new UserNotFoundException(); }


        if (user.RoleIds.Any() && user.RoleIds.Count > 0)
        {
            var roles = await _RoleRepository.GetRoleIdsAsync(user.RoleIds, cancellationToken);
            user.AddRolesToUser(roles.ToList());

        }
        // If login fails, you might throw an exception or return a failure response.
        // For simplicity, we'll throw here — consider using Result/Validation patterns in production.
        throw new InvalidOperationException("Invalid username or password.");
    }
}