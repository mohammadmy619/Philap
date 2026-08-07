using Application.Accountings.GetAllAccountings;
using Domain.AccountingAggregate;
using MediatR;

namespace Application.Accountings
{
    public class GetAllAccountingsQueryHandler
        : IRequestHandler<GetAllAccountingsQuery, IEnumerable<GetAccountingResponse>>
    {
        private readonly IAccountingRepository _accountingRepository;

        public GetAllAccountingsQueryHandler(IAccountingRepository accountingRepository)
        {
            _accountingRepository = accountingRepository;
        }

        public async Task<IEnumerable<GetAccountingResponse>> Handle(
            GetAllAccountingsQuery query,
            CancellationToken cancellationToken)
        {
            var accountings = await _accountingRepository.GetAllAccountingsAsync(cancellationToken);

            return accountings.Select(MapToResponse);
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