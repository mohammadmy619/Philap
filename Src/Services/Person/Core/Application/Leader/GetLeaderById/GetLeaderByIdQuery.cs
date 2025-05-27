using Application.Leader.GetLeaderById;
using MediatR;

public record GetLeaderByIdQuery(Guid Id) : IRequest<GetLeaderByIdResponse>;
