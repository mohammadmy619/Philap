using Domain.Persons;
using Domain.Persons.Passenger;
using MediatR;

public class CreatePassengerCommandHandler(IPassengerRepository _passengerRepository)
    : IRequestHandler<CreatePassengerCommand, Guid>
{
    public async Task<Guid> Handle(CreatePassengerCommand request, CancellationToken ct)
    {
        // ساخت Address از داده‌های ورودی
        var address = new Address(
            street: request.Street,
            city: request.City,
            state: request.State,
            zipCode: request.ZipCode);

        //ساخت Passenger جدید
       var passenger = new Passenger(
         
           request.TripIds ?? new List<Guid>(),
           name: request.Name,
           lastName: request.LastName,
           email: request.Email,
           phoneNumber: request.PhoneNumber,
           dateOfBirth: request.DateOfBirth,
           gender: request.Gender,
           address: address,
           nationality: request.Nationality,
           passportNumber: request.PassportNumber,
           frequentFlyerNumbers: request.FrequentFlyerNumbers);
     

        // اضافه کردن به ریپوزیتوری
        await _passengerRepository.AddPassengerAsync(passenger, ct);

        // ذخیره تغییرات
        await _passengerRepository.SaveChangesAsync(ct);

        // بازگشت Id مسافر ایجاد شده
        return passenger.Id;
    }
}