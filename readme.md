# Local Kafka

## Quick start
1. `secrets/gen-kafka.sh` to create ssl certs for kafka services
2. `docker-compose up` to start kafka cluster
3. `secrets/gen-client.sh <client_name>` to create ssl certs for kafka clients
4. Configure client SSL apssettings.json configs
5. `dotnet run` for clients/consumer and clients/producer

## Kafka Cluster Ports
See `.env`

## ACL's
Managed by Kafka Security Manager as part of the docker setup. ACL policies hot reloaded from `acls/acls.csv`

See: https://github.com/conduktor/kafka-security-manager

## Examples
### Consumer
.NET Webjob Consumer

### Producer
.NET Api Kafka Producer

### AdminClient
.NET Api for managing topics