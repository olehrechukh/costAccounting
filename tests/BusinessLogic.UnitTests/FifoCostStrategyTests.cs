using FluentAssertions;
using Models;

namespace BusinessLogic.UnitTests;

public class FifoCostStrategyTests
{
    private readonly FifoCostStrategy _fifoCostStrategy = new();
    private readonly ShareLot[] _shareLots;

    public FifoCostStrategyTests()
    {
        var firstInvestmentDateTime = new DateTime(DateTime.Now.Year, 1, 1);
        _shareLots =
        [
            new ShareLot(100, 20, firstInvestmentDateTime),
            new ShareLot(100, 20, firstInvestmentDateTime.AddMonths(1))
        ];
    }
    
    [Test]
    [TestCase(200, 0)]
    [TestCase(100, 100)]
    [TestCase(50, 150)]
    [TestCase(0, 200)]
    public void CalculateRemainingShares_ShouldReturnExpectedRemainingShares(int sharesToSell, int remainingShares)
    {
        int calculateRemainingShares = _fifoCostStrategy.CalculateRemainingShares(_shareLots, 50);
        calculateRemainingShares.Should().Be(150);
    }    
    
    [Test]
    public void CalculateRemainingShares_ShouldThrowInsufficientSharesException_WhenSellingMoreThanAvailable()
    {
        Action act = () => _fifoCostStrategy.CalculateRemainingShares(_shareLots, 201);

        act.Should().Throw<ArgumentException>()
            .WithMessage("You cannot sell more than 200 shares.");
    }
}
