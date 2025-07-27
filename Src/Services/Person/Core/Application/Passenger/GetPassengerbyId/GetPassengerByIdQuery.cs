using MediatR;

public record class GetPassengerByIdQuery(Guid Id) : IRequest<GetPassengerByIdResponse>;
