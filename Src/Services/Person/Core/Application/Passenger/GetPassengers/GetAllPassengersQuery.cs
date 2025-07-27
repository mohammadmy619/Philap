using MediatR;

namespace Application.Passenger.GetPassengers
{
    public record class GetAllPassengersQuery(int PageNumber, int PageSize) : IRequest<IReadOnlyCollection<GetAllPassengersResponse>>;
}
