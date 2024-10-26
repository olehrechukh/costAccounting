using BusinessLogic;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace Web.Controllers;

/// <summary>
/// 
/// </summary>
public class SharesController : ControllerBase
{
    private readonly SharesService _calculationService;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="calculationService"></param>
    public SharesController(SharesService calculationService)
    {
        _calculationService = calculationService;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<ShareLot[]>> GetShares()
    {
        var shareLots = await _calculationService.GetShares();

        return shareLots;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sharesToSell"></param>
    /// <param name="costStrategy"></param>
    /// <returns></returns>
    [HttpGet("remaining")]
    public async Task<ActionResult<RemainingSharesDto>> CalculateRemainingShares(int sharesToSell, CostStrategy costStrategy)
    {
        if (sharesToSell <= 0)
        {
            return BadRequest($"{nameof(sharesToSell)} could not be less that 0");
        }

        if (costStrategy != CostStrategy.FIFO)
        {
            return BadRequest($"Application supports only {CostStrategy.FIFO} strategy");
        }
        
        var remainingShares = await _calculationService.CalculateRemainingShares(sharesToSell, costStrategy);

        return new RemainingSharesDto {Value = remainingShares};
    }
}
