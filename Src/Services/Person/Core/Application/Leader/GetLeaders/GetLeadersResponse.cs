using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Persons;

namespace Application.Leader.GetLeaders
{
    public record GetLeadersResponse(
    List<LeaderDto> Leaders,
    int TotalCount);



    public record LeaderDto(
        Guid Id,
        string Name,
        string LastName,
        string Email,
        DateTime DateOfBirth,
        Gender Gender,
        string Nationality,
        string Title,
        string Department,
        DateTime JoiningDate,
        int SkillsCount);
}
