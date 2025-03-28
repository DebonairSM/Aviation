# Bicep Deployment for Aviation API Platform

This repository contains Bicep templates and GitHub Actions workflows to deploy the Aviation API platform to Azure.

## Prerequisites

- Azure subscription
- GitHub repository with permissions to create secrets
- Azure AD (Microsoft Entra ID) application registration for authentication

## Setup Instructions

### 1. Create Azure Service Principal

Run the following Azure CLI commands to create a service principal with Contributor access:

### Use "Azure Cloud Shell (Bash)" in Terminal
```bash
# Login to Azure
info@vsol.software
az login

Subscription: Azure Subscription
ID: 3e49ee00-6b52-44a2-9bcb-ae483a302d96
Tenant: vsol.software

az ad sp create-for-rbac \
    --name "AviationBicepDeployment" \
    --role contributor \
    --scopes /subscriptions/3e49ee00-6b52-44a2-9bcb-ae483a302d96 \
    --sdk-auth
```

The command will output JSON similar to:

```json
{
  "clientId": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx",
  "clientSecret": "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx",
  "subscriptionId": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx",
  "tenantId": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx",
  ...
}
```

### 2. Configure GitHub Secrets

Add the following secrets to your GitHub repository:

- `AZURE_SUBSCRIPTION_ID`: Your Azure subscription ID
- `AZURE_TENANT_ID`: Your Azure tenant ID
- `AZURE_CLIENT_ID`: The client ID from the service principal
- `AZURE_CLIENT_SECRET`: The client secret from the service principal
- `AZURE_AD_AUDIENCE`: The audience (application ID URI) for your Azure AD app

### 3. Update Ocelot Configuration

We've created an `ocelot.Azure.json` file specifically for the Azure environment. This file contains the routes for your API services in Azure.

Make sure to update your API.Gateway application to use this file when running in Azure. You can do this by modifying your `Program.cs` file to check the environment:

```csharp
// Use environment-specific Ocelot config
if (app.Environment.IsProduction())
{
    builder.Configuration.AddJsonFile("ocelot.Azure.json", optional: false, reloadOnChange: true);
}
else
{
    builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
}
```

### 4. Run the Deployment

The deployment can be triggered by:

1. Pushing to the main branch
2. Manually triggering the workflow in GitHub Actions UI

## Infrastructure Details

The Bicep template deploys the following resources:

- Resource Group: `rg-aviation-bicep-dev`
- App Service Plan: `asp-aviation-bicep-dev` (Free F1 tier)
- API Gateway: `api-gateway-aviation-dev`
- Aircraft Service: `api-aircraft-aviation-dev`
- Key Vault: `kv-aviation-bicep-dev`

## Connection Strings

The connection string for the database is stored in Key Vault as a secret named `DefaultConnection`. In the initial deployment, a placeholder value is used. You should update this secret with the actual connection string after deployment.

```bash
az keyvault secret set --vault-name kv-aviation-bicep-dev --name DefaultConnection --value "YOUR_ACTUAL_CONNECTION_STRING"
```

## Troubleshooting

If you encounter any issues with the deployment, check the GitHub Actions logs for detailed error messages.

Common issues:
- Service Principal doesn't have sufficient permissions
- Azure AD application configuration is incorrect
- Missing GitHub Secrets
- Resource name conflicts (already existing resources) 