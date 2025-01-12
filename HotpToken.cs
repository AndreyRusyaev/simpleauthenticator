namespace simpleauthenticator
{
    public struct HotpToken
    {
        public HotpToken(int tokenValue)
        {
            this.Value = tokenValue;
        }

        public int Value { get; }
    } 
}