using System;
using System.Linq;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using QuotaQueen.Configuration;
using QuotaQueen.Patches;

namespace QuotaQueen;

[BepInAutoPlugin]
public partial class QuotaQueenPlugin : BaseUnityPlugin
{
  internal static QuotaQueenPlugin Instance = null!;
  internal static ManualLogSource Log { get; private set; } = null!;
  internal static QuotaQueenConfig QueenConfig { get; private set; } = null!;

  private void Awake()
  {
    Log = Logger;

    Log.LogInfo($"Plugin {Name} is loading!");

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

    Harmony patcher = new(Id);
    patcher.PatchAll(typeof(GameManagerPatches));

    Log.LogMessage($"Applied {patcher.GetPatchedMethods().Count()} patches");
  }
}
