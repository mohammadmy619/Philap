public class CreateLeaderResponse
{
    public Guid LeaderId { get; set; }
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;

    public static CreateLeaderResponse SuccessResponse(Guid leaderId)
    {
        return new CreateLeaderResponse
        {
            LeaderId = leaderId,
            Success = true,
            Message = "Leader created successfully."
        };
    }

    public static CreateLeaderResponse FailureResponse(string message)
    {
        return new CreateLeaderResponse
        {
            Success = false,
            Message = message
        };
    }
}
