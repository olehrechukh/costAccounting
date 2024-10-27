using BusinessLogic.Strategies;
using FluentAssertions;
using Models;
using Models.Exceptions;

namespace BusinessLogic.UnitTests;

public class FifoCostStrategyTests
{
    private readonly FifoCostStrategy _fifoCostStrategy = new();
    private readonly ShareLot[] _shareLots;
    private readonly int _sharesLimit;

    public FifoCostStrategyTests()
    {
        var firstInvestmentDateTime = new DateTime(DateTime.Now.Year, 1, 1);
        _shareLots =
        [
            new ShareLot(100, 20, firstInvestmentDateTime),
            new ShareLot(150, 30, firstInvestmentDateTime.AddMonths(1)),
            new ShareLot(120, 10, firstInvestmentDateTime.AddMonths(1))
        ];
        _sharesLimit = 370;
    }

    [Test]
    [TestCase(370, 0)]
    [TestCase(100, 270)]
    [TestCase(50, 320)]
    public void CalculateRemainingShares_ShouldReturnExpectedShares(int sharesToSell, int remainingShares)
    {
        int calculateRemainingShares = _fifoCostStrategy.CalculateRemainingShares(_shareLots, sharesToSell);
        calculateRemainingShares.Should().Be(remainingShares);
    }

    [Test]
    public void CalculateRemainingShares_ShouldArgumentException_WhenSellingMoreThanAvailable()
    {
        Action act = () => _fifoCostStrategy.CalculateRemainingShares(_shareLots, _sharesLimit + 1);

        act.Should().Throw<InsufficientSharesException>();
    }

    [Test]
    [TestCase(100, 20)]
    [TestCase(150, 23.3333)]
    [TestCase(250, 26)]
    [TestCase(300, 23.3333)]
    [TestCase(370, 20.8108)]
    public void CalculateCostBasisSold_ShouldReturnExpectedRemainingShares(int sharesToSell, decimal cost)
    {
        var costBasisSold = _fifoCostStrategy.CalculateCostBasisSold(_shareLots, sharesToSell);

        costBasisSold.Should().BeApproximately(cost, (decimal)Math.Pow(10, -4));
    }

    [Test]
    public void CalculateCostBasisSold_ShouldArgumentException_WhenSellingMoreThanAvailable()
    {
        Action act = () => _fifoCostStrategy.CalculateCostBasisSold(_shareLots, _sharesLimit + 1);

        act.Should().Throw<InsufficientSharesException>();
    }

    [Test]
    [TestCase(370, 0)]
    [TestCase(100, 21.1111)]
    [TestCase(250, 10)]
    [TestCase(50, 20.9375)]
    public void CalculateCostBasisRemaining_ShouldReturnExpectedCostBasisRemaining(int sharesToSell, decimal cost)
    {
        var costBasisSold = _fifoCostStrategy.CalculateCostBasisRemaining(_shareLots, sharesToSell);

        costBasisSold.Should().BeApproximately(cost, (decimal)Math.Pow(10, -4));
    }

    [Test]
    public void CalculateCostBasisRemaining_ShouldArgumentException_WhenSellingMoreThanAvailable()
    {
        Action act = () => _fifoCostStrategy.CalculateCostBasisRemaining(_shareLots, _sharesLimit + 1);

        act.Should().Throw<InsufficientSharesException>();
    }

    [Test]
    [TestCase(150, 2500)]
    public void CalculateProfitOrLoss_ShouldReturnExpectedRemainingShares(int sharesToSell, decimal cost)
    {
        var costBasisSold = _fifoCostStrategy.CalculateProfitOrLoss(_shareLots, sharesToSell, 40);

        costBasisSold.Should().BeApproximately(cost, (decimal)Math.Pow(10, -4));
    }
}
