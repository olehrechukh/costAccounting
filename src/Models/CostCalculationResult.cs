namespace Models;

/// <summary>
/// Represents the result of a cost calculation for a share sale transaction.
/// </summary>
public class CostCalculationResult
{
    /// <summary>
    /// Gets or sets the number of shares remaining after the sale transaction.
    /// </summary>
    public int RemainingShares { get; set; }

    /// <summary>
    /// Gets or sets the average cost basis per share of the shares sold in the transaction.
    /// </summary>
    public decimal CostBasisSold { get; set; }

    /// <summary>
    /// Gets or sets the average cost basis per share of the shares remaining after the sale.
    /// </summary>
    public decimal CostBasisRemaining { get; set; }

    /// <summary>
    /// Gets or sets the total profit or loss from the share sale transaction.
    /// </summary>
    public decimal ProfitOrLoss { get; set; }
}
