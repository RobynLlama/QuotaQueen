using HarmonyLib;
using YAPYAP;

namespace QuotaQueen.Patches;

public static class GameManagerPatches
{
  [HarmonyPatch(typeof(GameManager), nameof(GameManager.OnStartServer))]
  [HarmonyPostfix]
  public static void GameStartPatch(GameManager __instance)
  {
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
    }
    else
      QuotaQueenPlugin.Log.LogWarning("OnStartServer as client, shutting down for this round");
  }
}
