public interface IConfigInitializable
{
    void Initialize(ConfigManager Config, bool IsUpdate = true);
}