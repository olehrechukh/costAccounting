using Models;

namespace BusinessLogic;

/// <summary>
/// Represents operations for determining key financial metrics in share sales,
/// </summary>
public interface ICostStraregy
{
    /// <summary>
    /// Calculates the remaining number of shares after the sale.
    /// </summary>
    /// <param name="shareLots">The collection of share lots available before the sale.</param>
    /// <param name="sharesToSell">The number of shares to sell.</param>
    /// <returns>The number of shares remaining after the sale.</returns>
    int CalculateRemainingShares(ShareLot[] shareLots, int sharesToSell);

    /// <summary>
    /// Calculates the cost basis per share of the shares that were sold.
    /// </summary>
    /// <param name="shareLots">The collection of share lots available before the sale.</param>
    /// <param name="sharesToSell">The number of shares to sell.</param>
    /// <returns>The cost basis per share of the sold shares.</returns>
    decimal CalculateCostBasisSold(List<ShareLot> shareLots, int sharesToSell);

    /// <summary>
    /// Calculates the cost basis per share of the shares remaining after the sale.
    /// </summary>
    /// <param name="shareLots">The collection of share lots remaining after the sale.</param>
    /// <returns>The cost basis per share of the remaining shares.</returns>
    decimal CalculateCostBasisRemaining(List<ShareLot> shareLots);

    /// <summary>
    /// Calculates the total profit or loss of the sale.
    /// </summary>
    /// <param name="shareLots">The collection of share lots available before the sale.</param>
    /// <param name="sharesToSell">The number of shares to sell.</param>
    /// <param name="salePrice">The sale price per share.</param>
    /// <returns>The total profit or loss of the sale.</returns>
    decimal CalculateProfitOrLoss(List<ShareLot> shareLots, int sharesToSell, decimal salePrice);
}
