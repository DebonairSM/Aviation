FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["AzureMicroservicesPlatform.Services.Aircraft/AzureMicroservicesPlatform.Services.Aircraft.csproj", "AzureMicroservicesPlatform.Services.Aircraft/"]
RUN dotnet restore "AzureMicroservicesPlatform.Services.Aircraft/AzureMicroservicesPlatform.Services.Aircraft.csproj"
COPY . .
WORKDIR "/src/AzureMicroservicesPlatform.Services.Aircraft"
RUN dotnet build "AzureMicroservicesPlatform.Services.Aircraft.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AzureMicroservicesPlatform.Services.Aircraft.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AzureMicroservicesPlatform.Services.Aircraft.dll"] 