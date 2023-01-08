namespace simpleauthenticator
{
    public sealed class Totp
    {
        public const int DefaultUnixTimeOffsetInSeconds = 0;
        public const int DefaultTimeStepInSeconds = 30;

        public static int Generate(byte[] secretKey, int tokenLength)
        {
            return Generate(
                secretKey,
                DefaultUnixTimeOffsetInSeconds,
                DefaultTimeStepInSeconds,
                tokenLength);
        }

        public static int Generate(
            byte[] secretKey,
            int unixTimeOffsetInSeconds = DefaultUnixTimeOffsetInSeconds,
            int timeStepInSeconds = DefaultTimeStepInSeconds,
            int tokenLength = 6)
        {
            var currentUnixTimeInSeconds = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            var timeSteps = (long)Math.Floor((currentUnixTimeInSeconds - unixTimeOffsetInSeconds) / (decimal)timeStepInSeconds);

            return Hotp.Generate(secretKey, timeSteps, tokenLength);
        }
    }
}
