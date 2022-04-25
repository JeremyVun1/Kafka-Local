OU="TestUnit"
O="TestOrg"
L="Sydney"
ST="NSW"
C="AU"
PASSWORD="Password01"

# Generate CA key and crt
export MSYS_NO_PATHCONV=1

rm -rf ca-private
mkdir ca-private
cd ca-private
openssl req -new -newkey rsa:2048 -x509 -subj "/CN=root/OU=${OU}/O=${O}/L=${L}/ST=${ST}/C=${C}" -passin pass:$PASSWORD -passout pass:$PASSWORD -keyout root.key -out root.crt

# Create truststore and import the CA cert.
cd ..
rm -rf ca-public
mkdir ca-public
cd ca-public
mv ../ca-private/root.crt .

keytool -noprompt \
	-keystore truststore.jks \
	-alias CARoot \
	-import -file root.crt \
	-storepass $PASSWORD \
	-keypass $PASSWORD

# Generate jks for each component
components=("broker" "schema-registry" "control-center" "rest-proxy" "admin-client")
for component in ${components[@]}; do
	cd ..
	rm -rf ${component}
	mkdir ${component}
	cd ${component}

	echo "[${component}] Generating key"
	openssl genrsa -passout pass:$PASSWORD -out ${component}.pem 2048

	openssl pkcs8 -in ${component}.pem -out ${component}.key -topk8 -passin pass:$PASSWORD -passout pass:$PASSWORD
	#openssl genrsa -passout pass:$PASSWORD -out ${component}.key 2048

	echo "[${component}] Generating CSR"
	openssl req -new -key ${component}.key -out ${component}.csr -subj "/CN=${component}/OU=${OU}/O=${O}/L=${L}/ST=${ST}/C=${C}" -passin pass:$PASSWORD -passout pass:$PASSWORD

	echo "[${component}] Generating CRT"
	openssl x509 -req -CA ../ca-public/root.crt -CAkey ../ca-private/root.key -in ${component}.csr -out ${component}.crt -CAcreateserial -passin pass:$PASSWORD

	echo "[${component}] Creating JKS"
	openssl pkcs12 -inkey ${component}.key -in ${component}.crt -export -out ${component}.pkcs12 -passin pass:$PASSWORD -password pass:$PASSWORD -name ${component}

	keytool -importkeystore -srckeystore ${component}.pkcs12 -srcstoretype pkcs12 -srcstorepass $PASSWORD -destkeystore ${component}.jks -deststorepass $PASSWORD -keypass $PASSWORD

	keytool -noprompt -keystore ${component}.jks -alias CARoot -import -file ../ca-public/root.crt -storepass $PASSWORD -keypass $PASSWORD
done

export MSYS_NO_PATHCONV=0

echo Finished. Press Enter
read tmp