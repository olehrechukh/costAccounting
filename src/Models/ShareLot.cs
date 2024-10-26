namespace Models;

/// <summary>
/// Represents a lot of shares purchased at a specific price and date.
/// </summary>
public class ShareLot
{
    /// <summary>
    /// Gets the quantity of shares in this lot.
    /// </summary>
    public int Quantity { get; }

    /// <summary>
    /// Gets the price per share for this lot.
    /// </summary>
    public decimal PricePerShare { get; }

    /// <summary>
    /// Gets the date when the shares were purchased.
    /// </summary>
    public DateTime InvestmentDate { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ShareLot"/> class.
    /// </summary>
    /// <param name="quantity">The quantity of shares in this lot.</param>
    /// <param name="pricePerShare">The price per share for this lot.</param>
    /// <param name="investmentDate">The date of purchase for this lot.</param>
    public ShareLot(int quantity, decimal pricePerShare, DateTime investmentDate)
    {
        Quantity = quantity;
        PricePerShare = pricePerShare;
        InvestmentDate = investmentDate;
    }
}
