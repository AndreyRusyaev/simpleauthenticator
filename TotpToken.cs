public struct TotpToken
{
    public TotpToken(int token, TimeSpan lifeTime)
    {
        Token = token;
        LifeTime = lifeTime;
    }

    public int Token { get; }

    public TimeSpan LifeTime { get; }
}