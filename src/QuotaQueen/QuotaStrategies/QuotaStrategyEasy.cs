namespace QuotaQueen.QuotaStrategies;

internal static class QuotaStrategyEasy
{
  internal static int GetEasyQuota(GameSnapshot gameState)
  {
    return 300 + (300 * gameState.EffectiveQuotaCount);
  }
}
