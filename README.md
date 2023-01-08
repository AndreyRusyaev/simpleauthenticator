# simpleauthenticator
C# implementation to generate one time passwords for open authentication defined by standard RFC's:
* [HOTP: An HMAC-Based One-Time Password Algorithm](https://www.rfc-editor.org/rfc/rfc4226),
* [TOTP: Time-Based One-Time Password Algorithm](https://www.rfc-editor.org/rfc/rfc6238)

Compatible with Google/Microsoft Authenticator and other authenticators that supports RFC's.

# Usage
```
Description:
  Generates one time passwords (HOTP and TOTP tokens).

Usage:
  simpleauthenticator [command] [options]

Options:
  --version       Show version information
  -?, -h, --help  Show help and usage information

Commands:
  totp  Generates time based one time password (TOTP token, RFC 6238).
  hotp  Generates hmac based one time password (HOTP token, RFC 4226).

totp Command Options:
  --secretkey-base64 <secretkey-base64>  Base64 encoded secret key.
  --secretkey-base32 <secretkey-base32>  Base32 encoded secret key.
  --token-length <token-length>          Token length. Default: 6.

hotp Command Options:
  --secretkey-base64 <secretkey-base64>  Base64 encoded secret key.
  --secretkey-base32 <secretkey-base32>  Base32 encoded secret key.
  --counter <counter>                    8-byte counter value, the moving factor. This counter MUST be synchronized between the HOTP generator (client) and the HOTP validator (server).
  --token-length <token-length>          Token length. Default: 6.
```

# Examples

## TOTP: Generate Time-Based One-Time Password
```shell
git clone https://github.com/AndreyRusyaev/simpleauthenticator
cd simpleauthenticator
dotnet run totp --secretkey-base32 "C3UWGR2FYYAADZMGGWEWLEHDET6SPMRSKC6IXBPHQDONMJFOYYBQ===="
```

OR

```shell
git clone https://github.com/AndreyRusyaev/simpleauthenticator
cd simpleauthenticator
dotnet run totp --secretkey-base64 "CjysLofv+76qUsybRGtj2EbQ3BugswvAIwIPBJyhXJ0="
```

Output:
```shell
Token: 316788.
```

## HOTP: Generate HMAC-Based One-Time Password

```shell
git clone https://github.com/AndreyRusyaev/simpleauthenticator
cd simpleauthenticator
dotnet run hotp --counter 12345 --secretkey-base32 "C3UWGR2FYYAADZMGGWEWLEHDET6SPMRSKC6IXBPHQDONMJFOYYBQ===="
```

OR

```shell
git clone https://github.com/AndreyRusyaev/simpleauthenticator
cd simpleauthenticator
dotnet run hotp --counter 12345 --secretkey-base64 "CjysLofv+76qUsybRGtj2EbQ3BugswvAIwIPBJyhXJ0="
```

Output:
```shell
Token: 316788.
```

