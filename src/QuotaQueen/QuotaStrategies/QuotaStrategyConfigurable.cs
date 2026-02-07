using System.IO;
using BepInEx;
using BepInEx.Configuration;

namespace QuotaQueen.QuotaStrategies;

internal static class QuotaStrategyConfigurable
{
  static QuotaStrategyConfigurable()
  {
    var config = new ConfigFile(Path.Combine(Paths.ConfigPath, "QuotaQueen.ConfigurableQuota.cfg"), true);
    BaseQuota = config.Bind("ConfigurableQuota", "BaseQuota", 900, "The base quota amount at the start of the game");
    QuotaGrowth = config.Bind("ConfigurableQuota", "QuotaGrowth", 900, "The amount the quota increases each time a new quota is created");
  }

  internal static void EarlyBind()
  {
    QuotaQueenPlugin.Log.LogMessage("Binding config for QuotaStrategyConfigurable");
  }

  static readonly ConfigEntry<int> BaseQuota;
  static readonly ConfigEntry<int> QuotaGrowth;

  internal static int GetQuota(GameSnapshot gameState)
  {
    return BaseQuota.Value + (QuotaGrowth.Value * gameState.EffectiveQuotaCount);
  }
}
