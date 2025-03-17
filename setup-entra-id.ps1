# Azure CLI commands to configure Entra ID for the Aircraft Service

# Set environment variables for Azure CLI
Write-Host "Setting up environment variables..."
$env:ARM_CLIENT_ID = "e96126b4-d4f9-4716-adac-afa697eee97d"
$env:ARM_CLIENT_SECRET = "cVs8Q~KqdWhBZARCdcCHa6IGqHQiDQG8O2_sgb5o"
$env:ARM_SUBSCRIPTION_ID = "08398d64-5d63-4da9-8daf-e15b00f4d227"
$env:ARM_TENANT_ID = "621db766-1ccd-40f1-80b0-ef469bd8f081"

# Variables
$tenantId = $env:ARM_TENANT_ID
$clientId = $env:ARM_CLIENT_ID
$appName = "Aircraft Service API"
$apiScope = "access_as_user"

# Login to Azure with service principal
Write-Host "Logging in to Azure with service principal..."
az login --service-principal -u $env:ARM_CLIENT_ID -p $env:ARM_CLIENT_SECRET --tenant $env:ARM_TENANT_ID
az account set --subscription $env:ARM_SUBSCRIPTION_ID

# Ensure Microsoft.Graph is installed
Write-Host "Ensuring Microsoft.Graph extension is installed..."
az extension add --name microsoft-graph

# Update the app registration
Write-Host "Updating app registration..."
az ad app update --id $clientId --display-name $appName --sign-in-audience AzureADMyOrg --enable-id-token-issuance true

# Configure API permissions
Write-Host "Configuring API permissions..."
az ad app permission add --id $clientId --api 00000003-0000-0000-c000-000000000000 --api-permissions "User.Read=Scope" "User.Read.All=Scope"

# Create client secret
Write-Host "Creating client secret..."
$secret = az ad app credential reset --id $clientId --query "password" -o tsv
Write-Host "Client Secret: $secret"
Write-Host "IMPORTANT: Save this secret somewhere safe!"

# Configure API scope
Write-Host "Configuring API scope..."
$apiId = "api://$clientId"
az ad app update --id $clientId --identifier-uris $apiId

# Add API scope
Write-Host "Adding API scope..."
az ad app update --id $clientId `
    --set "api.oauth2PermissionScopes[0].value=$apiScope" `
    --set "api.oauth2PermissionScopes[0].adminConsentDisplayName=Access as user" `
    --set "api.oauth2PermissionScopes[0].adminConsentDescription=Allow the application to access the API on behalf of the signed-in user" `
    --set "api.oauth2PermissionScopes[0].isEnabled=true" `
    --set "api.oauth2PermissionScopes[0].type=User"

# Grant admin consent for the API permissions
Write-Host "Granting admin consent..."
az ad app permission admin-consent --id $clientId

Write-Host "Configuration complete!"
Write-Host "Please save the client secret shown above."
Write-Host "You can now use this configuration in your application."

# Save the configuration to a local file for reference
$config = @{
    TenantId = $tenantId
    ClientId = $clientId
    ApiScope = $apiScope
    ClientSecret = $secret
    ApiId = $apiId
} | ConvertTo-Json

Set-Content -Path "azure-config.json" -Value $config
Write-Host "Configuration saved to azure-config.json" 