using Application.ApplicationServices;
using Application.DTO;
using Application.User.LoginUser;
using AutoMapper;
using Domain.PermissionAgregate;
using Domain.RoleAgregate;
using Domain.UserAgregate;
using MediatR;


public class LoginUserQueryHandler(IUserRepository _userRepository, IRoleRepository _RoleRepository
    ,IPermissionRepository _PermissionRepository, IPasswordHelper _PasswordHelper, IJwtService _jwtService, IMapper _mapper) : IRequestHandler<LoginUserQuery, LoginUserResponse>
{
    public async Task<LoginUserResponse> Handle(LoginUserQuery request, CancellationToken cancellationToken)
    {
        // پیدا کردن کاربر با نام کاربری
        var user = await _userRepository.FindUserAsync(
            predicate: u => u.UserName == request.Username,
            cancellationToken: cancellationToken);

        if (user == null)
            throw new UserNotFoundException();

        // بررسی رمز عبور با هش
        if (!_PasswordHelper.VerifyPassword(user.PasswordHash, request.Password))
            throw new InvalidCredentialsException(); // یک اکسپشن سفارشی

        // دریافت نقش‌ها و مجوزها (Permissions)
        var roles = new List<Role>();
        if (user.RoleIds.Any())
        {

            foreach (var roleid in user.RoleIds)
            {

                roles.Add(await _RoleRepository.GetRoleByIdAsync(roleid, cancellationToken)); 
            }
        }

        // دریافت مجوزها (Permission + AccessControl)
        var permissionClaims = new List<string>(); // یا List<Claim>

        foreach (var role in roles)
        {
            // دریافت Permission‌هایی که این نقش را دارند
            var permissions = await _PermissionRepository.GetPermissionByIdAsync(role.PermissionIds, cancellationToken);

            foreach (var permission in permissions)
            {
                // دریافت AccessControl‌های هر Permission
                var accessControls = await _PermissionRepository.GetAccessControlsByPermissionIdAsync(permission.Id, cancellationToken);

                foreach (var ac in accessControls)
                {
                    // ساخت یک Claim معنا دار مثلاً "permission:User:Edit"
                    string claimValue = $"{ac.Resource}:{ac.Action}";
                    permissionClaims.Add(claimValue);
                }
            }
        }

        // حذف تکراری‌ها
        permissionClaims = permissionClaims.Distinct().ToList();

        var userDto = _mapper.Map<UserDTO>(user);
        // تولید توکن JWT با نقش‌ها و مجوزها
        var token = _jwtService.GenerateToken(userDto, roles.Select(r => r.Name).ToList(), permissionClaims);

        return new LoginUserResponse(token, user.UserName);
    }
}
