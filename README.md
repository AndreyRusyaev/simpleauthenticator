﻿# simpleauthenticator
Cross-platform C#/.Net implementation to generate one time passwords for open authentication defined by standard RFC's:
* [HOTP: An HMAC-Based One-Time Password Algorithm](https://www.rfc-editor.org/rfc/rfc4226),
* [TOTP: Time-Based One-Time Password Algorithm](https://www.rfc-editor.org/rfc/rfc6238)

Compatible with Google/Microsoft Authenticator and other authenticators that supports corresponding RFC's.

## How to run it

Ensure that .Net 8 or later is installed
```
dotnet --version
```

Clone repository and run `totp` or `hotp` command:
``` shell
git clone https://github.com/AndreyRusyaev/simpleauthenticator
cd simpleauthenticator
dotnet run totp -s "<your base32 encoded secret key>"
```

Output:
```shell
Token: 123456 (valid for 25 seconds).
```

# Prerequisites
.Net 8.0 or higher.

<details>
  <summary>HOWTO: Install .NET 8 on Windows, Linux, and macOS</summary>
  
### Windows
``` shell
# run in elevated shell
winget install Microsoft.DotNet.SDK.8
```

### Ubuntu
``` shell
# Register Microsoft packages feed (https://learn.microsoft.com/en-us/linux/packages)
wget https://packages.microsoft.com/config/ubuntu/$(lsb_release -rs)/packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
rm packages-microsoft-prod.deb

# installation
sudo apt update && sudo apt-get install -y dotnet-sdk-8.0
```

### MacOS
``` shell
brew install dotnet
```

### See also
[Install .NET on Windows, Linux, and macOS](https://learn.microsoft.com/en-us/dotnet/core/install/)

</details>

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
  -s, --secretkey, --secretkey-base32 <secretkey-base32>  Base32 encoded secret key (whitespaces allowed). Example:
                                                          'A5YS 2UP6 K4UF 46GD'.
  -s64, --secretkey-base64 <secretkey-base64>             Base64 encoded secret key (whitespaces allowed). Example:
                                                          'B3Et Uf5X KF54 ww=='.
  --token-length <token-length>                           Token length. Default: 6.

hotp Command Options:
  -s, --secretkey, --secretkey-base32 <secretkey-base32>  Base32 encoded secret key (whitespaces allowed). Example:
                                                          'A5YS 2UP6 K4UF 46GD'.
  -s64, --secretkey-base64 <secretkey-base64>             Base64 encoded secret key (whitespaces allowed, '=' can be omited). Example:
                                                          'B3Et Uf5X KF54 ww=='.
  -c, --counter <counter> (REQUIRED)                      8-byte counter value, the moving factor.  This counter
                                                          MUST be synchronized between the HOTP generator (client)
                                                          and the HOTP validator (server).
  --token-length <token-length>                           Token length. Default: 6.
```

# Examples

## TOTP: Generate time-based one time password
``` shell
git clone https://github.com/AndreyRusyaev/simpleauthenticator
cd simpleauthenticator
dotnet run totp --secretkey "A5YS 2UP6 K4UF 46GD"
```

OR

``` shell
git clone https://github.com/AndreyRusyaev/simpleauthenticator
cd simpleauthenticator
dotnet run totp --secretkey-base64 "B3Et Uf5X KF54 ww=="
```

Output:
``` shell
Token: 316788 (valid for 25 seconds).
```

## HOTP: Generate HMAC-based one time password

``` shell
git clone https://github.com/AndreyRusyaev/simpleauthenticator
cd simpleauthenticator
dotnet run hotp --counter 12345 --secretkey "A5YS 2UP6 K4UF 46GD"
```

OR

``` shell
git clone https://github.com/AndreyRusyaev/simpleauthenticator
cd simpleauthenticator
dotnet run hotp --counter 12345 --secretkey-base64 "B3Et Uf5X KF54 ww=="
```

Output:
``` shell
Token: 316788.
```

