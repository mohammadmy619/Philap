namespace BuildingBlocks.Domain
{
    public abstract class DomainException : Exception
    {
        public string Code { get; protected set; }

        // Protected constructor for derived classes  
        protected DomainException(string code) : base(code)
        {
            Code = code;
        }

        // Optionally include a message and a specific code for better exception info  
        protected DomainException(string? message, string code) : base(message)
        {
            Code = code;
        }
    }
}