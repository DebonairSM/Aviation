# Azure Microservices Platform

This is a .NET 8 microservices solution that demonstrates a modern cloud-native architecture using Azure services, implementing CQRS (Command Query Responsibility Segregation) and DDD (Domain-Driven Design) patterns.

## Architecture Overview

The solution implements several modern architectural patterns:

- **CQRS Pattern**: Separates read and write operations for better scalability and performance
- **DDD (Domain-Driven Design)**: Rich domain models with encapsulated business logic
- **Event-Driven Architecture**: Domain events for eventual consistency
- **Microservices**: Independent services with clear bounded contexts
- **Clean Architecture**: Layered approach with clear separation of concerns

### Key Architectural Features

1. **Rich Domain Models**
   - Aggregate roots with encapsulated business logic
   - Value objects for domain invariants
   - Domain events for state changes
   - Strong domain model isolation

2. **CQRS Implementation**
   - Separate command and query models
   - Command side: Rich domain models
   - Query side: Optimized read models
   - MediatR for command/query handling

3. **Domain Events**
   - Event-driven consistency
   - Decoupled event handlers
   - Eventual consistency support

## Project Structure

The solution consists of the following projects:

1. **EnterpriseApiIntegration.Domain**
   - Core domain models and business logic
   - Aggregate roots and entities
   - Value objects and domain events
   - Repository interfaces

2. **EnterpriseApiIntegration.Application**
   - Application services and use cases
   - Command and query handlers
   - DTOs and mappings
   - Business rules validation

3. **EnterpriseApiIntegration.Infrastructure**
   - Data persistence implementations
   - External service integrations
   - Repository implementations
   - Infrastructure concerns

4. **AzureMicroservicesPlatform.ApiGateway**
   - API Gateway using Azure API Management (APIM)
   - Routing and request/response transformation
   - API versioning and documentation

5. **AzureMicroservicesPlatform.Identity**
   - Authentication & OAuth 2.0 via Azure AD
   - User authentication and authorization
   - JWT token validation

6. **AzureMicroservicesPlatform.Services**
   - **Customers**: Customer profile management
   - **Aircraft**: Aircraft services & bookings
   - **Subscriptions**: API subscription management

7. **Tests Projects**
   - Unit tests for domain logic
   - Integration tests for services
   - End-to-end testing

## Domain Model Examples

### Customer Aggregate

```csharp
public class Customer : AggregateRoot
{
    public string Name { get; private set; }
    public Email Email { get; private set; }  // Value Object
    public CustomerRole Role { get; private set; }  // Enumeration
    
    // Rich domain behavior
    public void UpdateContactInfo(string name, string email) { ... }
    public void ChangeRole(CustomerRole newRole) { ... }
}
```

## Getting Started

### Prerequisites

- .NET 8 SDK
- Docker Desktop
- Azure CLI
- Azure Subscription

### Local Development

1. Clone the repository
2. Navigate to the solution directory
3. Run the following command to build and start all services:
   ```bash
   docker-compose up --build
   ```

### Service Endpoints

- API Gateway: http://localhost:5000
- Customers Service: http://localhost:5001
- Aircraft Service: http://localhost:5002
- Subscriptions Service: http://localhost:5003

## Development Guidelines

1. **Domain Model First**
   - Start with the domain model
   - Define aggregate roots and value objects
   - Implement domain events

2. **CQRS Pattern**
   - Keep command and query models separate
   - Use MediatR for command/query handling
   - Optimize read models for specific use cases

3. **Event-Driven Design**
   - Use domain events for cross-aggregate updates
   - Implement event handlers for side effects
   - Maintain eventual consistency

4. **Testing Strategy**
   - Unit test domain logic
   - Integration test repositories
   - End-to-end test critical paths

## Latest Accomplishments

1. **Docker-Based Deployment Implementation**
   - Containerized all microservices for consistent deployment
   - Implemented multi-stage Docker builds for optimized images
   - Set up container registry integration with Azure Container Registry (aviationqa2024)
   - Configured container-based deployment to Azure Web Apps
   - Successfully resolved ACR naming conflicts by implementing globally unique naming strategy

2. **Azure Container Registry Setup**
   - Established secure ACR authentication using admin credentials
   - Implemented GitHub Actions secrets for ACR authentication
   - Updated deployment workflows to use centralized ACR configuration
   - Standardized image tagging and versioning strategy

