namespace simpleauthenticator;

public readonly struct HotpToken(int tokenValue)
{
    public int Value { get; } = tokenValue;
} 