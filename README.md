# KubernetesLearning

## Idea for project using k8s

App with some form that saves data in MSSQL database.
Simple form - user profile.

k8s resources
- service, deployment, pod - run the service + DB pod
- persistent volume - database
- config map - app configuration
- secret - app sensitive data e.g. connection string, password to DB
- cronJob - job to get current currency value and add it to DB **[???]**

## Docker commands

```
cd "D:\Kubernetes\KubernetesLearning\KubernetesTestApp"
docker build -t prasol/kubernetestestapp .
docker push prasol/kubernetestestapp:latest
```

## Docker Desktop settings

![Docker Desktop WSL settings](/images/docker-desktop-wsl-settings.png)

## Links
- https://woshub.com/move-wsl-another-drive-windows/
- https://docs.docker.com/get-started/introduction/build-and-push-first-image/#build-and-push-the-image
- https://medium.com/@iskandre/explanation-of-ports-in-kubernetes-service-a29490515fb2
