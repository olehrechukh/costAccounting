using Models;

namespace BusinessLogic.Strategies;

/// <summary>
/// Implements the FIFO (First-In, First-Out) cost basis strategy for share sales,
/// where shares are sold from the earliest acquired lot first. This strategy calculates 
/// metrics such as remaining shares, cost basis of sold shares, cost basis of remaining shares, 
/// and total profit or loss, based on a sequential order of acquisition.
/// </summary>
public class FifoCostStrategy : CostStrategyBase
{
    /// <summary>
    /// Orders the share lots in ascending order based on their acquisition date,
    /// implementing the FIFO (First-In, First-Out) strategy where the earliest acquired shares are sold first.
    /// </summary>
    /// <param name="shareLots">An array of <see cref="ShareLot"/> representing the available shares.</param>
    /// <returns>An ordered collection of <see cref="ShareLot"/> based on the acquisition date, from earliest to latest.</returns>
    protected override IEnumerable<ShareLot> OrderShareLot(ShareLot[] shareLots) =>
        shareLots.OrderBy(shareLot => shareLot.InvestmentDate);

}
