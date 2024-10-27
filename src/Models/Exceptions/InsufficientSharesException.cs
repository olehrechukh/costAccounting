namespace Models.Exceptions;

/// <summary>
/// Exception thrown when an attempt is made to sell more shares than are available.
/// </summary>
public class InsufficientSharesException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InsufficientSharesException"/> class with a default error message
    /// indicating that the requested shares to sell exceed the available shares.
    /// </summary>
    public InsufficientSharesException() : base("Requested shares to sell exceeds available shares.")
    {
    }
}
