using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildingBlocks.Domain;

namespace Application.Trips.GetTrip
{
    public class GetTripNotFoundException:DomainException
    {
        public GetTripNotFoundException(string message = "Trip Not Found", string code = "0814023")
    : base(message, code) { }
    }
}
