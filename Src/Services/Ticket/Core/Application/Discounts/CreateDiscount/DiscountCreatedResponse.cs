public record DiscountCreatedResponse(
    Guid Id,
    string Code,
    bool IsActive,
    DateTime CreatedAt
);