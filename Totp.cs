namespace simpleauthenticator
{
    public sealed class Totp
    {
        public const int DefaultTimeStepInSeconds = 30;
        public const int DefaultTokenLength = 6;

        public static int Generate(byte[] secretKey, int tokenLength = DefaultTokenLength)
        {
            return Generate(
                secretKey,
                DefaultTimeStepInSeconds,
                tokenLength);
        }

        public static int Generate(
            byte[] secretKey,
            int timeStepInSeconds,
            int tokenLength = DefaultTokenLength)
        {
            return Generate(
                secretKey,
                DateTimeOffset.UnixEpoch,
                timeStepInSeconds,
                tokenLength);
        }

        public static int Generate(
            byte[] secretKey,
            DateTimeOffset initialDateTime,
            int timeStepInSeconds,
            int tokenLength = DefaultTokenLength)
        {
            return Generate(
                secretKey,
                DateTimeOffset.UtcNow,
                initialDateTime,
                timeStepInSeconds,
                tokenLength);
        }

        internal static int Generate(
            byte[] secretKey,
            DateTimeOffset currentDateTime,
            int tokenLength)
        {
            return Generate(
                secretKey,
                currentDateTime, 
                DateTimeOffset.UnixEpoch,
                DefaultTimeStepInSeconds, 
                tokenLength);
        }

        internal static int Generate(
            byte[] secretKey,
            DateTimeOffset currentDateTime,
            DateTimeOffset initialDateTime,
            int timeStepInSeconds,
            int tokenLength)
        {
            var currentUnixTimeInSeconds = currentDateTime.ToUnixTimeSeconds();
            var initialUnixTimeInSeconds = initialDateTime.ToUnixTimeSeconds();

            if (currentUnixTimeInSeconds < initialUnixTimeInSeconds)
            {
                throw new InvalidOperationException(
                    $"Current date '{currentDateTime}' is less than initial offset '{initialDateTime}'.");
            }

            var timeSteps = (long)Math.Floor((currentUnixTimeInSeconds - initialUnixTimeInSeconds) / (decimal)timeStepInSeconds);

            return Hotp.Generate(secretKey, timeSteps, tokenLength);
        }
    }
}
