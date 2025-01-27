#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["UserManagementService/UserManagementService.csproj", "UserManagementService/"]
COPY ["EventBus/EventBus.csproj", "EventBus/"]
RUN dotnet restore "./UserManagementService/./UserManagementService.csproj"
COPY . .
WORKDIR "/src/UserManagementService"
RUN dotnet build "./UserManagementService.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./UserManagementService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:5001
ENV ASPNETCORE_HTTP_PORTS=5001
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UserManagementService.dll"]