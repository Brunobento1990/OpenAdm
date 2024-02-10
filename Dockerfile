#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["OpenAdm.Api/OpenAdm.Api.csproj", "OpenAdm.Api/"]
COPY ["OpenAdm.Application/OpenAdm.Application.csproj", "OpenAdm.Application/"]
COPY ["OpenAdm.Domain/OpenAdm.Domain.csproj", "OpenAdm.Domain/"]
COPY ["OpenAdm.IoC/OpenAdm.IoC.csproj", "OpenAdm.IoC/"]
COPY ["OpenAdm.Infra/OpenAdm.Infra.csproj", "OpenAdm.Infra/"]
RUN dotnet restore "OpenAdm.Api/OpenAdm.Api.csproj"
COPY . .
WORKDIR "/src/OpenAdm.Api"
RUN dotnet build "OpenAdm.Api/OpenAdm.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "OpenAdm.Api/OpenAdm.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV TZ=America/Sao_Paulo
ENTRYPOINT ["dotnet", "OpenAdm.Api.dll"]