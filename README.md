# Aviation - Enterprise API Integration

This project is an ASP.NET Core Web API that demonstrates enterprise-level API integration with robust authorization patterns.

## Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or later
- [Docker Desktop](https://www.docker.com/products/docker-desktop/)
- [Visual Studio 2022](https://visualstudio.microsoft.com/vs/) (recommended) or [VS Code](https://code.visualstudio.com/)
- SSL Certificate for HTTPS development

## Solution Architecture

This solution follows Clean Architecture principles with:

- Domain-Driven Design (DDD) concepts
- CQRS pattern (Command Query Responsibility Segregation)
- Dependency Injection
- Repository Pattern
- Policy-based Authorization

## Project Structure

- `EnterpriseApiIntegration.Api`: Main API project with controllers and configuration
- `EnterpriseApiIntegration.Application`: Application layer containing business logic and interfaces
- `EnterpriseApiIntegration.Domain`: Domain entities and business rules
- `EnterpriseApiIntegration.Infrastructure`: Data access and external service implementations
- `EnterpriseApiIntegration.Tests`: Unit and integration tests

## Getting Started

### 1. Clone the Repository

```powershell
git clone https://github.com/DebonairSM/Aviation.git
cd Aviation
```

### 2. SSL Certificate Setup

For HTTPS development, you'll need to create a development certificate:

```powershell
dotnet dev-certs https --clean
dotnet dev-certs https --trust
```

### 3. Configuration

#### Azure Key Vault Setup

The project uses Azure Key Vault to securely store sensitive configuration values. The following secrets are required:

1. **JWT Settings**
   - Name: `JwtSettings--SecretKey`
   - Purpose: Secret key for JWT token generation and validation
   - Value: A secure random string (minimum 16 characters)

2. **Microsoft Entra ID Settings**
   - Name: `MicrosoftEntraId--ClientSecret`
   - Purpose: Client secret for Microsoft Entra ID authentication
   - Value: The client secret from your Microsoft Entra ID application registration

To set up Azure Key Vault:

1. Create the Key Vault:
```powershell
az keyvault create --name vsol-aviation-kv --resource-group aviation-rg --location eastus --enabled-for-deployment --enabled-for-disk-encryption --enabled-for-template-deployment --sku standard
```

2. Assign necessary roles:
```powershell
# Get your user's object ID
az ad signed-in-user show --query id -o tsv

# Assign Key Vault Secrets Officer role
az role assignment create --role "Key Vault Secrets Officer" --assignee "YOUR_OBJECT_ID" --scope "/subscriptions/YOUR_SUBSCRIPTION_ID/resourceGroups/aviation-rg/providers/Microsoft.KeyVault/vaults/vsol-aviation-kv"
```

3. Store secrets:
```powershell
# Store JWT secret key
az keyvault secret set --vault-name vsol-aviation-kv --name "JwtSettings--SecretKey" --value "SuperSecretJWTKey_2025!@#"

# Store Microsoft Entra ID client secret
az keyvault secret set --vault-name vsol-aviation-kv --name "MicrosoftEntraId--ClientSecret" --value "M3tr1c4ClientSecret_2025$"
```

4. Verify stored secrets:
```powershell
# List all secrets in the Key Vault
az keyvault secret list --vault-name vsol-aviation-kv --output table
```

The application is configured to use these secrets through Key Vault references in `appsettings.json`:

```json
{
  "JwtSettings": {
    "SecretKey": "@Microsoft.KeyVault(SecretUri=https://vsol-aviation-kv.vault.azure.net/secrets/JwtSettings--SecretKey/)"
  },
  "MicrosoftEntraId": {
    "ClientSecret": "@Microsoft.KeyVault(SecretUri=https://vsol-aviation-kv.vault.azure.net/secrets/MicrosoftEntraId--ClientSecret/)"
  }
}
```

#### Development Authentication

The project uses a development authentication system that mimics Azure AD behavior without requiring cloud resources. This is controlled by the `UseDevAuthentication` setting in `appsettings.Development.json`.

Three development user roles are preconfigured:
1. **Admin User**
   - Email: admin@dev.local
   - Role: Admin
   - Access: Full system administration

2. **Internal User**
   - Email: internal@dev.local
   - Role: InternalUser
   - Access: Internal documents and analytics

3. **External Partner**
   - Email: external@dev.local
   - Role: ExternalPartner
   - Access: Product catalog and order management

To switch between roles during development, modify the order of users in the `DevAuth:Users` array in `appsettings.Development.json`. The first user in the array will be used as the authenticated user.

#### Environment Variables

The following environment variables need to be set:

```plaintext
ASPNETCORE_ENVIRONMENT=Development
ASPNETCORE_URLS=https://+:443;http://+:80
ASPNETCORE_Kestrel__Certificates__Default__Password=YourSecurePassword
UseDevAuthentication=true
```

You can set these in:
- Docker: Using the docker-compose.yml file
- Local Development: Using User Secrets or appsettings.json
- Production: Using your deployment platform's configuration system

### 4. Running the Application

#### Using Docker

```powershell
docker-compose up --build
```

The API will be available at:
- HTTP: http://localhost:8080
- HTTPS: https://localhost:8443

#### Using Visual Studio

1. Open `EnterpriseApiIntegration.sln`
2. Set `EnterpriseApiIntegration.Api` as the startup project
3. Press F5 to run the application

#### Using .NET CLI

```powershell
cd EnterpriseApiIntegration.Api
dotnet run
```

## API Documentation

The API documentation is available at:
- Swagger UI: `https://localhost:8443/swagger`
- OpenAPI JSON: `https://localhost:8443/swagger/v1/swagger.json`

### Available Endpoints

#### Admin Endpoints (`/api/v1/admin`)
- GET `/users` - Get all users
- POST `/users/{userId}/role` - Update user role
- GET `/system/settings` - Get system settings

#### Internal User Endpoints (`/api/v1/internal`)
- GET `/documents` - Get internal documents
- POST `/documents` - Create internal document
- GET `/reports/analytics` - Get analytics data

#### Partner Endpoints (`/api/v1/partner`)
- GET `/products` - Get product catalog
- POST `/orders` - Create new order
- GET `/orders/{orderId}/status` - Check order status
- GET `/account/profile` - Get partner profile

### Testing Different Roles

1. Open `appsettings.Development.json`
2. Locate the `DevAuth:Users` array
3. Move the desired user to the first position in the array
4. Restart the application
5. Use Swagger UI to test the endpoints

Example for testing as Internal User:
```json
{
  "DevAuth": {
    "Users": [
      {
        "Id": "dev-internal",
        "Name": "Development Internal User",
        "Email": "internal@dev.local",
        "Roles": ["InternalUser"]
      },
      // ... other users
    ]
  }
}
```

## Authentication and Authorization

The API uses policy-based authorization with the following policies:

- `RequireInternalUserRole`: For internal users
- `RequireAdminRole`: For administrative access
- `RequireExternalPartnerRole`: For external partner access

In development, authentication is handled by the `DevAuthHandler` which automatically authenticates requests with the first user in the configuration.

For production deployment, update the authentication configuration to use Azure AD by setting `UseDevAuthentication=false` and providing valid Azure AD credentials in the configuration.

## Development Notes

1. The project uses Docker for containerization
2. Health checks are configured at `/health` endpoint
3. HTTPS is enabled by default in development
4. Authentication can be toggled using the `UseDevAuthentication` setting

## Testing

```powershell
dotnet test
```

The solution includes:
- Unit Tests
- Integration Tests
- API Tests

## Contributing

1. Create a feature branch
2. Make your changes
3. Submit a pull request

### Coding Standards

- Follow Microsoft's C# Coding Conventions
- Use async/await for asynchronous operations
- Include XML documentation for public APIs
- Write unit tests for new features

## License

MIT License - See LICENSE file for details