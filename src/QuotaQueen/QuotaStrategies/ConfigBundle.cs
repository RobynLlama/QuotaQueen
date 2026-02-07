using BepInEx.Configuration;

namespace QuotaQueen.QuotaStrategies;

public class ConfigBundle<TConfig>(ConfigDescription desc, ConfigDefinition def, TConfig defValue) : ConfigBundleBase(def, desc)
{
  public TConfig DefaultValue = defValue;
  public TConfig Value => BoundConfig is not null ? BoundConfig.Value : DefaultValue;
  protected ConfigEntry<TConfig>? BoundConfig = null;
  public override void BindToConfig(ConfigFile file) =>
    BoundConfig = BindToConfigInternal(file, DefaultValue);
}
