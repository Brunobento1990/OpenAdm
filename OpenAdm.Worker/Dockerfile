FROM mcr.microsoft.com/dotnet/runtime:10.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["OpenAdm.Worker/OpenAdm.Worker.csproj", "OpenAdm.Worker/"]
COPY ["OpenAdm.Worker.Infra/OpenAdm.Worker.Infra.csproj", "OpenAdm.Worker.Infra/"]
COPY ["OpenAdm.Data/OpenAdm.Data.csproj", "OpenAdm.Data/"]
COPY ["OpenAdm.Domain/OpenAdm.Domain.csproj", "OpenAdm.Domain/"]
COPY ["OpenAdm.Worker.Application/OpenAdm.Worker.Application.csproj", "OpenAdm.Worker.Application/"]
COPY ["OpenAdm.Pdf/OpenAdm.Pdf.csproj", "OpenAdm.Pdf/"]
RUN dotnet restore "OpenAdm.Worker/OpenAdm.Worker.csproj"
COPY . .
WORKDIR "/src/OpenAdm.Worker"
RUN dotnet build "./OpenAdm.Worker.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./OpenAdm.Worker.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV TZ=America/Sao_Paulo
ENTRYPOINT ["dotnet", "OpenAdm.Worker.dll"]
