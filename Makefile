create-redis:
	helm install my-redis bitnami/redis --set auth.enabled=false

build-msproducts:
	docker build -t msproducts:latest -f MsProducts/Dockerfile .

restart-msproducts:
	kubectl rollout restart deployment ms-products -n netby-inventory	

dp: build-msproducts restart-msproducts

build-msauth:
	docker build -t msauth:latest -f MsAuth/Dockerfile .

build-msautharm:
	docker build -t msauth:latest -f MsAuth/Dockerfile --platform linux/arm64 .	

build-msproductsarm:
	docker build -t msproducts:latest -f MsProducts/Dockerfile --platform linux/arm64 .

build-mstransactionsarm:
	docker build -t mstransactions:latest -f MsTransactions/Dockerfile --platform linux/arm64 .

restart-msauth:
	kubectl rollout restart deployment ms-auth -n netby-inventory
	
da: build-msauth restart-msauth
	

build-mstransactions:
	docker build -t mstransactions:latest -f MsTransactions/Dockerfile .
restart-mstransactions:
	kubectl rollout restart deployment ms-transactions -n netby-inventory
dt: build-mstransactions restart-mstransactions

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



docker-push-all:
	docker tag msproducts:latest drkappspruebaregistry.azurecr.io/netby/msproducts:latest
	docker push drkappspruebaregistry.azurecr.io/netby/msproducts:latest
	docker tag msauth:latest drkappspruebaregistry.azurecr.io/netby/msauth:latest
	docker push drkappspruebaregistry.azurecr.io/netby/msauth:latest
	docker tag mstransactions:latest drkappspruebaregistry.azurecr.io/netby/mstransactions:latest
	docker push drkappspruebaregistry.azurecr.io/netby/mstransactions:latest