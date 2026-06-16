namespace simpleauthenticator;

public readonly struct TotpToken(int tokenValue, TimeSpan lifeTime)
{
    public int Value { get; } = tokenValue;

    public TimeSpan LifeTime { get; } = lifeTime;
}