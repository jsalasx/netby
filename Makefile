create-redis:
	helm repo add bitnami https://charts.bitnami.com/bitnami
	helm install my-redis bitnami/redis --set auth.enabled=false

build-msproducts:
	docker build -t msproducts:latest -f MsProducts/Dockerfile .

build-mstransactions:
	docker build -t mstransactions:latest -f MsTransactions/Dockerfile .

helm-install:
	helm install netby-inventory ./netby-inventory --namespace netby-inventory --create-namespace

helm-uninstall:
	helm uninstall netby-inventory --namespace netby-inventory

helm-upgrade:
	helm upgrade netby-inventory ./netby-inventory --namespace netby-inventory

kong-install:
	helm repo add kong https://charts.konghq.com
	helm repo update
	helm install kong kong/ingress -n kong --create-namespace