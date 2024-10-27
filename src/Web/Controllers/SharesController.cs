using BusinessLogic;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace Web.Controllers;

/// <summary>
/// Represent share-related endpoints.
/// </summary>
[Route("api/[controller]")]
public class SharesController : ControllerBase
{
    private readonly ShareService _calculationService;

    /// <summary>
    /// Initializes a new instance of the <see cref="SharesController"/> class.
    /// </summary>
    /// <param name="calculationService">Service for calculating share data.</param>
    public SharesController(ShareService calculationService)
    {
        _calculationService = calculationService;
    }

    /// <summary>
    /// Retrieves the available share lots.
    /// </summary>
    /// <returns>An array of <see cref="ShareLot"/> objects representing the available shares.</returns>
    [HttpGet]
    public async Task<ShareLot[]> GetShares()
    {
        var shareLots = await _calculationService.GetShares();
        return shareLots;
    }

    /// <summary>
    /// Calculates the cost details, including remaining shares, based on the number of shares to sell, sale price, and cost strategy.
    /// </summary>
    /// <param name="sharesToSell">The number of shares to sell.</param>
    /// <param name="salePrice">The price per share for the sale.</param>
    /// <param name="costStrategy">The cost strategy to use for the calculation.</param>
    /// <returns>A <see cref="CostCalculationResult"/> object with remaining shares, cost basis, and profit/loss details.</returns>
    [HttpGet("remaining")]
    public async Task<ActionResult<CostCalculationResult>> CalculateRemainingShares(int sharesToSell, decimal salePrice, CostStrategy costStrategy)
    {
        if (sharesToSell <= 0)
        {
            return BadRequest(new ProblemDetails {Detail = $"{nameof(sharesToSell)} must be greater than 0."});
        }

        if (costStrategy != CostStrategy.FIFO)
        {
            return BadRequest(new ProblemDetails {Detail = $"Only {CostStrategy.FIFO} strategy is supported."});
        }

        var remainingShares = await _calculationService.CalculateCostDetails(sharesToSell, salePrice, costStrategy);

        return remainingShares;
    }
}
