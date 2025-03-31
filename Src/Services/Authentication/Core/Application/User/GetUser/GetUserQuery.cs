using MediatR;

namespace Application.User.GetUser
{
    public record GetUserQuery(Guid UserId) : IRequest<GetUserResponse>;
}