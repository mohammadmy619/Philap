using MediatR;

namespace Application.User.GetUser
{
    public record GetUserQuery() : IRequest<GetUserResponse>;
}