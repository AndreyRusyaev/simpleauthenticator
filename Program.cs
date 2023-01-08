using System.CommandLine;

namespace simpleauthenticator
{
    internal class Program
    {
        static int Main(string[] args)
        {
            RootCommand rootCommand = new RootCommand("Generates one time passwords (HOTP and TOTP tokens).")
            {
                CreateTotpCommand(),
                CreateHotpCommand()
            };

            return rootCommand.Invoke(args);
        }

        private static Command CreateTotpCommand()
        {
            var totpCommand = new Command(
                "totp",
                "Generates time based one time password (TOTP token, RFC 6238).");

            var secretKeyInBase64Option = new Option<string?>(
              name: "--secretkey-base64",
              description: "Base64 encoded secret key.");

            totpCommand.AddOption(secretKeyInBase64Option);

            var secretKeyInBase32Option = new Option<string?>(
              name: "--secretkey-base32",
              description: "Base32 encoded secret key.");

            totpCommand.AddOption(secretKeyInBase32Option);

            var tokenLengthOption = new Option<int?>(
              name: "--token-length",
              description: "Token length. Default: 6.");

            totpCommand.AddOption(tokenLengthOption);

            totpCommand.SetHandler(
                (base32EncodedSecretKey, base64EncodedSecretKey, tokenLength) => 
                {
                    GenerateTotp(base32EncodedSecretKey, base64EncodedSecretKey, tokenLength);
                },
                secretKeyInBase32Option,
                secretKeyInBase64Option,
                tokenLengthOption);

            return totpCommand;
        }

        private static Command CreateHotpCommand()
        {
            var command = new Command(
                "hotp",
                "Generates hmac based one time password (HOTP token, RFC 4226).");

            var secretKeyInBase64Option = new Option<string?>(
              name: "--secretkey-base64",
              description: "Base64 encoded secret key.");

            command.AddOption(secretKeyInBase64Option);

            var secretKeyInBase32Option = new Option<string?>(
              name: "--secretkey-base32",
              description: "Base32 encoded secret key.");

            command.AddOption(secretKeyInBase32Option);

            var counterOption = new Option<long>(
              name: "--counter",
              description: @"8-byte counter value, the moving factor.  This counter MUST be synchronized between the HOTP generator (client) and the HOTP validator (server).");

            command.AddOption(counterOption);

            var tokenLengthOption = new Option<int?>(
              name: "--token-length",
              description: "Token length. Default: 6.");

            command.AddOption(tokenLengthOption);

            command.SetHandler(
                (base32EncodedSecretKey, base64EncodedSecretKey, counter, tokenLength) =>
                {
                    GenerateHotp(base32EncodedSecretKey, base64EncodedSecretKey, counter, tokenLength);
                },
                secretKeyInBase32Option,
                secretKeyInBase64Option,
                counterOption,
                tokenLengthOption);

            return command;
        }

        private static int GenerateTotp(
            string? base32EncodedSecretKey,
            string? base64EncodedSecretKey,
            int? tokenLength)
        {
            byte[] secretKey;

            if (base64EncodedSecretKey != null)
            {
                try
                {
                    secretKey =
                        BaseEncodings.FromBase64String(base64EncodedSecretKey);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Failed to decode secret key from base64: {0}", ex);
                    return -1;
                }
            }
            else if (base32EncodedSecretKey != null)
            {
                try
                {
                    secretKey =
                        BaseEncodings.FromBase32String(base32EncodedSecretKey);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Failed to decode secret key from base32: {0}.", ex);
                    return -1;
                }
            }
            else
            {
                Console.WriteLine("Secret key is not specified.");
                return -1;
            }

            var token = Totp.Generate(secretKey, tokenLength ?? 6);

            Console.WriteLine("Token: {0}.", token);
            return 0;
        }

        private static int GenerateHotp(
            string? base32EncodedSecretKey,
            string? base64EncodedSecretKey,
            long counter,
            int? tokenLength)
        {
            byte[] secretKey;

            if (base64EncodedSecretKey != null)
            {
                try
                {
                    secretKey =
                        BaseEncodings.FromBase64String(base64EncodedSecretKey);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Failed to decode secret key from base64: {0}", ex);
                    return -1;
                }
            }
            else if (base32EncodedSecretKey != null)
            {
                try
                {
                    secretKey =
                        BaseEncodings.FromBase32String(base32EncodedSecretKey);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Failed to decode secret key from base32: {0}.", ex);
                    return -1;
                }
            }
            else
            {
                Console.WriteLine("Secret key is not specified.");
                return -1;
            }

            var token = Hotp.Generate(secretKey, counter, tokenLength ?? 6);

            Console.WriteLine("Token: {0}.", token);
            return 0;
        }
    }
}