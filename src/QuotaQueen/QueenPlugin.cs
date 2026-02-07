using System;
using System.Linq;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using QuotaQueen.Configuration;
using QuotaQueen.Patches;
using QuotaQueen.QuotaStrategies;
using UnityEngine.SceneManagement;

namespace QuotaQueen;

/// <summary>
/// Plugin
/// </summary>
[BepInAutoPlugin]
public partial class QuotaQueenPlugin : BaseUnityPlugin
{
  internal static ManualLogSource Log { get; private set; } = null!;
  internal static QuotaQueenConfig QueenConfig { get; private set; } = null!;

  private bool MainMenuSeen = false;

  private void Awake()
  {
    Log = Logger;

    Log.LogInfo($"Quota Queen startup");

    SceneManager.sceneLoaded += OnSceneChange;

    Harmony patcher = new(Id);
    patcher.PatchAll(typeof(GameManagerPatches));

    Log.LogMessage($"Applied {patcher.GetPatchedMethods().Count()} patches");

    QuotaStrategyManager.RegisterStrategy("QuotaQueen", "VeryEasy", new(QuotaStrategyEasy.GetEasyQuota));
    QuotaStrategyManager.RegisterStrategy("QuotaQueen", "ConfigurableQuota", new(QuotaStrategyConfigurable.GetQuota));

    QuotaStrategyConfigurable.EarlyBind();
  }

  private void OnSceneChange(Scene arg0, LoadSceneMode arg1)
  {
    if (MainMenuSeen)
      return;

    if (arg0.name.Equals("menu", StringComparison.OrdinalIgnoreCase))
    {
      FreshenConfig();
      MainMenuSeen = true;
    }
  }

  internal void FreshenConfig()
  {
    if (QueenConfig is null)
    {
      try
      {
        QueenConfig = QuotaQueenConfig.BindConfig(Info.Metadata);
        Log.LogMessage($"QuotaQueen Configuration loaded\n\n{QueenConfig}");
      }
      catch (Exception ex)
      {
        Log.LogError($"Failed to bind config file!\n\n{ex.StackTrace}\n\n{ex.Message}");
        return;
      }
    }
  }
}
