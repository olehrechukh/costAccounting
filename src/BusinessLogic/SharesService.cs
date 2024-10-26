using DataAccess;
using Models;

namespace BusinessLogic;

public class SharesService
{
    private readonly ICostStrategyFactory _costStrategyFactory;
    private readonly IShareRepository _repository;

    public SharesService(
        ICostStrategyFactory costStrategyFactory,
        IShareRepository repository)
    {
        _costStrategyFactory = costStrategyFactory;
        _repository = repository;
    }

    public async Task<ShareLot[]> GetShares()
    {
        var shareLots = await _repository.GetShares();

        return shareLots;
    }

    public async Task<int> CalculateRemainingShares(int sharesToSell, CostStrategy costStrategy)
    {
        var shareLots = await _repository.GetShares();
        var strategy = _costStrategyFactory.CreateStrategy(costStrategy);
        int calculateRemainingShares = strategy.CalculateRemainingShares(shareLots, sharesToSell);

        return calculateRemainingShares;
    }
}

public interface ICostStrategyFactory
{
    /// <summary>
    ///     Creates the cost calculation strategy based on the specified strategy type.
    /// </summary>
    /// <param name="strategy">The cost basis strategy to use.</param>
    /// <returns>An instance of <see cref="ICostStraregy" /> corresponding to the specified strategy.</returns>
    ICostStraregy CreateStrategy(CostStrategy strategy);
}

internal class CostStrategyFactory : ICostStrategyFactory
{
    private readonly IServiceResolver _serviceResolver;

    private readonly Dictionary<CostStrategy, Type> _strategiesTypes = new()
    {
        [CostStrategy.FIFO] = typeof(FifoCostStrategy),
    };

    public CostStrategyFactory(IServiceResolver serviceResolver)
    {
        _serviceResolver = serviceResolver;
    }

    public ICostStraregy CreateStrategy(CostStrategy strategy)
    {
        if (!_strategiesTypes.TryGetValue(strategy, out var strategyType))
        {
            throw new NotSupportedException($"Strategy '{strategy}' is not supported.");
        }

        object costStrategy = _serviceResolver.GetService(strategyType);

        return (ICostStraregy)costStrategy;
    }
}

public interface IServiceResolver
{
    object GetService(Type type);
}
