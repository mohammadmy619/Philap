using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Persons;
using MediatR;

namespace Application.Leader.GetLeaders
{
    public record GetLeadersQuery(int Page = 1, int PageSize = 10) : IRequest<GetLeadersResponse>;
}



