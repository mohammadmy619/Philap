using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Persons;

namespace Application.Leader.GetLeaderById
{
    using static GetLeaderByIdResponse;

    public record GetLeaderByIdResponse(
        Guid Id,
        List<Guid>? TripIds,
        string Name,
        string LastName,
        string Email,
        string PhoneNumber,
        DateTime DateOfBirth,
        Gender gender,
        AddressDto Address,
        string Nationality,
        string Title,
        string Department,
        DateTime JoiningDate,
        List<string> Skills,
        string Bio)
    {
        public record AddressDto(
            string Street,
            string City,
            string State,
            string ZipCode);
 
    }
}
