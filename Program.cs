using simpleauthenticator;

RootCommand rootCommand = new("Generates one time passwords (HOTP and TOTP tokens).")
{
    CreateTotpCommand(),
    CreateHotpCommand()
};

return rootCommand.Parse(args).Invoke();

Command CreateTotpCommand()
{
    Option<string?> secretKeyInBase32Option = new("--secretkey-base32", "--secretkey", "-s")
    {
        Description = "Base32 encoded secret key (whitespaces allowed). Example: 'A5YS 2UP6 K4UF 46GD'."
    };

    Option<string?> secretKeyInBase64Option = new("--secretkey-base64", "--secretkey64", "-s64")
    {
        Description = "Base64 encoded secret key (whitespaces allowed). Example: 'B3Et Uf5X KF54 ww=='."
    };

    Option<int?> tokenLengthOption = new("--token-length", "-l")
    {
        Description = "Token length. Default: 6."
    };

    Option<HmacAlgorithm?> hmacAlgorithmOption = new("--hmac", "-m")
    {
        Description = "HMAC algorithm to use. Default: SHA1. Allowed values: Md5, Sha1, Sha2_256, Sha2_512, Sha3_256, Sha3_512."
    };

    var command = new Command("totp", "Generates time based one time password (TOTP token, RFC 6238).")
    {
        secretKeyInBase32Option,
        secretKeyInBase64Option,
        tokenLengthOption,
        hmacAlgorithmOption
    };

    command.SetAction(
        parseResult =>
        {
            return GenerateTotp(
                parseResult.GetValue(secretKeyInBase32Option),
                parseResult.GetValue(secretKeyInBase64Option),
                parseResult.GetValue(tokenLengthOption),
                parseResult.GetValue(hmacAlgorithmOption));
        });

    return command;
}

Command CreateHotpCommand()
{
    Option<string?> secretKeyInBase32Option = new("--secretkey-base32", "--secretkey", "-s")
    {
        Description = "Base32 encoded secret key (whitespaces allowed). Example: 'A5YS 2UP6 K4UF 46GD'."
    };

    Option<string?> secretKeyInBase64Option = new("--secretkey-base64", "--secretkey64", "-s64")
    {
        Description = "Base64 encoded secret key (whitespaces allowed). Example: 'B3Et Uf5X KF54 ww=='."
    };

    Option<long> counterOption = new("--counter", "-c")
    {
        Description = @"8-byte counter value, the moving factor.  This counter MUST be synchronized between the HOTP generator (client) and the HOTP validator (server).",
        Required = true
    };

    Option<int?> tokenLengthOption = new("--token-length", "-l")
    {
        Description = "Token length. Default: 6."
    };

    Option<HmacAlgorithm?> hmacAlgorithmOption = new("--hmac", "-m")
    {
        Description = "HMAC algorithm to use. Default: SHA1. Allowed values: Md5, Sha1, Sha2_256, Sha2_512, Sha3_256, Sha3_512."
    };

    var command = new Command("hotp", "Generates hmac based one time password (HOTP token, RFC 4226).")
    {
        secretKeyInBase32Option,
        secretKeyInBase64Option,
        counterOption,
        tokenLengthOption,
        hmacAlgorithmOption
    };

    command.SetAction(
        parseResult =>
        {
            return GenerateHotp(
                parseResult.GetValue(secretKeyInBase32Option),
                parseResult.GetValue(secretKeyInBase64Option),
                parseResult.GetRequiredValue(counterOption),
                parseResult.GetValue(tokenLengthOption),
                parseResult.GetValue(hmacAlgorithmOption));
        });

    return command;
}

int GenerateTotp(
    string? base32EncodedSecretKey,
    string? base64EncodedSecretKey,
    int? tokenLength,
    HmacAlgorithm? hmacAlgorithm)
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
            Console.Error.WriteLine("Failed to decode secret key from base64: {0}", ex);
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
            Console.Error.WriteLine("Failed to decode secret key from base32: {0}.", ex);
            return -1;
        }
    }
    else
    {
        Console.Error.WriteLine("Secret key is not specified.");
        return -1;
    }

    var result = Totp.Generate(secretKey, tokenLength ?? 6, hmacAlgorithm ?? HmacAlgorithm.Sha1);

    Console.WriteLine(
        "Token: {0} (valid for {1} {2}).",
        result.Value.ToString("d" + (tokenLength ?? 6).ToString()),
        result.LifeTime.TotalSeconds,
        result.LifeTime.TotalSeconds > 1 ? "seconds" : "second");

    return 0;
}

int GenerateHotp(
    string? base32EncodedSecretKey,
    string? base64EncodedSecretKey,
    long counter,
    int? tokenLength,
    HmacAlgorithm? hmacAlgorithm)
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
            Console.Error.WriteLine("Failed to decode secret key from base64: {0}", ex);
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
            Console.Error.WriteLine("Failed to decode secret key from base32: {0}.", ex);
            return -1;
        }
    }
    else
    {
        Console.Error.WriteLine("Secret key is not specified.");
        return -1;
    }

    var token = Hotp.Generate(secretKey, counter, tokenLength ?? 6, hmacAlgorithm ?? HmacAlgorithm.Sha1);

    Console.WriteLine("Token: {0}.", token.Value);
    return 0;
}
