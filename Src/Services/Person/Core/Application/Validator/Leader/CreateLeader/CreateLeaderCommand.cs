using Domain.Persons;
using MediatR;

public record CreateLeaderCommand(List<Guid>? TripIds, DateTime DateOfBirth, Gender Gender, Address Address, DateTime JoiningDate, List<string> Skills) : IRequest<CreateLeaderResponse>, ICreateLeaderCommand
{
    public string Name { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Nationality { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public string Bio { get; set; } = string.Empty;
}
