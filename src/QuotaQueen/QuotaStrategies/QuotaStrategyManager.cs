using System;
using System.Collections.Generic;

namespace QuotaQueen.QuotaStrategies;

/// <summary>
/// This class is for managing the use and registration of quota strategies
/// that modify the difficulty of quota scaling
/// </summary>
public static class QuotaStrategyManager
{
  /// <summary>
  /// The GUID for the default game behavior
  /// </summary>
  public const string DefaultStrategyGUID = "YAPYAP.Default";

  /// <summary>
  /// A listing of all strategy GUIDs including the default
  /// </summary>
  public static string[] StrategyKeys => [DefaultStrategyGUID, .. _quotaStrategies.Keys];
  private static readonly Dictionary<string, Func<GameSnapshot, int>> _quotaStrategies = [];

  internal static void Lock() => Locked = true;
  private static bool Locked = false;

  /// <summary>
  /// Registers a new quota strategy for use<br /><br />
  /// Example: <example>RegisterStrategy("MyCoolStrategies", "VeryEasy", new(QuotaStrategyEasy.GetEasyQuota));</example>
  /// </summary>
  /// <remarks>Users will need to either copy/paste or manually type in the full namespaced strategy name from the available options in their config file so its best to keep the full name somewhat short</remarks>
  /// <param name="ownerGUID">A unique identifier for the source of this strategy</param>
  /// <param name="strategyName">An identifier for this specific strategy</param>
  /// <param name="strategy">The strategy to register</param>
  public static void RegisterStrategy(string ownerGUID, string strategyName, Func<GameSnapshot, int> strategy)
  {
    var namespacedID = $"{ownerGUID}.{strategyName}";

    if (Locked)
    {
      QuotaQueenPlugin.Log.LogWarning($"Discarding strategy {namespacedID}, registration is too late. Registration must occur before a game is loaded");
      return;
    }

    if (_quotaStrategies.ContainsKey(namespacedID))
      QuotaQueenPlugin.Log.LogWarning($"{namespacedID} already exists in strategy dictionary, overwriting");

    _quotaStrategies[namespacedID] = strategy;
  }

  internal static bool TryExecuteStrategy(string which, GameSnapshot context, out int nextQuota)
  {
    nextQuota = 0;

    if (!_quotaStrategies.TryGetValue(which, out var strategy))
      return false;

    nextQuota = strategy(context);
    return true;
  }
}
