namespace simpleauthenticator
{
    public struct TotpToken
    {
        public TotpToken(int tokenValue, TimeSpan lifeTime)
        {
            Value = tokenValue;
            LifeTime = lifeTime;
        }

        public int Value { get; }

        public TimeSpan LifeTime { get; }
    }
}