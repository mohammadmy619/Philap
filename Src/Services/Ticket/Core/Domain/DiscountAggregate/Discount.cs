using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using BuildingBlocks.Domain;
using Domain.DiscountAggregate.Exceptions;

namespace Domain.DiscountAggregate
{
    public class Discount : AggregateRoot<Guid>
    {
        #region Constructor
        // Constructor برای EF Core
        private Discount() { }

        public Discount(string code, DiscountType type, decimal value, DateTime? validFrom, DateTime? validTo, int? maxUsageCount, Guid? applicableTripId = null, Guid? applicablePassengerId = null)
        {
            GuardAgainstValue(value, type);
            GuardAgainstValidityPeriod(validFrom, validTo);
            GuardAgainstMaxUsageCount(maxUsageCount);
            Code = code;
            Type = type;
            Value = value;
            ValidFrom = validFrom;
            ValidTo = validTo;
            MaxUsageCount = maxUsageCount;
            UsedCount = 0; // Initialize to 0 on creation
            ApplicableTripId = applicableTripId;
            ApplicablePassengerId = applicablePassengerId;
            IsActive = true; // Assume active on creation
            CreatedAt = DateTime.UtcNow; // Set creation time
            LastUsedAt = null; // Not used yet


        }
        #endregion


        #region Peropertis
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

        #endregion


        #region Guard Methods
        private void GuardAgainstValue(decimal value, DiscountType type)
        {
            if (value < 0)
            {
                throw new DiscountValueIsInvalidException("Discount value cannot be negative.");
            }

            if (type == DiscountType.Percentage && value > 100)
            {
                throw new DiscountValueIsInvalidException("Percentage discount value cannot exceed 100%.");
            }
        }

        private void GuardAgainstValidityPeriod(DateTime? validFrom, DateTime? validTo)
        {
            if (validFrom.HasValue && validTo.HasValue && validFrom.Value > validTo.Value)
            {
                throw new DiscountValidityPeriodIsInvalidException("Valid from date cannot be after valid to date.");
            }
        }

        private void GuardAgainstMaxUsageCount(int? maxUsageCount)
        {
            if (maxUsageCount.HasValue && maxUsageCount.Value <= 0)
            {
                throw new DiscountMaxUsageCountIsInvalidException("Maximum usage count must be greater than zero.");
            }
        }
        #endregion

        #region Methods
        // Method to apply discount logic
        public decimal ApplyDiscount(decimal originalAmount, Guid? tripId = null, Guid? passengerId = null)
        {
            if (!CanApplyTo(tripId, passengerId))
            {
                throw new DiscountNotApplicableException("Discount cannot be applied to this booking.");
            }

            if (IsExpired())
            {
                throw new DiscountExpiredException("Discount has expired.");
            }

            if (IsAtMaxUsage())
            {
                throw new DiscountMaxUsageExceededException("Discount usage limit has been reached.");
            }

            decimal discountAmount = 0;
            if (Type == DiscountType.Percentage)
            {
                discountAmount = originalAmount * (Value / 100);
            }
            else if (Type == DiscountType.FixedAmount)
            {
                discountAmount = Math.Min(Value, originalAmount); // Cap at original amount
            }
            // Ensure discount doesn't make price negative
            return Math.Max(0, originalAmount - discountAmount);
        }

        // Method to record usage of the discount
        public void RecordUsage()
        {
            if (IsAtMaxUsage())
            {
                throw new DiscountMaxUsageExceededException("Cannot record usage: maximum usage count reached.");
            }

            UsedCount++;
            LastUsedAt = DateTime.UtcNow;

            // Add Domain Event for usage if needed
            // AddEvent(new DiscountUsedDomainEvent(Id, UsedCount, LastUsedAt));
        }

        // Check if discount is applicable to a specific booking context
        private bool CanApplyTo(Guid? tripId, Guid? passengerId)
        {
            if (!IsActive) return false;

            // Check trip-specific condition
            if (ApplicableTripId.HasValue && tripId.HasValue && ApplicableTripId.Value != tripId.Value)
            {
                return false;
            }

            // Check passenger-specific condition
            if (ApplicablePassengerId.HasValue && passengerId.HasValue && ApplicablePassengerId.Value != passengerId.Value)
            {
                return false;
            }

            return true;
        }

        private bool IsExpired()
        {
            if (ValidTo.HasValue && DateTime.UtcNow > ValidTo.Value)
            {
                return true;
            }
            return false;
        }

        private bool IsAtMaxUsage()
        {
            if (MaxUsageCount.HasValue && UsedCount >= MaxUsageCount.Value)
            {
                return true;
            }
            return false;
        }

        // Method to deactivate discount
        public void Deactivate()
        {
            IsActive = false;
            // Add Domain Event for deactivation if needed
            // AddEvent(new DiscountDeactivatedDomainEvent(Id));
        }

        // Method to activate discount
        public void Activate()
        {
            IsActive = true;
            // Add Domain Event for activation if needed
            // AddEvent(new DiscountActivatedDomainEvent(Id));
        }
        #endregion

    }
}
