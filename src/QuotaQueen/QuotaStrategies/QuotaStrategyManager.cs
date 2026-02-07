using System.Collections.Generic;
using System.IO;
using BepInEx;
using BepInEx.Configuration;

namespace QuotaQueen.QuotaStrategies;

/// <summary>
/// This class is for managing the use and registration of quota strategies
/// that modify the difficulty of quota scaling
/// </summary>
public static class QuotaStrategyManager
{
  public const string DefaultStrategyGUID = "YAPYAP.Default";
  public static string[] StrategyKeys => [DefaultStrategyGUID, .. _quotaStrategies.Keys];
  private static readonly Dictionary<string, QuotaStrategy> _quotaStrategies = [];

  internal static void Lock() => Locked = true;
  internal static BepInPlugin MetaInf = null!;
  private static bool Locked = false;

  /// <summary>
  /// Registers a new quota strategy for use<br /><br />
  /// Example: <example>RegisterStrategy("MyCoolStrategies", "VeryEasy", new(QuotaStrategyEasy.GetEasyQuota));</example>
  /// </summary>
  /// <remarks>Users will need to either copy/paste or manually type in the full namespaced strategy name from the available options in their config file so its best to keep the full name somewhat short</remarks>
  /// <param name="ownerGUID">A unique identifier for the source of this strategy</param>
  /// <param name="strategyName">An identifier for this specific strategy</param>
  /// <param name="strategy">The strategy to register</param>
  public static void RegisterStrategy(string ownerGUID, string strategyName, QuotaStrategy strategy)
  {
    var namespacedID = $"{ownerGUID}.{strategyName}";

    if (Locked)
    {
      QuotaQueenPlugin.Log.LogWarning($"Discarding strategy {namespacedID}, registration is too late. Registration must occur before a game is loaded");
      return;
    }

    if (_quotaStrategies.ContainsKey(namespacedID))
      QuotaQueenPlugin.Log.LogWarning($"{namespacedID} already exists in strategy dictionary, overwriting");

    //insert strategy into table
    _quotaStrategies[namespacedID] = strategy;

    //set up configs
    if (strategy.Configs is not null)
    {
      ConfigFile file = new(Path.Combine(Paths.ConfigPath, namespacedID + ".cfg"), true, MetaInf);
      foreach (var config in strategy.Configs)
        config.BindToConfig(file);

      strategy.ConfigFile = file;

      //If an in-game config menu ever appears this is where we
      //would register our 'bonus' configs
    }
  }

  internal static bool TryExecuteStrategy(string which, GameSnapshot context, out int nextQuota)
  {
    nextQuota = 0;

    if (!_quotaStrategies.TryGetValue(which, out var strategy))
      return false;

    nextQuota = strategy.ExecuteStrategy(context);
    return true;
  }
}
