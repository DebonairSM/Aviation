// main.bicep - Main deployment file for Aviation API platform
@description('Environment name')
param environmentName string = 'dev'

@description('Location for all resources')
param location string = 'eastus'

@description('Azure AD tenant ID')
param tenantId string

@description('Azure AD audience')
param audience string

@description('Azure AD instance')
param azureAdInstance string = 'https://login.microsoftonline.com/'

var tags = {
  Environment: environmentName
  DeployedBy: 'Bicep'
  Project: 'Aviation'
}

// Resource names
var resourceGroupName = 'rg-aviation-bicep-${environmentName}'
var appServicePlanName = 'asp-aviation-bicep-${environmentName}'
var gatewayAppName = 'api-gateway-aviation-${environmentName}'
var aircraftServiceAppName = 'api-aircraft-aviation-${environmentName}'
var keyVaultName = 'kv-aviation-bicep-${environmentName}'

// Create Resource Group
resource resourceGroup 'Microsoft.Resources/resourceGroups@2021-04-01' = {
  name: resourceGroupName
  location: location
  tags: tags
}

// Deploy Key Vault
module keyVault 'modules/keyvault.bicep' = {
  name: 'keyVaultDeployment'
  scope: resourceGroup
  params: {
    keyVaultName: keyVaultName
    location: location
    tags: tags
  }
}

// Deploy App Service Plan
module appServicePlan 'modules/appserviceplan.bicep' = {
  name: 'appServicePlanDeployment'
  scope: resourceGroup
  params: {
    appServicePlanName: appServicePlanName
    location: location
    tags: tags
  }
}

// Deploy API Gateway
module gatewayApp 'modules/webapp.bicep' = {
  name: 'gatewayAppDeployment'
  scope: resourceGroup
  params: {
    appName: gatewayAppName
    location: location
    appServicePlanId: appServicePlan.outputs.appServicePlanId
    tags: tags
    appSettings: [
      {
        name: 'AzureAd:TenantId'
        value: tenantId
      }
      {
        name: 'AzureAd:Audience'
        value: audience
      }
      {
        name: 'AzureAd:Instance'
        value: azureAdInstance
      }
      // URL for Aircraft service will be dynamically set
      {
        name: 'Services:AircraftService'
        value: 'https://${aircraftServiceApp.outputs.appUrl}'
      }
    ]
  }
}

// Deploy Aircraft Service
module aircraftServiceApp 'modules/webapp.bicep' = {
  name: 'aircraftServiceDeployment'
  scope: resourceGroup
  params: {
    appName: aircraftServiceAppName
    location: location
    appServicePlanId: appServicePlan.outputs.appServicePlanId
    tags: tags
    appSettings: [
      {
        name: 'ConnectionStrings:DefaultConnection'
        value: '@Microsoft.KeyVault(SecretUri=https://${keyVaultName}.vault.azure.net/secrets/DefaultConnection)'
      }
      {
        name: 'MicrosoftEntraId:TenantId'
        value: tenantId
      }
      {
        name: 'MicrosoftEntraId:Audience'
        value: audience
      }
      {
        name: 'MicrosoftEntraId:Instance'
        value: azureAdInstance
      }
      {
        name: 'KeyVaultName'
        value: keyVaultName
      }
    ]
  }
}

// Output important information
output resourceGroupName string = resourceGroupName
output gatewayUrl string = gatewayApp.outputs.appUrl
output aircraftServiceUrl string = aircraftServiceApp.outputs.appUrl
output keyVaultName string = keyVaultName 
