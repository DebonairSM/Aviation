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

## Deployment

### Azure Kubernetes Service (AKS)

1. Create an AKS cluster:
   ```bash
   az aks create --resource-group <YourResourceGroup> --name <YourAKSClusterName> --node-count 1 --enable-addons monitoring --generate-ssh-keys
   ```

2. Deploy the services:
   ```bash
   kubectl apply -f k8s/
   ```

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

## Contributing

1. Fork the repository
2. Create a feature branch
3. Commit your changes
4. Push to the branch
5. Create a Pull Request

## License

This project is licensed under the MIT License - see the LICENSE file for details.