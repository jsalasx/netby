create-redis:
	helm repo add bitnami https://charts.bitnami.com/bitnami
	helm install my-redis bitnami/redis

