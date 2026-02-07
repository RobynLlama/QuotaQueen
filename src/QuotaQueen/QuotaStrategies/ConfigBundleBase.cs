using BepInEx.Configuration;

namespace QuotaQueen.QuotaStrategies;

public abstract class ConfigBundleBase(ConfigDefinition def, ConfigDescription desc)
{
  public ConfigDefinition Definition = def;
  public ConfigDescription Description = desc;

  protected ConfigEntry<T> BindToConfigInternal<T>(ConfigFile file, T value) =>
    file.Bind(Definition, value, Description);

  public abstract void BindToConfig(ConfigFile file);
}
