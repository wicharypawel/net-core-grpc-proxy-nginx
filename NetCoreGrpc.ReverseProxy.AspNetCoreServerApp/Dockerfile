#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 5000
ENV ASPNETCORE_URLS http://*:5000
ENV ASPNETCORE_ENVIRONMENT Development

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["NetCoreGrpc.ReverseProxy.AspNetCoreServerApp/NetCoreGrpc.ReverseProxy.AspNetCoreServerApp.csproj", "NetCoreGrpc.ReverseProxy.AspNetCoreServerApp/"]
RUN dotnet restore "NetCoreGrpc.ReverseProxy.AspNetCoreServerApp/NetCoreGrpc.ReverseProxy.AspNetCoreServerApp.csproj"
COPY . .
WORKDIR "/src/NetCoreGrpc.ReverseProxy.AspNetCoreServerApp"
RUN dotnet build "NetCoreGrpc.ReverseProxy.AspNetCoreServerApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NetCoreGrpc.ReverseProxy.AspNetCoreServerApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NetCoreGrpc.ReverseProxy.AspNetCoreServerApp.dll"]