# simpleauthenticator
C# implementation to generate one time passwords for open authentication defined by standard RFC's:
* [HOTP: An HMAC-Based One-Time Password Algorithm](https://www.rfc-editor.org/rfc/rfc4226),
* [TOTP: Time-Based One-Time Password Algorithm](https://www.rfc-editor.org/rfc/rfc6238)

Compatible with Google/Microsoft Authenticator and other authenticators that supports RFC's.

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

