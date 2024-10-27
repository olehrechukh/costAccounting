using BusinessLogic.Strategies;
using FluentAssertions;
using Models;
using Moq;

namespace BusinessLogic.UnitTests;

public class CostStrategyFactoryTests
{
    private readonly Mock<IServiceProvider> _serviceProviderMock;
    private readonly CostStrategyFactory _factory;

    public CostStrategyFactoryTests()
    {
        _serviceProviderMock = new Mock<IServiceProvider>();
        _factory = new CostStrategyFactory(_serviceProviderMock.Object);
    }

    [Test]
    public void CreateStrategy_ShouldReturnFifoStrategy_WhenStrategyIsFifo()
    {
        var fifoCostStrategy = new FifoCostStrategy();
        _serviceProviderMock.Setup(sp => sp.GetService(typeof(FifoCostStrategy)))
            .Returns(fifoCostStrategy);

        var result = _factory.CreateStrategy(CostStrategy.FIFO);

        result.Should().Be(fifoCostStrategy);
    }

    [Test]
    public void CreateStrategy_ShouldThrowNotSupportedException_WhenStrategyIsNotSupported()
    {
        var unsupportedStrategy = (CostStrategy)999; // Invalid strategy type

        Action act = () => _factory.CreateStrategy(unsupportedStrategy);

        act.Should().Throw<NotSupportedException>()
            .WithMessage("Strategy '999' is not supported.");
    }

    [Test]
    public void CreateStrategy_ShouldThrowInvalidOperationException_WhenStrategyCannotBeResolved()
    {
        _serviceProviderMock.Setup(sp => sp.GetService(typeof(FifoCostStrategy)))
            .Returns(null!);

        Action act = () => _factory.CreateStrategy(CostStrategy.FIFO);

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Cannot resolve instance of BusinessLogic.Strategies.FifoCostStrategy.");
    }
}
