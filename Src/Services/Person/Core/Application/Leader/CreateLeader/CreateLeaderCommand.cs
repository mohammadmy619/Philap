using Domain.Persons;
using MediatR;

public record class CreateLeaderCommand(List<Guid>? TripIds, DateTime DateOfBirth, Gender Gender, DateTime JoiningDate, List<string> Skills) : IRequest<CreateLeaderResponse>
{
    public string Name { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Nationality { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public string Bio { get; set; } = string.Empty;

    #region address
    public string Street { get; private set; }
    public string City { get; private set; }
    public string State { get; private set; }
    public string ZipCode { get; private set; }
    #endregion

}
