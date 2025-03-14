FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["AzureMicroservicesPlatform.Services.Customers/AzureMicroservicesPlatform.Services.Customers.csproj", "AzureMicroservicesPlatform.Services.Customers/"]
RUN dotnet restore "AzureMicroservicesPlatform.Services.Customers/AzureMicroservicesPlatform.Services.Customers.csproj"
COPY . .
WORKDIR "/src/AzureMicroservicesPlatform.Services.Customers"
RUN dotnet build "AzureMicroservicesPlatform.Services.Customers.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AzureMicroservicesPlatform.Services.Customers.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AzureMicroservicesPlatform.Services.Customers.dll"] 