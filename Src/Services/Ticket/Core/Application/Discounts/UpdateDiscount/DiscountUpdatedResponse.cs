public record DiscountUpdatedResponse(
    Guid Id,
    string Code,
    bool IsActive,
    DateTime UpdatedAt 
);