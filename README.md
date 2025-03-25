# API Gateway

This project implements an API Gateway using Ocelot for routing requests to downstream services. The gateway is configured to handle authentication via Azure Entra ID (formerly Azure Active Directory) and aggregates Swagger documentation for easier API discovery.

## Configuration Overview

### ocelot.json

The `ocelot.json` file contains the configuration for routing requests to downstream services. Key sections include:

- **Routes**: Defines how incoming requests are mapped to downstream services.
  - **DownstreamPathTemplate**: The path on the downstream service.
  - **UpstreamPathTemplate**: The path that clients will use to access the API Gateway.
  - **AuthenticationOptions**: Specifies the use of JWT Bearer tokens for authentication.

- **SwaggerEndPoints**: Configures how the API Gateway aggregates Swagger documentation from downstream services.

### Authentication

The API Gateway uses JWT Bearer tokens for authentication, which are issued by Azure Entra ID. The configuration in `Program.cs` sets up the necessary parameters for validating these tokens, including the authority and audience.

### Running the API Gateway

1. Ensure that the downstream services are running (e.g., the Aircraft service).
2. Start the API Gateway project.
3. Access the Swagger UI at `https://localhost:7500/swagger` to explore the available endpoints.

## Dependencies

- Ocelot
- MMLib.SwaggerForOcelot
- Microsoft.AspNetCore.Authentication.JwtBearer
- Newtonsoft.Json

## Notes

- The `DangerousAcceptAnyServerCertificate` option is set to `true` for development purposes. Ensure to configure proper SSL validation in production environments.
