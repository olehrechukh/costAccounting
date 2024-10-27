using Ardalis.GuardClauses;
using Models;
using Models.Exceptions;

namespace BusinessLogic.Strategies;

/// <summary>
/// An abstract base class for calculating share-related metrics like cost basis and profit/loss.
/// This class contains all the logic for calculations, only requiring derived classes to override the share ordering strategy.
/// </summary>
public abstract class CostStrategyBase : ICostStrategy
{
    /// <summary>
    /// Calculates the remaining shares after selling a specified number of shares.
    /// Ensures that the number of shares to sell does not exceed available shares.
    /// </summary>
    /// <param name="shareLots">An array of <see cref="ShareLot"/> representing the available shares.</param>
    /// <param name="sharesToSell">The number of shares to sell.</param>
    /// <returns>The number of remaining shares after the sale.</returns>
    /// <exception cref="ArgumentException">Thrown if shares to sell exceed available shares.</exception>
    public int CalculateRemainingShares(ShareLot[] shareLots, int sharesToSell)
    {
        Guard.Against.NegativeOrZero(sharesToSell);

        var totalShares = shareLots.Sum(shareLot => shareLot.Quantity);
        var remainingShares = totalShares - sharesToSell;

        if (remainingShares < 0)
        {
            throw new InsufficientSharesException();
        }

        return remainingShares;
    }

    /// <summary>
    /// Calculates the cost basis per share of the shares sold based on the specified strategy.
    /// This method determines the average cost basis of the shares sold according to the derived class's ordering strategy.
    /// </summary>
    /// <param name="shareLots">An array of <see cref="ShareLot"/> representing the available shares.</param>
    /// <param name="sharesToSell">The number of shares to sell.</param>
    /// <returns>The average cost basis per share of the sold shares.</returns>
    public decimal CalculateCostBasisSold(ShareLot[] shareLots, int sharesToSell)
    {
        Guard.Against.NegativeOrZero(sharesToSell);

        int remainingSharesToSell = sharesToSell;
        decimal totalCost = 0m;
        int totalSharesSold = 0;

        foreach (var lot in OrderShareLot(shareLots))
        {
            if (remainingSharesToSell <= 0)
            {
                break;
            }

            int sharesFromLot = Math.Min(remainingSharesToSell, lot.Quantity);

            totalCost += sharesFromLot * lot.PricePerShare;
            totalSharesSold += sharesFromLot;
            remainingSharesToSell -= sharesFromLot;
        }

        if (remainingSharesToSell > 0)
        {
            throw new InsufficientSharesException();
        }

        return totalCost / totalSharesSold;
    }

    /// <summary>
    /// Calculates the cost basis per share of the remaining shares after a sale.
    /// This method determines the cost basis of the remaining shares based on the specific ordering strategy.
    /// </summary>
    /// <param name="shareLots">An array of <see cref="ShareLot"/> representing the available shares.</param>
    /// <param name="sharesToSell">The number of shares to sell.</param>
    /// <returns>The cost basis per share of the remaining shares after the sale.</returns>
    public decimal CalculateCostBasisRemaining(ShareLot[] shareLots, int sharesToSell)
    {
        Guard.Against.NegativeOrZero(sharesToSell);

        int remainingSharesToSell = sharesToSell;
        var remainingLots = new List<ShareLot>();

        foreach (var lot in OrderShareLot(shareLots))
        {
            if (remainingSharesToSell <= 0)
            {
                remainingLots.Add(lot);
                continue;
            }

            if (remainingSharesToSell < lot.Quantity)
            {
                var shareLot = new ShareLot(lot.Quantity - remainingSharesToSell, lot.PricePerShare,
                    lot.InvestmentDate);
                
                remainingLots.Add(shareLot);
                remainingSharesToSell = 0;
            }
            else
            {
                remainingSharesToSell -= lot.Quantity;
            }
        }

        if (remainingSharesToSell > 0)
        {
            throw new InsufficientSharesException();
        }

        if (remainingLots.Count == 0)
        {
            return 0;
        }

        var totalRemainingCost = remainingLots.Sum(lot => lot.Quantity * lot.PricePerShare);
        return totalRemainingCost / remainingLots.Sum(lot => lot.Quantity);
    }

    /// <summary>
    /// Calculates the profit or loss for a sale based on the number of shares to sell and the sale price per share.
    /// </summary>
    /// <param name="shareLots">An array of <see cref="ShareLot"/> representing the available shares.</param>
    /// <param name="sharesToSell">The number of shares to sell.</param>
    /// <param name="salePrice">The sale price per share.</param>
    /// <returns>The profit or loss of the sale.</returns>
    public decimal CalculateProfitOrLoss(ShareLot[] shareLots, int sharesToSell, decimal salePrice)
    {
        var costBasisSold = CalculateCostBasisSold(shareLots, sharesToSell);
        var totalSaleRevenue = salePrice * sharesToSell;

        return totalSaleRevenue - (costBasisSold * sharesToSell);
    }

    /// <summary>
    /// Orders the share lots according to a specific strategy.
    /// Derived classes override this method to specify the desired order.
    /// </summary>
    /// <param name="shareLots">An array of <see cref="ShareLot"/> representing the available shares.</param>
    /// <returns>An ordered collection of <see cref="ShareLot"/> based on the specific strategy.</returns>
    protected abstract IEnumerable<ShareLot> OrderShareLot(ShareLot[] shareLots);
}
