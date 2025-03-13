FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["EnterpriseApiIntegration.Api/EnterpriseApiIntegration.Api.csproj", "EnterpriseApiIntegration.Api/"]
COPY ["EnterpriseApiIntegration.Application/EnterpriseApiIntegration.Application.csproj", "EnterpriseApiIntegration.Application/"]
COPY ["EnterpriseApiIntegration.Domain/EnterpriseApiIntegration.Domain.csproj", "EnterpriseApiIntegration.Domain/"]
COPY ["EnterpriseApiIntegration.Infrastructure/EnterpriseApiIntegration.Infrastructure.csproj", "EnterpriseApiIntegration.Infrastructure/"]
RUN dotnet restore "EnterpriseApiIntegration.Api/EnterpriseApiIntegration.Api.csproj"
COPY . .
WORKDIR "/src/EnterpriseApiIntegration.Api"
RUN dotnet build "EnterpriseApiIntegration.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EnterpriseApiIntegration.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EnterpriseApiIntegration.Api.dll"] 