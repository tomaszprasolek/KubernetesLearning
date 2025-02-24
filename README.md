# KubernetesLearning

<!-- TOC start (generated with https://github.com/derlin/bitdowntoc) -->

- [Idea for project using k8s](#idea-for-project-using-k8s)
- [How to run environment and project](#how-to-run-environment-and-project)
   * [Prepare k8s environment - minukube](#prepare-k8s-environment---minukube)
   * [Sample project](#sample-project)
   * [Create namespace](#create-namespace)
   * [Prepare secret(s)](#prepare-secrets)
   * [Create database](#create-database)
   * [Deploy the config map (application configuration)**](#deploy-the-config-map-application-configuration)
   * [Deploy the application](#deploy-the-application)
   * [Test the application](#test-the-application)
- [How to debug common issues](#how-to-debug-common-issues)
   * [Problem](#problem)
   * [Debugging Steps](#debugging-steps)
   * [Key Takeaways](#key-takeaways)
- [Docker commands](#docker-commands)
- [Docker Desktop settings](#docker-desktop-settings)
- [Links](#links)

<!-- TOC end -->

## Idea for project using k8s

App with some form that saves data in MSSQL database.
Simple form - user profile.

k8s resources
- service, deployment, pod - run the service + DB pod
- persistent volume - database
- config map - app configuration
- secret - app sensitive data e.g. connection string, password to DB
- cronJob - job to get current currency value and add it to DB **[???]**

## How to run environment and project

### Prepare k8s environment - minukube

I use [minikube](https://minikube.sigs.k8s.io/docs/) to run a k8s cluster locally on my computer. It requires a container runtime or virtual machine - in this case, I use [Docker](https://www.docker.com).

To start the k8s cluster, run:
```
minikube start
```
After the cluster starts, you will see a new running container in your Docker Desktop application.
![Docker desktop with minikube](/images/docker-minikube-container.png)

### Sample project
Navigate to the [Deployment](https://github.com/tomaszprasolek/KubernetesLearning/tree/master/Deployment) folder, which contains all the necessary `.yaml` configuration files:
```
Namespace.yaml
Mssql_Secret.yaml
Mssql_StatefulSet.yaml
Deployment.yaml
```
### Create namespace
```
kubectl apply -f .\Namespace.yaml
kubens tomo-app
```
The first command creates the namespace, while the second changes the active namespace in kubectl. Note: [Kubens](https://github.com/ahmetb/kubectx) is an additional tool (not built-in) that I installed to quickly switch between contexts and namespaces in k8s.

---

### Prepare secret(s)
```
kubectl apply -f .\Mssql_Secret.yaml
```
Expected response in CLI: `secret/mssql created`

---

### Create database
```
kubectl apply -f .\Mssql_StatefulSet.yaml
```
Expected response in terminal:
```
statefulset.apps/mssql created
service/mssql-service created
```
To verify if the database is running, use:
```
kubectl get statefulset
```
You should see output similar to:
```
NAME    READY   AGE
mssql   1/1     56s
```

---
### Deploy the config map (application configuration)

```
kubectl apply -f .\ConfigMap.yaml
```
Expected response in CLI: `configmap/cm-app-config created`

---
### Deploy the application

```
kubectl apply -f .\Deployment.yaml
```
Expected response in terminal:
```
deployment.apps/kubernetestestapp created
service/tomo-app-service created
```
To verify if the application is running, use:
```
kubectl get pod
```
Expected output:
```
NAME                                 READY   STATUS    RESTARTS   AGE
kubernetestestapp-744fd9ffd4-ttvpj   1/1     Running   0          16s
mssql-0                              1/1     Running   0          103s
```
`kubernetestestapp-744fd9ffd4-ttvpj` is our application pod with status `Running`.  
`mssql-0` is our database pod, also with status `Running`. Now we can verify if everything works properly.

---
### Test the application

Since the application is running in the kubernetes cluster, we need to use port-forwarding to access it:
```
kubectl port-forward kubernetestestapp-744fd9ffd4-ttvpj 8080:8080 
```
Expected terminal output:
```
Forwarding from 127.0.0.1:8080 -> 8080
Forwarding from [::1]:8080 -> 8080
```
This indicates that the application is now accessible at `localhost:8080`.

![Working app](/images/locahost-workingApp.png)

## How to debug common issues

In this example, I'll demonstrate how to troubleshoot when your Kubernetes application fails to start due to a missing ConfigMap.

### Problem

The application pod was created but didn't start and showed no logs when using `kubectl logs`.

### Debugging Steps

1. When `kubectl logs` shows no output, examine the pod details using:

    ```bash
    kubectl describe pod <pod-name>
    ```

2. Pay special attention to:
   - Pod Status (in this case `Pending`)
   - Container State (shows `Waiting` with `CreateContainerConfigError`)
   - Events section at the bottom (reveals the root cause)

In this example, the Events section showed:

```
Warning  Failed     37s (x8 over 2m6s)   kubelet            Error: configmap "cm-app-config" not found
```

This clearly indicates that the application failed because the required ConfigMap cm-app-config was not deployed.

### Key Takeaways

- When pods don't start, `kubectl describe pod` is your first debugging tool
- The Events section often contains the root cause of the failure
- Check for missing resources (ConfigMaps, Secrets, etc.) when you see ConfigError messages

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