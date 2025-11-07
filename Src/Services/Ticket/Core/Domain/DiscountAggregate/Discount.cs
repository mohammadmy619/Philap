using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using BuildingBlocks.Domain;

namespace Domain.DiscountAggregate
{
    public class Discount : AggregateRoot<Guid>
    {
        public Guid Id { get; private set; }
        public string Code { get; private set; } // کد تخفیف (اختیاری)
        public DiscountType Type { get; private set; } // درصدی یا مقدار ثابت
        public decimal Value { get; private set; } // مقدار تخفیف (مثلاً 15% یا 20000 تومان)

        public DateTime? ValidFrom { get; private set; }
        public DateTime? ValidTo { get; private set; }

        public int? MaxUsageCount { get; private set; } // حداکثر تعداد استفاده کل
        public int UsedCount { get; private set; } // تعداد دفعات استفاده شده

        public Guid? ApplicableTripId { get; private set; } // اگر فقط برای یک سفر خاص باشد
        public Guid? ApplicablePassengerId { get; private set; } // اگر فقط برای یک کاربر خاص باشد

        public bool IsActive { get; private set; }

        public DateTime CreatedAt { get; private set; }
        public DateTime? LastUsedAt { get; private set; }

        // لیست استفاده‌های ثبت‌شده از این تخفیف (اختیاری - برای Audit)
        private readonly List<DiscountUsage> _usages = new();
        public IReadOnlyCollection<DiscountUsage> Usages => _usages.AsReadOnly();

    }
    public enum DiscountType
    {
        Percentage,     // درصدی
        FixedAmount     // مقدار ثابت
    }
}

public class DiscountUsage : Entity<Guid>
{
    public Guid DiscountId { get; private set; }
    public Guid BookingId { get; private set; }
    public Guid PassengerId { get; private set; }
    public Guid TripId { get; private set; }
    public decimal AppliedAmount { get; private set; }
    public DateTime UsedAt { get; private set; }

    public DiscountUsage(Guid discountId, Guid bookingId, Guid passengerId, Guid tripId, decimal appliedAmount)
    {
        Id = Guid.NewGuid();
        DiscountId = discountId;
        BookingId = bookingId;
        PassengerId = passengerId;
        TripId = tripId;
        AppliedAmount = appliedAmount;
        UsedAt = DateTime.UtcNow;
    }
}
