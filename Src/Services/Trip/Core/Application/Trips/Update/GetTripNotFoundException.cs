using BuildingBlocks.Domain;

public class GetTripNotFoundException :DomainException
{
    public GetTripNotFoundException()
        : base("Trip not found.")
    {
    }
}
