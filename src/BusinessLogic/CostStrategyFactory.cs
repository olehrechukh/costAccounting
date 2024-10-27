using BusinessLogic.Strategies;
using Models;

namespace BusinessLogic;

/// <summary>
/// Factory responsible for creating instances of cost calculation strategies based on the specified cost basis strategy.
/// </summary>
public class CostStrategyFactory
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// A dictionary mapping each <see cref="CostStrategy"/> type to its corresponding implementation type.
    /// </summary>
    private readonly Dictionary<CostStrategy, Type> _strategiesTypes = new()
    {
        [CostStrategy.FIFO] = typeof(FifoCostStrategy),
    };

    /// <summary>
    /// Initializes a new instance of the <see cref="CostStrategyFactory"/> class.
    /// </summary>
    /// <param name="serviceProvider">The service provider used to resolve strategy instances.</param>
    public CostStrategyFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Creates an instance of a cost calculation strategy based on the specified strategy type.
    /// </summary>
    /// <param name="strategy">The cost basis strategy to use for the calculation (e.g., FIFO).</param>
    /// <returns>An instance of <see cref="ICostStrategy"/> corresponding to the specified strategy.</returns>
    /// <exception cref="NotSupportedException">Thrown when the specified strategy type is not supported.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the strategy instance cannot be resolved from the service provider.</exception>
    public ICostStrategy CreateStrategy(CostStrategy strategy)
    {
        if (!_strategiesTypes.TryGetValue(strategy, out var strategyType))
        {
            throw new NotSupportedException($"Strategy '{strategy}' is not supported.");
        }

        var costStrategy = (ICostStrategy)_serviceProvider.GetService(strategyType);

        if (costStrategy == null)
        {
            throw new InvalidOperationException($"Cannot resolve instance of {strategyType}.");
        }

        return costStrategy;
    }
}
