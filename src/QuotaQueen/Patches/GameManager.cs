using HarmonyLib;
using QuotaQueen.QuotaStrategies;
using YAPYAP;

namespace QuotaQueen.Patches;

internal static class GameManagerPatches
{

  [HarmonyPatch(typeof(GameManager), nameof(GameManager.OnStartServer))]
  [HarmonyPostfix]
  static void GameStartPatch(GameManager __instance)
  {
    QuotaQueenPlugin.Instance.FreshenConfig();

    if (__instance.isServer)
    {
      var cfg = QuotaQueenPlugin.QueenConfig;
      QuotaQueenPlugin.Log.LogInfo("Configuring this round's details for the host");

      __instance.baseRoundDuration = cfg.RoundDuration.Value;

      __instance.roundsToQuota = cfg.QuotaDays.Value;
      __instance.baseGoldReward = cfg.GoldReward.Value;

      __instance.deathPenaltyPercentageBased = cfg.UsePCTPenalty.Value;
      __instance.deathPenaltyFlat = cfg.DeathPenaltyFlat.Value;
      __instance.deathPenaltyPercent = cfg.DeathPenaltyPCT.Value;

      RunInitialQuotaCalc(__instance);
    }
    //Pretty sure this never runs because OnStartServer is ONLY called on hosts
    else
      QuotaQueenPlugin.Log.LogWarning("OnStartServer as client, shutting down for this round");
  }

  [HarmonyPatch(typeof(GameManager), nameof(GameManager.GetNextQuotaScoreGoal))]
  [HarmonyPostfix]
  static void GetNextQuotaScoreGoalPatch(GameManager __instance, ref int __result)
  {
    var strategy = QuotaQueenPlugin.QueenConfig.QuotaStrategy.Value;

    QuotaQueenPlugin.Log.LogMessage($"Using {strategy} for this round's quota calculation");

    if (strategy == QuotaStrategyManager.DefaultStrategyGUID)
      return;

    if (QuotaStrategyManager.TryExecuteStrategy(strategy, new(__instance, true), out var value))
      __result = value;
    else
    {
      QuotaQueenPlugin.Log.LogError("Failed to execute strategy, using default instead");
    }
  }

  [HarmonyPatch(typeof(GameManager), nameof(GameManager.GetQuotaForSession))]
  [HarmonyPostfix]
  static void GetQuotaForSessionPatch(GameManager __instance, int sessionIndex, ref int __result)
  {
    var strategy = QuotaQueenPlugin.QueenConfig.QuotaStrategy.Value;

    QuotaQueenPlugin.Log.LogMessage($"Using {strategy} for session quota");

    if (strategy == QuotaStrategyManager.DefaultStrategyGUID)
      return;

    if (QuotaStrategyManager.TryExecuteStrategy(strategy, new(__instance), out var value))
      __result = value;
    else
    {
      QuotaQueenPlugin.Log.LogError("Failed to execute strategy, using default instead");
    }
  }

  static void RunInitialQuotaCalc(GameManager __instance)
  {
    var strategy = QuotaQueenPlugin.QueenConfig.QuotaStrategy.Value;

    QuotaQueenPlugin.Log.LogMessage($"Using {strategy} for initial/restore quota");

    if (strategy == QuotaStrategyManager.DefaultStrategyGUID)
      return;

    if (QuotaStrategyManager.TryExecuteStrategy(strategy, new(__instance), out var value))
      __instance.NetworkcurrentQuotaScoreGoal = value;
    else
    {
      QuotaQueenPlugin.Log.LogError("Failed to execute strategy, using default instead");
    }
  }
}
