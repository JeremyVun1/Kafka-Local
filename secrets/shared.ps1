$OU = "TestUnit"
$O = "TestOrg"
$L = "Sydney"
$ST = "NSW"
$C = "AU"
$Password = "Password01"

$CaName = "root"
$CaPublicDir = "$CaName-public"
$CaPrivateDir = "$CaName-private"

Function AssertCommand($cmdName)
{
	if (-not (Get-Command $cmdName -ErrorAction SilentlyContinue))
	{
		Write-Host "$cmdName required"
		exit 1
	}
}

Function CreateDir($dirName)
{
	Remove-Item $dirName -Force -Recurse -ErrorAction SilentlyContinue
	New-Item $dirName -ItemType Directory > $null
}

Function CreateRootCerts()
{
	CreateDir $CaPublicDir
	CreateDir $CaPrivateDir

	openssl req -new -newkey rsa:2048 -x509 -subj "/CN=$component/OU=$OU/O=$O/L=$L/ST=$ST/C=$C" -passout pass:$Password -keyout "$CaPrivateDir/$CaName.key" -out "$CaPublicDir/$CaName.crt"

	keytool -noprompt -keystore "$CaPublicDir/truststore.jks" -alias CARoot -import -file "$CaPublicDir/root.crt" -storepass $Password -keypass $Password
}

Function CreateKafkaCerts($component)
{
	CreateDir $component

	Write-Host "[$component] Generating Key"
	openssl genrsa -out "$component/$component.unencrypted.key" 2048
	openssl pkcs8 -in "$component/$component.unencrypted.key" -out "$component/$component.key" -topk8 -passout pass:$Password

	Write-Host "[$component] Generating CSR"
	openssl req -new -key "$component/$component.key" -out "$component/$component.csr" -subj "/CN=$component/OU=$OU/O=$O/L=$L/ST=$ST/C=$C" -passin pass:$Password

	Write-Host "[$component] Generating CRT"
	openssl x509 -req -CA "$CaPublicDir/$CaName.crt" -CAkey "$CaPrivateDir/$CaName.key" -in "$component/$component.csr" -out "$component/$component.crt" -CAcreateserial -passin pass:$Password

	Write-Host "[$component] Generating p12 keystore"
	openssl pkcs12 -export -in "$component/$component.crt" -inkey "$component/$component.key" -out "$component/$component.p12" -passin pass:$Password -passout pass:$Password -name $component -CAfile "$CaPublicDir/$CaName.crt" -caname $CaName -chain

	echo "[$component] Packing JKS Keystore"
	keytool -importkeystore -srckeystore "$component/$component.p12" -srcstoretype pkcs12 -srcstorepass $Password -destkeystore "$component/$component.jks" -deststorepass $Password -keypass $Password

	Write-Host "[$component] Cleaning up"
	Remove-Item "$component/$component.unencrypted.key"
	Remove-Item "$component/$component.csr"

	Pop-Location
}
