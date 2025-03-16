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
   - Set up container registry integration with Azure Container Registry
   - Configured container-based deployment to Azure Web Apps

2. **Version Control Improvements**
   - Successfully cleaned up Terraform state files from version control
   - Implemented proper `.gitignore` patterns for Terraform files
   - Removed cached `.terraform` directories using `git filter-branch`
   - Fixed issues with large file handling in the repository

3. **Infrastructure Management**
   - Enhanced Terraform state management
   - Improved handling of provider versions
   - Better organization of environment-specific configurations

## Deployment

### Docker-Based Deployment

1. Build Docker images for each service:
   ```bash
   # Build Identity Service
   docker build -t aviation.azurecr.io/identity:latest -f src/AzureMicroservicesPlatform.Identity/Dockerfile .
   
   # Build Aircraft Service
   docker build -t aviation.azurecr.io/aircraft:latest -f src/AzureMicroservicesPlatform.Services/Aircraft/Dockerfile .
   
   # Build Customers Service
   docker build -t aviation.azurecr.io/customers:latest -f src/AzureMicroservicesPlatform.Services/Customers/Dockerfile .
   
   # Build Subscriptions Service
   docker build -t aviation.azurecr.io/subscriptions:latest -f src/AzureMicroservicesPlatform.Services/Subscriptions/Dockerfile .
   ```

2. Push images to Azure Container Registry:
   ```bash
   # Login to Azure Container Registry
   az acr login --name aviation
   
   # Push all images
   docker push aviation.azurecr.io/identity:latest
   docker push aviation.azurecr.io/aircraft:latest
   docker push aviation.azurecr.io/customers:latest
   docker push aviation.azurecr.io/subscriptions:latest
   ```

