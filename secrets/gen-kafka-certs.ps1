. "$pwd/shared.ps1"

AssertCommand "openssl"
AssertCommand "keytool"

CreateRootCerts

$Components = @("zookeeper", "broker", "schema-registry", "control-center")
foreach ($component in $components)
{
	CreateKafkaCerts $component
}

