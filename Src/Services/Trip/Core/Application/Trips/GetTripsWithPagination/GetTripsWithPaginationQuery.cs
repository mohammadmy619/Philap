using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Pagination;
using Application.Trips.GetTrip;
using MediatR;

namespace Application.Trips.GetTripsWithPagination
{
    public record class GetTripsWithPaginationQuery(int PageNumber=1, int PageSize=10) : IRequest<IReadOnlyCollection<GetTripsResponse>>;
  
}
