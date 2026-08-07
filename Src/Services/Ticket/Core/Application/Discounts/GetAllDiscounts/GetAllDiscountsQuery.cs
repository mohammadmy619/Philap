using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Discounts.GetAllDiscounts
{
    public record GetDiscountsQuery(int Page = 1, int PageSize = 10)
          : IRequest<GetDiscountsResponse>;
}
