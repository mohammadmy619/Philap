using Domain.Persons;
using MediatR;

public record class CreateLeaderCommand(List<Guid>? TripIds, DateTime DateOfBirth, Gender Gender, DateTime JoiningDate, List<string> Skills) : IRequest<CreateLeaderResponse>
{
    public string Name { get; set; } 
    public string LastName { get; set; } 
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Nationality { get; set; } 
    public string Title { get; set; } 
    public string Department { get; set; } 
    public string Bio { get; set; }
    public bool isActive { get; set; }

    #region address
    public string Street { get;  set; }
    public string City { get;  set; }
    public string State { get;  set; }
    public string ZipCode { get;  set; }
    #endregion

}
