#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Services/DevOps/DevOps.Api/DevOps.Api.csproj", "Services/DevOps/DevOps.Api/"]
COPY ["Services/DevOps/DevOps.Infrastructure/DevOps.Infrastructure.csproj", "Services/DevOps/DevOps.Infrastructure/"]
COPY ["Services/DevOps/DevOps.AppLogic/DevOps.AppLogic.csproj", "Services/DevOps/DevOps.AppLogic/"]
COPY ["Services/DevOps/DevOps.Domain/DevOps.Domain.csproj", "Services/DevOps/DevOps.Domain/"]
RUN dotnet restore "./Services/DevOps/DevOps.Api/./DevOps.Api.csproj"
COPY . .
WORKDIR "/src/Services/DevOps/DevOps.Api"
ARG Mode=Debug
RUN dotnet build "./DevOps.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
ARG Mode=Debug
RUN dotnet publish "./DevOps.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DevOps.Api.dll"]