using Models;

namespace BusinessLogic;

/// <summary>
/// Implements the FIFO (First-In, First-Out) cost basis strategy for share sales,
/// where shares are sold from the earliest acquired lot first. This strategy calculates 
/// metrics such as remaining shares, cost basis of sold shares, cost basis of remaining shares, 
/// and total profit or loss, based on a sequential order of acquisition.
/// </summary>
public class FifoCostStrategy : ICostStraregy
{
    public int CalculateRemainingShares(ShareLot[] shareLots, int sharesToSell)
    {
        var totalShares = shareLots.Sum(shareLot => shareLot.Quantity);
        var remainingShares = totalShares - sharesToSell;

        if (remainingShares < 0)
        {
            throw new ArgumentException($"You cannot sell more than {totalShares} shares.");
        }

        return remainingShares;
    }

    public decimal CalculateCostBasisSold(List<ShareLot> shareLots, int sharesToSell)
    {
        throw new NotImplementedException();
    }

    public decimal CalculateCostBasisRemaining(List<ShareLot> shareLots)
    {
        throw new NotImplementedException();
    }

    public decimal CalculateProfitOrLoss(List<ShareLot> shareLots, int sharesToSell, decimal salePrice)
    {
        throw new NotImplementedException();
    }
}
