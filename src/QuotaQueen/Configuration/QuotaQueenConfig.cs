using System.IO;
using System.Text;
using BepInEx;
using BepInEx.Configuration;
using QuotaQueen.QuotaStrategies;

namespace QuotaQueen.Configuration;

internal class QuotaQueenConfig
{

  internal ConfigFile Cfg;
  internal ConfigEntry<int> RoundDuration;
  internal ConfigEntry<int> QuotaDays;
  internal ConfigEntry<int> GoldReward;
  internal ConfigEntry<string> QuotaStrategy;

  internal ConfigEntry<bool> UsePCTPenalty;
  internal ConfigEntry<int> DeathPenaltyFlat;
  internal ConfigEntry<float> DeathPenaltyPCT;

  internal QuotaQueenConfig(ConfigFile cfg)
  {
    Cfg = cfg;
    RoundDuration = cfg.Bind("Round Settings", "RoundDuration", 720, "The length of each day in seconds");

    QuotaDays = cfg.Bind("Quota Settings", "QuotaDays", 3, "The number of days per quota");
    GoldReward = cfg.Bind("Quota Settings", "GoldReward", 25, "How much gold is given for completing a quota.\nNote: More gold is rewarded for exceeding the quota, this is the minimum for meeting any quota");

    QuotaStrategy = cfg.Bind("Quota Settings", "QuotaStrategy", QuotaStrategyManager.DefaultStrategyGUID, new ConfigDescription("This controls the way the game generates each new quota", new AcceptableValueList<string>(QuotaStrategyManager.StrategyKeys)));
    QuotaStrategyManager.Lock();

    UsePCTPenalty = cfg.Bind("Death Settings", "UsePCTPenalty", true, "If this flag is disabled then the flat death penalty will be subbed in for the default percentage based penalty");
    DeathPenaltyFlat = cfg.Bind("Death Settings", "DeathPenaltyFlat", 100, "If the death penalty is set to flat mode this much score will be removed per dead player at the end of the round");
    DeathPenaltyPCT = cfg.Bind("Death Settings", "DeathPenaltyPCT", 0.1f, "If the death penalty is set to flat mode this percentage of the total score will be removed per dead player");
  }

  internal static QuotaQueenConfig BindConfig(BepInPlugin owner)
  {
    var cfg = new ConfigFile(Path.Combine(Paths.ConfigPath, "QuotaQueen.cfg"), true, owner);
    return new(cfg);
  }

  public override string ToString()
  {
    StringBuilder sb = new();
    sb.AppendLine("Quota Queen Config:");
    sb.AppendLine($"--Round Configs--");
    sb.AppendLine($"  Round Duration:    {RoundDuration.Value}");
    sb.AppendLine($"--Quota Configs--");
    sb.AppendLine($"  Quota Days:        {QuotaDays.Value}");
    sb.AppendLine($"  Quota Reward:      {GoldReward.Value}");
    sb.AppendLine($"  Quota Strategy:    {QuotaStrategy.Value}");
    sb.AppendLine($"--Death Configs--");
    sb.AppendLine($"  Using PCT Penalty: {UsePCTPenalty.Value}");
    sb.AppendLine($"  Percent Penalty:   {DeathPenaltyPCT.Value:P1}");
    sb.AppendLine($"  Flat Penalty:      {DeathPenaltyFlat.Value}");

    return sb.ToString();
  }
}
