namespace QuotaQueen.QuotaStrategies;

public class QuotaStrategy(UpdateQuotaDelegate strategy)
{
  public readonly UpdateQuotaDelegate ExecuteStrategy = strategy;
}
