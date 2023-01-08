Param (
	[parameter(Mandatory=$true, ParameterSetName="WithBase32")]
	[string] 
	$SecretKeyBase32,
	[parameter(Mandatory=$true, ParameterSetName="WithBase64")]
	[string] 
	$SecretKeyBase64,
	[int]
	$TokenLength = 6
)

Set-StrictMode -version Latest
Set $ErrorActionPreference='Stop'

if ($SecretKeyBase32)
{
	dotnet run totp --secretkey-base32 "$SecretKeyBase32" --token-length $TokenLength
} 
elseif ($SecretKeyBase64)
{
	dotnet run totp --secretkey-base64 "$SecretKeyBase64" --token-length $TokenLength
}
else
{
	Write-Error "Secret key is not specified."
}