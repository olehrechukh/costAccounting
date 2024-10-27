namespace Models;

/// <summary>
/// Cost basis strategies used in selling shares.
/// </summary>
public enum CostStrategy
{
    /// <summary>
    /// Unknown strategy, used as a default when no specific strategy is selected.
    /// </summary>
    Unknown,

    /// <summary>
    /// First-In, First-Out (FIFO) strategy.
    /// Shares are sold from the earliest purchased lot first, moving sequentially to later lots.
    /// </summary>
    FIFO,

    /// <summary>
    /// Last-In, First-Out (LIFO) strategy.
    /// Shares are sold from the most recently purchased lot first, moving sequentially to earlier lots.
    /// </summary>
    LIFO,

    /// <summary>
    /// Average Cost strategy.
    /// The average cost per share across all lots is used to calculate the cost basis of the shares sold.
    /// </summary>
    AverageCost,

    /// <summary>
    /// Lowest Tax Exposure strategy.
    /// Shares are sold in a way that minimizes tax liability, considering both short-term and long-term tax exposure.
    /// </summary>
    LowestTaxExposure,

    /// <summary>
    /// Highest Tax Exposure strategy.
    /// Shares are sold in a way that maximizes tax liability, which may be desired in certain tax planning scenarios.
    /// </summary>
    HighestTaxExposure,

    /// <summary>
    /// Lot-Based strategy.
    /// Allows the user to choose specific lots to sell from, rather than relying on an automated strategy.
    /// </summary>
    LotBased
}
