# KubernetesLearning

[![Build and push to DockerHub](https://github.com/tomaszprasolek/KubernetesLearning/actions/workflows/dotnet.yml/badge.svg)](https://github.com/tomaszprasolek/KubernetesLearning/actions/workflows/dotnet.yml)

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

Create simple web app that uses Kubernetes underneath to show how Kubernetes works using it basic components. Show how some features can be done with Kubernets.

The app is written in C#, I used ASP.NET Razor Pages with MSSQL database to store data.
App has very (very very) simple profile form that save/update data in MSSSQL database.

k8s resources used:
- **Namespace** - to put all my app components in one group, that groups are called namespaces in k8s
- **StatefulSet** - database
- **ConfigMap** - app configuration
- **Secret** - app sensitive data e.g. connection string, password to DB

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
ConfigMap.yaml
```
### Create namespace
```
kubectl apply -f .\Namespace.yaml
kubens tomo-app
```
The first command creates the namespace, while the second changes the active namespace in kubectl. Note: [Kubens](https://github.com/ahmetb/kubectx) is an additional tool (not built-in) that I installed to quickly switch between contexts and namespaces in k8s.

From [documantation](https://kubernetes.io/docs/concepts/overview/working-with-objects/namespaces/):  
*namespaces provide a mechanism for isolating groups of resources within a single cluster. Names of resources need to be unique within a namespace, but not across namespaces. Namespace-based scoping is applicable only for namespaced objects (e.g. Deployments, Services, etc.) and not for cluster-wide objects (e.g. StorageClass, Nodes, PersistentVolumes, etc.).*

---

### Prepare secret(s)
```
kubectl apply -f .\Mssql_Secret.yaml
```
Expected response in CLI: `secret/mssql created`

From [documentation](https://kubernetes.io/docs/concepts/configuration/secret/)  
*A Secret is an object that contains a small amount of sensitive data such as a password, a token, or a key. Such information might otherwise be put in a Pod specification or in a container image. Using a Secret means that you don't need to include confidential data in your application code.*

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

## TODO

- [ ] names, labels, matchlabes - what is it? how it works? why it is important
- [ ] cronJob - job to get current currency value and add it to DB
- [ ] ingress - does not work on my machine with Win11 and on minikube
- [ ] HELM charts - what is it? how it works?

## Links
- https://woshub.com/move-wsl-another-drive-windows/
- https://docs.docker.com/get-started/introduction/build-and-push-first-image/#build-and-push-the-image
- https://medium.com/@iskandre/explanation-of-ports-in-kubernetes-service-a29490515fb2