3. **Version Control Improvements**
   - Successfully cleaned up Terraform state files from version control
   - Implemented proper `.gitignore` patterns for Terraform files
   - Removed cached `.terraform` directories using `git filter-branch`
   - Fixed issues with large file handling in the repository

4. **Infrastructure Management**
   - Enhanced Terraform state management
   - Improved handling of provider versions
   - Better organization of environment-specific configurations

## Deployment

### Docker-Based Deployment

1. Build Docker images for each service:
   ```bash
   # Build Identity Service
   docker build -t aviationqa2024.azurecr.io/identity:latest -f src/AzureMicroservicesPlatform.Identity/Dockerfile .
   
   # Build Aircraft Service
   docker build -t aviationqa2024.azurecr.io/aircraft:latest -f src/AzureMicroservicesPlatform.Services/Aircraft/Dockerfile .
   
   # Build Customers Service
   docker build -t aviationqa2024.azurecr.io/customers:latest -f src/AzureMicroservicesPlatform.Services/Customers/Dockerfile .
   
   # Build Subscriptions Service
   docker build -t aviationqa2024.azurecr.io/subscriptions:latest -f src/AzureMicroservicesPlatform.Services/Subscriptions/Dockerfile .
   ```

2. Push images to Azure Container Registry:
   ```bash
   # Login to Azure Container Registry
   az acr login --name aviationqa2024
   
   # Push all images
   docker push aviationqa2024.azurecr.io/identity:latest
   docker push aviationqa2024.azurecr.io/aircraft:latest
   docker push aviationqa2024.azurecr.io/customers:latest
   docker push aviationqa2024.azurecr.io/subscriptions:latest
   ```

3. Deploy to Azure Web Apps for Containers:
   ```bash
   # Update each web app with the latest container image
   az webapp config container set --name aviation-identity-service --resource-group rg-aviation \
       --docker-custom-image-name aviationqa2024.azurecr.io/identity:latest \
       --docker-registry-server-url https://aviationqa2024.azurecr.io
   
   az webapp config container set --name aviation-aircraft-service --resource-group rg-aviation \
       --docker-custom-image-name aviationqa2024.azurecr.io/aircraft:latest \
       --docker-registry-server-url https://aviationqa2024.azurecr.io
   
   az webapp config container set --name aviation-customers-service --resource-group rg-aviation \
       --docker-custom-image-name aviationqa2024.azurecr.io/customers:latest \
       --docker-registry-server-url https://aviationqa2024.azurecr.io
   
   az webapp config container set --name aviation-subscriptions-service --resource-group rg-aviation \
       --docker-custom-image-name aviationqa2024.azurecr.io/subscriptions:latest \
       --docker-registry-server-url https://aviationqa2024.azurecr.io
   ```

### Important Deployment Notes

- Each service includes a multi-stage Dockerfile for optimized builds
- Images are versioned using Git commit SHA and latest tag
- Azure Container Registry handles image storage and distribution
- Web Apps for Containers provides managed container hosting
- Container logs can be accessed via:
  ```bash
  az webapp log tail --name <app-name> --resource-group rg-aviation
  ```

### Container Configuration

Each service's Dockerfile follows this pattern:

```dockerfile
# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["ServiceName.csproj", "./"]
RUN dotnet restore
COPY . .
RUN dotnet publish -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ServiceName.dll"]
```

Environment-specific configurations are handled through environment variables in Azure Web App settings.

## GitHub Actions for CI/CD

This project uses GitHub Actions to automate the deployment of microservices to Azure. The workflows are organized by environment:

1. **Development Workflow** (`azure-deploy-dev.yml`)
   - Triggers on push to `master` branch
   - Deploys to development resources
   - No manual approval required
   - Uses development-specific app settings

2. **QA Workflow** (`azure-deploy-qa.yml`)
   - Manual trigger only (workflow_dispatch)
   - Requires version tag and deployment confirmation
   - Deploys to QA resources
   - Uses QA-specific app settings

Each workflow requires the following secrets in your GitHub repository:

1. **Azure Authentication**:
   - **AZURE_CREDENTIALS**: The JSON output from the Azure Service Principal creation
   - **AZURE_SUBSCRIPTION_ID**: Your Azure subscription ID
   - **AZURE_TENANT_ID**: Your Azure tenant ID