3. Deploy to Azure Web Apps for Containers:
   ```bash
   # Update each web app with the latest container image
   az webapp config container set --name aviation-identity-service --resource-group rg-aviation \
       --docker-custom-image-name aviation.azurecr.io/identity:latest \
       --docker-registry-server-url https://aviation.azurecr.io
   
   az webapp config container set --name aviation-aircraft-service --resource-group rg-aviation \
       --docker-custom-image-name aviation.azurecr.io/aircraft:latest \
       --docker-registry-server-url https://aviation.azurecr.io
   
   az webapp config container set --name aviation-customers-service --resource-group rg-aviation \
       --docker-custom-image-name aviation.azurecr.io/customers:latest \
       --docker-registry-server-url https://aviation.azurecr.io
   
   az webapp config container set --name aviation-subscriptions-service --resource-group rg-aviation \
       --docker-custom-image-name aviation.azurecr.io/subscriptions:latest \
       --docker-registry-server-url https://aviation.azurecr.io
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

This project uses GitHub Actions to automate the deployment of microservices to Azure. To set up the workflow, follow these tips:

1. **GitHub Secrets**:
   Before using the GitHub Actions workflow, ensure that you add the following secrets to your GitHub repository:
   - **AZURE_CREDENTIALS**: The JSON output from the Azure Service Principal creation. This allows GitHub Actions to authenticate with Azure. You can get this by running:
     ```bash
     az ad sp create-for-rbac --name "github-actions-aviation" --role contributor --scopes /subscriptions/08398d64-5d63-4da9-8daf-e15b00f4d227 --sdk-auth
     ```
   - **AZURE_SUBSCRIPTION_ID**: `08398d64-5d63-4da9-8daf-e15b00f4d227`
   - **AZURE_TENANT_ID**: `621db766-1ccd-40f1-80b0-ef469bd8f081`

2. **Variable Values in Workflow**:
   In addition to the secrets, make sure to update the following variable values in the `.github/workflows/azure-deploy.yml` file:
   - **app-name**: Replace the placeholder values with the actual names of your Azure Web Apps (e.g., `aviation-api-gateway-app`, `aviation-identity-service`, etc.).
   - **DOTNET_VERSION**: Ensure this is set to the correct version of .NET you are using (e.g., `8.0.x`).

3. **Triggering the Workflow**:
   The workflow is triggered automatically on pushes to the `main` branch. You can also manually trigger it from the GitHub Actions tab in your repository.

4. **Monitoring Deployments**:
   You can monitor the deployment process in the "Actions" tab of your GitHub repository. If there are any issues, the logs will provide details on what went wrong.

## Azure Web Apps Creation

To deploy the microservices to Azure, we created several Web Apps using the Azure CLI. Below are the commands used to create each Web App with the appropriate runtime specification.

### Commands to Create Web Apps

1. **Create the Identity Service Web App**:
   ```bash
   az webapp create --resource-group rg-aviation --plan asp-aviation --name aviation-identity-service --runtime "DOTNETCORE|8.0"
   ```

2. **Create the Aircraft Service Web App**:
   ```bash
   az webapp create --resource-group rg-aviation --plan asp-aviation --name aviation-aircraft-service --runtime "DOTNETCORE|8.0"
   ```

3. **Create the Customers Service Web App**:
   ```bash
   az webapp create --resource-group rg-aviation --plan asp-aviation --name aviation-customers-service --runtime "DOTNETCORE|8.0"
   ```

4. **Create the Subscriptions Service Web App**:
   ```bash
   az webapp create --resource-group rg-aviation --plan asp-aviation --name aviation-subscriptions-service --runtime "DOTNETCORE|8.0"
   ```

### Important Notes

- Ensure that the runtime specified is compatible with the Azure Web App service. For .NET Core applications, the correct format is `DOTNETCORE|<version>`.
- If you encounter issues with the runtime, you can list the supported runtimes for Linux Web Apps using the command:
  ```bash
  az webapp list-runtimes --os-type linux
  ```

This section documents the process of creating Azure Web Apps for the microservices in the Aviation project, ensuring that the deployment steps are clear and reproducible.

## Lessons Learned

During the development and CI/CD implementation, we encountered and resolved several important challenges:

1. **Test Project Configuration**
   - Always set `<IsTestProject>true</IsTestProject>` in test project files
   - Ensure test projects have proper package references with correct asset inclusion:
     ```xml
     <PackageReference Include="coverlet.collector" Version="6.0.0">
       <PrivateAssets>all</PrivateAssets>
       <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
     </PackageReference>
     ```
   - Configure VSTest settings in project files for better test discovery and logging

2. **GitHub Actions Workflow Best Practices**
   - Maintain consistency between build and test configurations (e.g., Debug vs Release)
   - Use explicit configuration settings in test commands:
     ```yaml
     dotnet test --configuration Release --no-build --verbosity normal
     ```
   - Implement detailed logging for better troubleshooting:
     ```yaml
     --logger "trx;LogFileName=test-results.trx"
     --collect:"XPlat Code Coverage"
     ```
   - Store test results as artifacts for post-run analysis

3. **Version Control Management**
   - When reverting changes, consider using `git revert` for shared repositories
   - For local-only changes, `git reset` can be used with caution
   - Always stash or commit local changes before performing repository operations
   - Use `--force` push carefully and only when necessary

4. **CI/CD Pipeline Debugging**
   - Monitor build and test configurations across different environments
   - Verify test assembly paths and configurations match between local and CI environments
   - Use detailed logging and artifacts collection for troubleshooting
   - Implement proper error handling and reporting in workflows

5. **Terraform Authentication and Azure Setup**
   - When setting up Terraform with Azure, follow these authentication steps:
     ```powershell
     # Set required environment variables
     $env:ARM_CLIENT_ID="your-client-id"
     $env:ARM_CLIENT_SECRET="your-client-secret"
     $env:ARM_TENANT_ID="your-tenant-id"
     $env:ARM_SUBSCRIPTION_ID="your-subscription-id"
     ```
   - Common authentication issues and solutions:
     1. Invalid client secret error:
        - Ensure you're using the secret VALUE, not the secret ID
        - Create a new client secret if needed:
          ```powershell
          az ad app credential reset --id your-app-id --append
          ```
     2. Token/login issues:
        - Clear existing credentials: `az logout`
        - Login with specific scope:
          ```powershell
          az login --scope https://graph.microsoft.com/.default
          ```
   - Best practices for service principal setup:
     ```bash
     # Create service principal with contributor role
     az ad sp create-for-rbac \
       --name "terraform-sp" \
       --role contributor \
       --scopes /subscriptions/your-subscription-id
     ```

6. **Terraform State and Resource Management**
   - Resource naming conventions:
     ```hcl
     resource "azurerm_resource_group" "rg" {
       name     = "rg-${var.project}-${var.environment}"
       location = var.location
     }
     ```
   - App Service configuration patterns:
     ```hcl
     resource "azurerm_linux_web_app" "app" {
       name                = "app-${var.service}-${var.environment}"
       resource_group_name = azurerm_resource_group.rg.name
       location            = azurerm_resource_group.rg.location
       service_plan_id     = azurerm_service_plan.plan.id
       
       site_config {
         application_stack {
           dotnet_version = "8.0"
         }
       }
       
       app_settings = {
         "ASPNETCORE_ENVIRONMENT" = title(var.environment)
         "KeyVaultName"          = "kv-${var.project}-${var.environment}"
       }
     }
     ```
   - Environment-specific configurations:
     ```hcl
     # terraform.tfvars
     environment          = "qa"
     resource_group      = "rg-aviation-qa"
     location            = "eastus2"
     app_service_plan_sku = "B1"  # Use different SKUs per environment
     ```

7. **Terraform Workflow Best Practices**
   - Always run `terraform init` after adding new providers or modules
   - Use `terraform plan` to review changes before applying
   - Save plans for critical environments:
     ```bash
     terraform plan -out=qa.tfplan
     terraform apply qa.tfplan
     ```
   - Use consistent formatting:
     ```bash
     terraform fmt -recursive
     ```
   - Validate configurations:
     ```bash
     terraform validate
     ```

## Infrastructure as Code (IaC)

This project uses Terraform to automate infrastructure provisioning across multiple environments (QA, Pre-prod, and Production). The infrastructure code is stored in the `terraform/` directory.

### Directory Structure

```
terraform/
├── environments/
│   ├── qa/
│   │   ├── main.tf
│   │   ├── variables.tf
│   │   └── terraform.tfvars
│   ├── pre-prod/
│   │   ├── main.tf
│   │   ├── variables.tf
│   │   └── terraform.tfvars
│   └── prod/
│       ├── main.tf
│       ├── variables.tf
│       └── terraform.tfvars
├── modules/
│   ├── app-service/
│   ├── sql-database/
│   └── key-vault/
└── backend.tf
```

### Environment Configuration

Each environment (QA, Pre-prod, Prod) uses the same module configurations with different variables:

```hcl
# environments/qa/main.tf example
module "web_apps" {
  source = "../../modules/app-service"
  
