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

## Docker Desktop settings

![Docker Desktop WSL settings](/images/docker-desktop-wsl-settings.png)

## Links
- https://woshub.com/move-wsl-another-drive-windows/
