using BepInEx.Configuration;

namespace QuotaQueen.QuotaStrategies;

/// <summary>
/// Encapsulates a Quota Strategy and its relevant configs for use
/// with the strategy manager
/// </summary>
/// <param name="strategy">A reference to the method to call for this strategy</param>
/// <param name="configs">(Optional) A list of config definitions to be managed by Quota Queen</param>
public class QuotaStrategy(UpdateQuotaDelegate strategy, ConfigBundleBase[]? configs = null)
{
  public readonly UpdateQuotaDelegate ExecuteStrategy = strategy;
  public readonly ConfigBundleBase[]? Configs = configs;
  public ConfigFile? ConfigFile;
}
