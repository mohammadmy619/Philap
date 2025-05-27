using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Persons;
using MediatR;

namespace Application.Leader.UpdateLeader
{
    public record class UpdateLeaderCommand(
      Guid LeaderId,
      List<Guid>? TripIds,
      string Name,
      string LastName,
      string Email,
      string PhoneNumber,
      DateTime DateOfBirth,
      Gender Gender,
      string Nationality,
      string Title,
      string Department,
      DateTime JoiningDate,
      List<string> Skills,
      string Bio,
      string Street,
      string City,
      string State,
      string ZipCode
  ) : IRequest
    {
    }
}
