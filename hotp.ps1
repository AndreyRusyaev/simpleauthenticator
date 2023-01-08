Param (
	[parameter(Mandatory=$true, ParameterSetName="WithBase32")]
	[string] 
	$SecretKeyBase32,
	[parameter(Mandatory=$true, ParameterSetName="WithBase64")]
	[string] 
	$SecretKeyBase64,
	[parameter(Mandatory=$true)]
	[long]
	$Counter,
	[int]
	$TokenLength = 6
)

Set-StrictMode -version Latest
Set $ErrorActionPreference='Stop'

if ($SecretKeyBase32)
{
	dotnet run hotp --secretkey-base32 "$SecretKeyBase32" --counter $Counter --token-length $TokenLength
} 
elseif ($SecretKeyBase64)
{
	dotnet run hotp --secretkey-base64 "$SecretKeyBase64" --counter $Counter --token-length $TokenLength
}
else
{
	Write-Error "Secret key is not specified."
}