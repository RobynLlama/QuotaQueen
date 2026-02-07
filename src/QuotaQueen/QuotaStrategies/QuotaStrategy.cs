namespace QuotaQueen.QuotaStrategies;

/// <summary>
/// Encapsulates a Quota Strategy and its relevant configs for use
/// with the strategy manager
/// </summary>
/// <param name="strategy">A reference to the method to call for this strategy</param>
public class QuotaStrategy(UpdateQuotaDelegate strategy)
{
  /// <summary>
  /// The delegate for running the strategy
  /// </summary>
  public readonly UpdateQuotaDelegate ExecuteStrategy = strategy;
}
