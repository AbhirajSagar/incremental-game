public interface IConfigurable
{
    void ApplyConfig(GameConfig config);
}

public interface ISessionBindable
{
    void Bind(GameSession session);
    void Unbind(GameSession session);
}