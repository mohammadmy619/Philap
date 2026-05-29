using MediatR;


public record IsLeaderValidQuery(Guid LeaderId) : IRequest<bool>;