  environment     = "qa"
  resource_group  = "rg-aviation-qa"
  location        = "eastus2"
  
  apps = {
    identity = {
      name     = "aviation-identity-qa"
      sku_name = "B1"
    }
    aircraft = {
      name     = "aviation-aircraft-qa"
      sku_name = "B1"
    }
    # ... other services
  }
}
```

### GitHub Actions Integration

The infrastructure deployment is integrated into the CI/CD pipeline using GitHub Actions. Add these secrets to your repository:

- `ARM_CLIENT_ID`: Azure Service Principal ID
- `ARM_CLIENT_SECRET`: Azure Service Principal Secret
- `ARM_SUBSCRIPTION_ID`: Azure Subscription ID
- `ARM_TENANT_ID`: Azure Tenant ID

Example workflow for infrastructure deployment:

```yaml
name: 'Infrastructure Deployment'

on:
  push:
    branches:
      - main
    paths:
      - 'terraform/**'
  workflow_dispatch:
    inputs:
      environment:
        description: 'Environment to deploy to'
        required: true
        default: 'qa'
        type: choice
        options:
          - qa
          - pre-prod
          - prod

jobs:
  terraform:
    runs-on: ubuntu-latest
    environment: ${{ github.event.inputs.environment || 'qa' }}
    
    steps:
    - uses: actions/checkout@v4
    
    - name: Setup Terraform
      uses: hashicorp/setup-terraform@v3
      
    - name: Terraform Init
      run: |
        cd terraform/environments/${{ github.event.inputs.environment || 'qa' }}
        terraform init
      env:
        ARM_CLIENT_ID: ${{ secrets.ARM_CLIENT_ID }}
        ARM_CLIENT_SECRET: ${{ secrets.ARM_CLIENT_SECRET }}
        ARM_SUBSCRIPTION_ID: ${{ secrets.ARM_SUBSCRIPTION_ID }}
        ARM_TENANT_ID: ${{ secrets.ARM_TENANT_ID }}
        
    - name: Terraform Plan
      run: |
        cd terraform/environments/${{ github.event.inputs.environment || 'qa' }}
        terraform plan
      
    - name: Terraform Apply
      if: github.ref == 'refs/heads/main'
      run: |
        cd terraform/environments/${{ github.event.inputs.environment || 'qa' }}
        terraform apply -auto-approve
```

### State Management

The Terraform state is stored in Azure Storage Account for better collaboration and state locking:

```hcl
# backend.tf
terraform {
  backend "azurerm" {
    resource_group_name  = "rg-terraform-state"
    storage_account_name = "tfstateaviation"
    container_name      = "tfstate"
    key                 = "terraform.tfstate"
  }
}
```

### Environment Promotion

The infrastructure follows a promotion path:
1. Changes are first applied to QA
2. After testing, promoted to Pre-prod
3. Finally, deployed to Production

This is managed through GitHub Actions environments and approval processes:

```yaml
environments:
  qa:
    deployment_branch_policy:
      protected_branches: false
  pre-prod:
    deployment_branch_policy:
      protected_branches: true
    reviewers:
      - tech-leads
  prod:
    deployment_branch_policy:
      protected_branches: true
    reviewers:
      - release-managers
```

### Best Practices

1. **State Management**
   - Use remote state storage in Azure Storage Account
   - Enable state locking to prevent concurrent modifications
   - Use workspaces for environment isolation

2. **Security**
   - Store sensitive values in Azure Key Vault
   - Use managed identities where possible
   - Implement least privilege access

3. **Module Design**
   - Create reusable modules for common infrastructure
   - Version modules for better change management
   - Document module inputs and outputs

4. **Cost Management**
   - Use different SKUs per environment
   - Implement auto-scaling rules
   - Set up cost alerts and budgets

## Contributing

1. Fork the repository
2. Create a feature branch
3. Commit your changes
4. Push to the branch
5. Create a Pull Request

## License

This project is licensed under the MIT License - see the LICENSE file for details.