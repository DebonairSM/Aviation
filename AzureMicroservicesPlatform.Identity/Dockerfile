# Use the official .NET SDK image as the build image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the solution file and restore dependencies
COPY ["EnterpriseApiIntegration.sln", "./"]
COPY ["AzureMicroservicesPlatform.Identity/AzureMicroservicesPlatform.Identity.csproj", "AzureMicroservicesPlatform.Identity/"]
COPY ["EnterpriseApiIntegration.Domain/EnterpriseApiIntegration.Domain.csproj", "EnterpriseApiIntegration.Domain/"]
COPY ["EnterpriseApiIntegration.Application/EnterpriseApiIntegration.Application.csproj", "EnterpriseApiIntegration.Application/"]
COPY ["EnterpriseApiIntegration.Infrastructure/EnterpriseApiIntegration.Infrastructure.csproj", "EnterpriseApiIntegration.Infrastructure/"]
RUN dotnet restore "AzureMicroservicesPlatform.Identity/AzureMicroservicesPlatform.Identity.csproj"

# Copy the rest of the source code
COPY . .

# Build the application
RUN dotnet build "AzureMicroservicesPlatform.Identity/AzureMicroservicesPlatform.Identity.csproj" -c Release -o /app/build

# Publish the application
RUN dotnet publish "AzureMicroservicesPlatform.Identity/AzureMicroservicesPlatform.Identity.csproj" -c Release -o /app/publish

# Build the runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "AzureMicroservicesPlatform.Identity.dll"] 