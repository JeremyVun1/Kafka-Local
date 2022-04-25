OU="TestUnit"
O="TestOrg"
L="Sydney"
ST="NSW"
C="AU"
PASSWORD="Password01"

export MSYS_NO_PATHCONV=1
component=$1

rm -rf ${component}
mkdir ${component}
cd ${component}

echo "[${component}] Generating Key"
openssl genrsa -passout pass:$PASSWORD -out ${component}.pem 2048

openssl pkcs8 -in ${component}.pem -out ${component}.key -topk8 -passin pass:$PASSWORD -passout pass:$PASSWORD

echo "[${component}] Generating CSR"
openssl req -new -key ${component}.key -out ${component}.csr -subj "/CN=${component}/OU=${OU}/O=${O}/L=${L}/ST=${ST}/C=${C}" -passin pass:$PASSWORD

echo "[${component}] Generating CRT"
openssl x509 -req -CA ../ca-public/root.crt -CAkey ../ca-private/root.key -in ${component}.csr -out ${component}.crt -CAcreateserial -passin pass:$PASSWORD

#openssl req -new -newkey rsa:4096 -x509 -subj "/CN=root/OU=${OU}/O=${O}/L=${L}/ST=${ST}/C=${C}" -passin pass:$PASSWORD -passout pass:$PASSWORD -keyout root.key -out root.crt -outform PEM

#echo "[${component}] Generating key"
#openssl genrsa -passout pass:$PASSWORD -out ${component}.key 4096

#echo "[${component}] Signing Key"
#openssl req -new -key ${component}.key -out ${component}.csr -subj "/CN=${component}/OU=${OU}/O=${O}/L=${L}/ST=${ST}/C=${C}" -passout pass:$PASSWORD
#openssl x509 -req -CA root.crt -CAkey root.key -in ${component}.csr -out ${component}.crt -CAcreateserial -passin pass:$PASSWORD

echo "[${component}] Generating JKS"
openssl pkcs12 -inkey ${component}.key -in ${component}.crt -export -out ${component}.pkcs12 -passin pass:$PASSWORD -password pass:$PASSWORD -name ${component}

keytool -importkeystore -srckeystore ${component}.pkcs12 -srcstoretype pkcs12 -srcstorepass $PASSWORD -destkeystore ${component}.jks -deststorepass $PASSWORD -keypass $PASSWORD

keytool -noprompt -keystore ${component}.jks -alias CARoot -import -file ../ca-public/root.crt -storepass $PASSWORD -keypass $PASSWORD

export MSYS_NO_PATHCONV=0

echo Press Any Key
read tmp