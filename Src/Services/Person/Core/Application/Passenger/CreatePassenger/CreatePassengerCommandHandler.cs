using Domain.Persons;
using Domain.Persons.Passenger;
using MediatR;

public class CreatePassengerCommandHandler(IPassengerRepository _passengerRepository)
    : IRequestHandler<CreatePassengerCommand, CreatePassengerResponse>
{
    public async Task<CreatePassengerResponse> Handle(CreatePassengerCommand request, CancellationToken ct)
    {
        // ساخت Address از داده‌های ورودی
        var address = new Address(
            street: request.Street,
            city: request.City,
            state: request.State,
            zipCode: request.ZipCode);

        var passenger = new Passenger(
               name: request.Name,
               lastName: request.LastName,
               email: request.Email,
               phoneNumber: request.PhoneNumber,
               dateOfBirth: request.DateOfBirth,
               gender: request.Gender,
               address: address,
               nationality: request.Nationality,
               isActive: request.isActive,
               passportNumber: request.PassportNumber,
               frequentFlyerNumbers: request.FrequentFlyerNumbers);

        // ۳. اضافه کردن TripIds به صورت امن و طبق اصول DDD (اگر مقداری وجود داشته باشد)
        if (request.TripIds != null && request.TripIds.Any())
        {
            foreach (var tripId in request.TripIds)
            {
                passenger.AddTripId(tripId);
            }
        }


        // اضافه کردن به ریپوزیتوری
        await _passengerRepository.AddPassengerAsync(passenger, ct);

        // ذخیره تغییرات
        await _passengerRepository.SaveChangesAsync(ct);

        // بازگشت Id مسافر ایجاد شده
        return new CreatePassengerResponse(passenger.Id);
    }
}
