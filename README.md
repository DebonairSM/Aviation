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
- Azure CLI
- Azure Subscription
- Terraform

### Azure Authentication

You can set up Azure authentication in PowerShell to avoid logging in repeatedly:

```powershell
# Set Azure environment variables
$env:ARM_CLIENT_ID = "your-client-id"
$env:ARM_CLIENT_SECRET = "your-client-secret"
$env:ARM_SUBSCRIPTION_ID = "your-subscription-id"
$env:ARM_TENANT_ID = "your-tenant-id"

# Or set them all at once
$env:ARM_CLIENT_ID = "your-client-id"; $env:ARM_CLIENT_SECRET = "your-client-secret"; $env:ARM_SUBSCRIPTION_ID = "your-subscription-id"; $env:ARM_TENANT_ID = "your-tenant-id"
```

These environment variables will persist for your current PowerShell session. To make them permanent, you can add them to your PowerShell profile:

```powershell
# Open PowerShell profile
notepad $PROFILE

# Add the environment variables
$env:ARM_CLIENT_ID = "your-client-id"
$env:ARM_CLIENT_SECRET = "your-client-secret"
$env:ARM_SUBSCRIPTION_ID = "your-subscription-id"
$env:ARM_TENANT_ID = "your-tenant-id"
```

### Configuring Graph API Permissions

To enable service-to-service authentication and management capabilities, you need to configure the following Graph API permissions:

1. Go to Azure Portal (portal.azure.com)
2. Navigate to Azure Active Directory (Entra ID)
3. Go to "App registrations"
4. Find your app with ClientId: e96126b4-d4f9-4716-adac-afa697eee97d
5. Click on "API permissions"
6. Click "Add a permission"
7. Select "Microsoft Graph"
8. Select "Application permissions"
9. Add these permissions:
   - Application.ReadWrite.All
   - Directory.Read.All
   - Directory.ReadWrite.All
10. Click "Grant admin consent"

These permissions are required for:
- Managing application registrations
- Reading and writing directory data
- Managing service principals
- Configuring API permissions

### Service Endpoints

- API Gateway: https://aviation-gateway-qa.azurewebsites.net
- Identity Service: https://aviation-identity-qa.azurewebsites.net
- Aircraft Service: https://aviation-aircraft-qa.azurewebsites.net
- Customers Service: https://aviation-customers-qa.azurewebsites.net
- Subscriptions Service: https://aviation-subscriptions-qa.azurewebsites.net

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

## Infrastructure Management with Terraform

The infrastructure is managed using Terraform, with separate configurations for different environments:

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

### Infrastructure Deployment

To deploy or update infrastructure:

1. **Set up Azure Service Principal**:
   ```bash
   az ad sp create-for-rbac --name "github-actions-aviation" --role contributor --scopes /subscriptions/<subscription-id> --sdk-auth
   ```

2. **Configure Terraform Variables**:
   Create a `terraform.tfvars` file in your environment directory with:
   ```hcl
   subscription_id = "your-subscription-id"
   tenant_id       = "your-tenant-id"
   client_id       = "your-client-id"
   client_secret   = "your-client-secret"
   ```

3. **Initialize and Apply**:
   ```bash
   cd terraform/environments/qa
   terraform init
   terraform plan
   terraform apply
   ```

### Environment-Specific Configurations

1. **QA Environment**:
   - Resource Group: `rg-aviation-qa`
   - App Service Names: `aviation-*-qa`
   - Environment Setting: `"ASPNETCORE_ENVIRONMENT" = "QA"`
   - Service Plan: `B1` SKU
   - Location: `eastus2`

### Best Practices

1. **State Management**:
   - Use local state for development
   - Consider remote state for team collaboration
   - Never commit state files to version control

2. **Security**:
   - Store credentials in Azure Key Vault
   - Use service principals with minimum required permissions
   - Keep sensitive variables in `terraform.tfvars`
   - Never commit `terraform.tfvars` to version control

3. **Resource Naming**:
   - Use consistent naming conventions
   - Include environment in resource names
   - Follow Azure naming best practices

## GitHub Actions for CI/CD

This project uses GitHub Actions to automate the deployment to Azure. The workflows are organized by environment:

1. **QA Workflow** (`azure-deploy-qa.yml`)
   - Manual trigger only (workflow_dispatch)
   - Requires deployment confirmation
   - Deploys to QA resources
   - Uses QA-specific app settings

Each workflow requires the following secrets in your GitHub repository:

1. **Azure Authentication**:
   - **AZURE_CREDENTIALS**: The JSON output from the Azure Service Principal creation
   - **AZURE_SUBSCRIPTION_ID**: Your Azure subscription ID
   - **AZURE_TENANT_ID**: Your Azure tenant ID

## Contributing

1. Fork the repository
2. Create a feature branch
3. Commit your changes
4. Push to the branch
5. Create a Pull Request

## License

This project is licensed under the MIT License - see the LICENSE file for details.