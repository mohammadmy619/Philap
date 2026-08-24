using Application.Accountings.GetAllAccountings;
using Domain.AccountingAggregate;
using MediatR;

namespace Application.Accountings.GetAccountingById
{
    public class GetAccountingByIdQueryHandler
        : IRequestHandler<GetAccountingByIdQuery, GetAccountingResponse?>
    {
        private readonly IAccountingRepository _accountingRepository;

        public GetAccountingByIdQueryHandler(IAccountingRepository accountingRepository)
        {
            _accountingRepository = accountingRepository;
        }

        public async Task<GetAccountingResponse?> Handle(
            GetAccountingByIdQuery query,
            CancellationToken cancellationToken)
        {
            var accounting = await _accountingRepository.GetAccountingByIdAsync(query.AccountingId, cancellationToken);

            if (accounting == null)
            {
                return null;
            }

            return MapToResponse(accounting);
        }

        private static GetAccountingResponse MapToResponse(Accounting accounting)
        {
            return new GetAccountingResponse(
                AccountingId: accounting.Id,
                BookingId: accounting.BookingId,
                TripId: accounting.TripId,
                PassengerId: accounting.PassengerId,
                EntryDate: accounting.EntryDate,
                PurchaseDate: accounting.PurchaseDate,
                PriceAmount: accounting.Price.Amount,
                PriceDiscountAmount: accounting.Price.DiscountAmount,
                PriceFinalAmount: accounting.Price.FinalAmount,
                PriceCurrency: accounting.Price.Currency,
                PaymentStatus: accounting.PaymentStatus,
                Description: accounting.Description
            );
        }
    }
}