2. **Container Registry**:
   - **ACR_USERNAME**: Your Azure Container Registry username (aviationqa2024)
   - **ACR_PASSWORD**: Your Azure Container Registry admin password

### Deployment Process

The deployment process follows these stages:

1. **Development**:
   - Automatic deployment on push to `master`
   - Builds and deploys to dev environment
   - Uses commit SHA for image tags
   - Resource naming pattern: `aviation-*-dev`

2. **QA**:
   - Manual trigger with version tag
   - Requires explicit confirmation
   - Builds and deploys to QA environment
   - Uses version tags for images
   - Resource naming pattern: `aviation-*-qa`

### Infrastructure Management

Each environment has its own Terraform configuration in `terraform/environments/`:

```
terraform/
├── environments/
│   ├── dev/
│   │   ├── main.tf        # Development environment config
│   │   └── terraform.tfvars
│   └── qa/
│       ├── main.tf        # QA environment config
│       └── terraform.tfvars
├── modules/
│   └── app-service/       # Shared infrastructure modules
└── backend.tf             # State configuration
```

Key differences between environments:

1. **Development**:
   - Resource Group: `rg-aviation-dev`
   - App Service Names: `aviation-*-dev`
   - Environment Setting: `"ASPNETCORE_ENVIRONMENT" = "Development"`

2. **QA**:
   - Resource Group: `rg-aviation-qa`
   - App Service Names: `aviation-*-qa`
   - Environment Setting: `"ASPNETCORE_ENVIRONMENT" = "QA"`

### Deployment Commands

1. **Build Docker Images**:
   ```bash
   # Build Identity Service
   docker build -t aviationqa2024.azurecr.io/identity:${TAG} -f AzureMicroservicesPlatform.Identity/Dockerfile .
   
   # Build Aircraft Service
   docker build -t aviationqa2024.azurecr.io/aircraft:${TAG} -f AzureMicroservicesPlatform.Services.Aircraft/Dockerfile .
   
   # Build Customers Service
   docker build -t aviationqa2024.azurecr.io/customers:${TAG} -f AzureMicroservicesPlatform.Services.Customers/Dockerfile .
   
   # Build Subscriptions Service
   docker build -t aviationqa2024.azurecr.io/subscriptions:${TAG} -f AzureMicroservicesPlatform.Services.Subscriptions/Dockerfile .
   ```

2. **Push Images**:
   ```bash
   # Login to Azure Container Registry
   az acr login --name aviationqa2024
   
   # Push all images
   docker push aviationqa2024.azurecr.io/identity:${TAG}
   docker push aviationqa2024.azurecr.io/aircraft:${TAG}
   docker push aviationqa2024.azurecr.io/customers:${TAG}
   docker push aviationqa2024.azurecr.io/subscriptions:${TAG}
   ```

Where `${TAG}` is:
- For development: The commit SHA (`github.sha`)
- For QA: A version tag (e.g., `v1.0.0`)

### Infrastructure Deployment

To deploy or update infrastructure:

1. **Development**:
   ```bash
   cd terraform/environments/dev
        terraform init
        terraform plan
   terraform apply
   ```

2. **QA**:
   ```bash
   cd terraform/environments/qa
   terraform init
   terraform plan
   terraform apply
```

### Best Practices

1. **Image Tagging**:
   - Development: Use commit SHA for traceability
   - QA: Use semantic version tags (e.g., v1.0.0)
   - Never use 'latest' tag in production environments

2. **Security**:
   - Store all credentials in GitHub Secrets
   - Use service principals with minimum required permissions
   - Regularly rotate ACR admin credentials
   - Use managed identities where possible

3. **Monitoring**:
   - Use Application Insights for each service
   - Set up alerts for critical metrics
   - Monitor container health and logs
   - Container logs can be accessed via:
     ```bash
     az webapp log tail --name <app-name> --resource-group rg-aviation-dev  # For dev
     az webapp log tail --name <app-name> --resource-group rg-aviation-qa   # For QA
     ```

## Contributing

1. Fork the repository
2. Create a feature branch
3. Commit your changes
4. Push to the branch
5. Create a Pull Request

## License

This project is licensed under the MIT License - see the LICENSE file for details.