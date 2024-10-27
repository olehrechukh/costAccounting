using Ardalis.GuardClauses;
using DataAccess;
using Models;
using Models.Exceptions;

namespace BusinessLogic;

/// <summary>
/// Service responsible for handling share-related operations.
/// </summary>
public class ShareService
{
    private readonly CostStrategyFactory _costStrategyFactory;
    private readonly IShareRepository _repository;

    /// <summary>
    /// Initializes a new instance of the <see cref="ShareService"/> class.
    /// </summary>
    /// <param name="costStrategyFactory">The factory responsible for creating the appropriate cost calculation strategy.</param>
    /// <param name="repository">The repository for retrieving share data.</param>
    public ShareService(
        CostStrategyFactory costStrategyFactory,
        IShareRepository repository)
    {
        _costStrategyFactory = costStrategyFactory;
        _repository = repository;
    }

    /// <summary>
    /// Retrieves the available shares.
    /// </summary>
    /// <returns>An array of <see cref="ShareLot"/> representing the available shares.</returns>
    public async Task<ShareLot[]> GetShares()
    {
        var shareLots = await _repository.GetShares();
        return shareLots;
    }

    /// <summary>
    /// Calculates cost details for a specified share sale transaction, including remaining shares,
    /// cost basis for sold and remaining shares, and profit or loss.
    /// </summary>
    /// <param name="sharesToSell">The number of shares to sell.</param>
    /// <param name="salePrice">The sale price per share.</param>
    /// <param name="costStrategy">The cost calculation strategy to use (e.g., FIFO, LIFO).</param>
    /// <returns>A <see cref="CostCalculationResult"/> containing calculated details for the sale.</returns>
    /// <exception cref="ArgumentException">Thrown if requested shares to sell exceed available shares.</exception>
    public async Task<CostCalculationResult> CalculateCostDetails(int sharesToSell, decimal salePrice, CostStrategy costStrategy)
    {
        Guard.Against.NegativeOrZero(sharesToSell);
        Guard.Against.NegativeOrZero(salePrice);
        
        var shareLots = await _repository.GetShares();

        if (GetTotalSharesQuantity(shareLots) < sharesToSell)
        {
            throw new InsufficientSharesException();
        }
        
        var strategy = _costStrategyFactory.CreateStrategy(costStrategy);

        var remainingShares = strategy.CalculateRemainingShares(shareLots, sharesToSell);
        var costBasisRemaining = strategy.CalculateCostBasisRemaining(shareLots, sharesToSell);
        var costBasisSold = strategy.CalculateCostBasisSold(shareLots, sharesToSell);
        var calculateProfitOrLoss = strategy.CalculateProfitOrLoss(shareLots, sharesToSell, salePrice);

        return new CostCalculationResult
        {
            RemainingShares = remainingShares,
            CostBasisRemaining = costBasisRemaining,
            CostBasisSold = costBasisSold,
            ProfitOrLoss = calculateProfitOrLoss
        };
    }

    private static int GetTotalSharesQuantity(ShareLot[] shareLots) => shareLots.Sum(shareLot => shareLot.Quantity);
}
