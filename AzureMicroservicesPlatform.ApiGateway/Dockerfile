FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["AzureMicroservicesPlatform.ApiGateway/AzureMicroservicesPlatform.ApiGateway.csproj", "AzureMicroservicesPlatform.ApiGateway/"]
RUN dotnet restore "AzureMicroservicesPlatform.ApiGateway/AzureMicroservicesPlatform.ApiGateway.csproj"
COPY . .
WORKDIR "/src/AzureMicroservicesPlatform.ApiGateway"
RUN dotnet build "AzureMicroservicesPlatform.ApiGateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AzureMicroservicesPlatform.ApiGateway.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AzureMicroservicesPlatform.ApiGateway.dll"] 