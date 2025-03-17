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

# Login to Azure with service principal
Write-Host "Logging in to Azure with service principal..."
az login --service-principal -u $env:ARM_CLIENT_ID -p $env:ARM_CLIENT_SECRET --tenant $env:ARM_TENANT_ID
az account set --subscription $env:ARM_SUBSCRIPTION_ID

# Update the app registration
Write-Host "Updating app registration..."
az ad app update --id $clientId --display-name $appName --sign-in-audience AzureADMyOrg

# Configure API permissions
Write-Host "Configuring API permissions..."
az rest --method PATCH --uri "https://graph.microsoft.com/v1.0/applications/d5f046b0-db64-47e3-96b3-d2ff434f7354" --headers "Content-Type=application/json" --body '{\"requiredResourceAccess\":[{\"resourceAppId\":\"00000003-0000-0000-c000-000000000000\",\"resourceAccess\":[{\"id\":\"1bfefb4e-e0b5-418b-a88f-73c46d2cc8e9\",\"type\":\"Role\"},{\"id\":\"df021288-bdef-4463-88db-98f22de89214\",\"type\":\"Role\"},{\"id\":\"19dbc75e-c2e2-444c-a770-ec69d8559fc7\",\"type\":\"Role\"}]}]}'

# Create client secret
Write-Host "Creating client secret..."
$secret = az ad app credential reset --id $clientId --append --query "password" -o tsv
Write-Host "Client Secret: $secret"
Write-Host "IMPORTANT: Save this secret somewhere safe!"

# Grant admin consent for the API permissions
Write-Host "Granting admin consent..."
az ad app permission grant --id $clientId --api 00000003-0000-0000-c000-000000000000 --scope "Application.ReadWrite.All Directory.Read.All Directory.ReadWrite.All"

Write-Host "Configuration complete!"
Write-Host "Please save the client secret shown above."
Write-Host "You can now use this configuration in your application."

# Save the configuration to a local file for reference
$config = @{
    TenantId = $tenantId
    ClientId = $clientId
    ClientSecret = $secret
} | ConvertTo-Json

Set-Content -Path "azure-config.json" -Value $config
Write-Host "Configuration saved to azure-config.json" 