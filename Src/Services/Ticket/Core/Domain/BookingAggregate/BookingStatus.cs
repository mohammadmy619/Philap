public enum BookingStatus
{
    /// <summary>
    /// رزرو اولیه ایجاد شده است
    /// </summary>
    Created = 1,

    /// <summary>
    /// رزرو تایید شده و قطعی است
    /// </summary>
    Confirmed = 2,

    /// <summary>
    /// رزرو کنسل شده است
    /// </summary>
    Cancelled = 3,

    /// <summary>
    /// رزرو در انتظار پرداخت است
    /// </summary>
    PendingPayment = 4,

    /// <summary>
    /// رزرو تکمیل شده است
    /// </summary>
    Completed = 5,

    /// <summary>
    /// رزرو منقضی شده است
    /// </summary>
    Expired = 6,

    /// <summary>
    /// رزرو رد شده است
    /// </summary>
    Rejected = 7
}