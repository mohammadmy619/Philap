using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Application.Trips.GetTrip
{
    public record class GetTripQuery(Guid TripID) :IRequest<GetTripResponse>;

}
