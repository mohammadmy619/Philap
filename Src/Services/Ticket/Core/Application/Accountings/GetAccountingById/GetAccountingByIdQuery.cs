using Application.Accountings.GetAllAccountings;
using MediatR;

namespace Application.Accountings.GetAccountingById
{
    public record GetAccountingByIdQuery(Guid AccountingId) : IRequest<GetAccountingResponse?>;
}
