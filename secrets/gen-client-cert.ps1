param (
	[string] $component
)

. "$pwd/shared.ps1"

AssertCommand "openssl"
AssertCommand "keytool"
CreateKafkaCerts $component
