namespace simpleauthenticator
{
    /// <summary>
    /// Implements TOTP tokens generation according RFC6238: https://www.rfc-editor.org/rfc/rfc6238.
    /// </summary>
    public sealed class Totp
    {
        public const int DefaultTimeStepInSeconds = 30;

        public const int DefaultTokenLength = 6;

        public const HmacAlgorithm DefaultHmacAlgorithm = HmacAlgorithm.Sha1;

        public static TotpToken Generate(
            byte[] secretKey,
            int tokenLength = DefaultTokenLength,
            HmacAlgorithm hmacAlgorithm = DefaultHmacAlgorithm)
        {
            return Generate(
                secretKey,
                DateTimeOffset.UtcNow - DateTimeOffset.UnixEpoch,
                DefaultTimeStepInSeconds,
                tokenLength,
                hmacAlgorithm);
        }

        public static TotpToken Generate(
            byte[] secretKey,
            DateTimeOffset initialDateTime,
            int tokenLength = DefaultTokenLength,
            HmacAlgorithm hmacAlgorithm = DefaultHmacAlgorithm)
        {
            return Generate(
                secretKey,
                initialDateTime.ToUniversalTime() - DateTimeOffset.UnixEpoch,
                DefaultTimeStepInSeconds,
                tokenLength,
                hmacAlgorithm);
        }

        public static TotpToken Generate(
            byte[] secretKey,
            TimeSpan timeSpan,
            int timeStepInSeconds = DefaultTimeStepInSeconds,
            int tokenLength = DefaultTokenLength,
            HmacAlgorithm hmacAlgorithm = DefaultHmacAlgorithm)
        {
            var timeSteps = (long)Math.Floor(timeSpan.TotalSeconds / timeStepInSeconds);
            var timeStepRemaining = (int)Math.Floor(timeSpan.TotalSeconds % timeStepInSeconds);
            
            var hotpToken = Hotp.Generate(secretKey, timeSteps, tokenLength, hmacAlgorithm);
            var lifeTime = TimeSpan.FromSeconds(timeStepInSeconds - timeStepRemaining);

            return new TotpToken(hotpToken.Value, lifeTime);
        }
    }
}
