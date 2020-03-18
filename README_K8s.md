# gRPC reverse proxy Nginx hosted in K8s

__NOTE: Run commands in root directory__

## Build images
```
docker build -t grpc-reverseproxy-client:latest -f .\NetCoreGrpc.ReverseProxy.ConsoleClientApp\Dockerfile .
docker build -t grpc-reverseproxy-nginx:latest -f .\NetCoreGrpc.ReverseProxy.Nginx\Dockerfile .\NetCoreGrpc.ReverseProxy.Nginx
docker build -t grpc-reverseproxy-server:latest -f .\NetCoreGrpc.ReverseProxy.AspNetCoreServerApp\Dockerfile .
```

## Create resources in K8s
```
kubectl apply -f .\k8s\grpc-reverseproxy-server.yaml
kubectl apply -f .\k8s\grpc-reverseproxy-nginx.yaml
kubectl create -f .\k8s\grpc-reverseproxy-client.yaml
```

## Verify connection
```
kubectl logs grpc-reverseproxy-client
```

## Tear down resources
```
kubectl delete -f .\k8s\grpc-reverseproxy-client.yaml
kubectl delete -f .\k8s\grpc-reverseproxy-nginx.yaml
kubectl delete -f .\k8s\grpc-reverseproxy-server.yaml
```
