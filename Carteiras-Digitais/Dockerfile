# Acesse https://aka.ms/customizecontainer para saber como personalizar seu contêiner de depuração e como o Visual Studio usa este Dockerfile para criar suas imagens para uma depuração mais rápida.

# Esta fase é usada durante a execução no VS no modo rápido (Padrão para a configuração de Depuração)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# Esta fase é usada para compilar o projeto de serviço
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Carteiras-Digitais/Carteiras-Digitais.API.csproj", "Carteiras-Digitais/"]
COPY ["Carteiras-Digitais.Api/Carteiras-Digitais.Core.csproj", "Carteiras-Digitais.Api/"]
COPY ["Carteiras-Digitais.Shared/Carteiras-Digitais.Shared.csproj", "Carteiras-Digitais.Shared/"]
COPY ["Carteiras-Digitais.Infrasctruture/Carteiras-Digitais.Infrasctruture.csproj", "Carteiras-Digitais.Infrasctruture/"]
COPY ["Carteiras-Digitais.Test/Carteiras-Digitais.Test.csproj", "Carteiras-Digitais.Test/"]
COPY ["Carteiras-Digitais.Application/Carteiras-Digitais.Application.csproj", "Carteiras-Digitais.Application/"]
RUN dotnet restore "./Carteiras-Digitais/Carteiras-Digitais.API.csproj"
COPY . .
WORKDIR "/src/Carteiras-Digitais"
RUN dotnet build "./Carteiras-Digitais.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Esta fase é usada para publicar o projeto de serviço a ser copiado para a fase final
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Carteiras-Digitais.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Esta fase é usada na produção ou quando executada no VS no modo normal (padrão quando não está usando a configuração de Depuração)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Carteiras-Digitais.API.dll"]