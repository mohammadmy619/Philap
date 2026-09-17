using Domain.UserAgregate;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Application.User.Exceptions;

namespace Application.User.GetUser
{
    public class GetUserQueryHandler(IUserRepository _userRepository, ICurrentUserService _currentUserService) : IRequestHandler<GetUserQuery, GetUserResponse>
    {
       
        public async Task<GetUserResponse> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {


            var userId = _currentUserService.UserId
          ?? throw new UnauthorizedAccessException("کاربر معتبر نیست.");



            // دریافت کاربر از repo  
            var user = await _userRepository.GetUserByIdAsync(userId, cancellationToken);

            if (user == null)
            {
                throw new UserNotFoundException(); 
            }

            // ایجاد پاسخ  
            var response = new GetUserResponse(
                UserId: user.Id,
                UserName: user.UserName,
                Email: user.Email,
                CreatedAt: DateTime.UtcNow, 
                AssignedRoles: user._RoleIds.ToList() ?? new List<Guid>()
            );

            return response;
        }
    }
}