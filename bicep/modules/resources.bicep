// resources.bicep - Module for deploying all resources in the resource group
@description('Location for all resources')
param location string

@description('Resource tags')
param tags object

@description('App Service Plan name')
param appServicePlanName string

@description('Gateway App name')
param gatewayAppName string

@description('Aircraft Service App name')
param aircraftServiceAppName string

@description('Key Vault name')
param keyVaultName string

@description('Azure AD tenant ID')
param tenantId string

@description('Azure AD audience')
param audience string

@description('Azure AD instance')
param azureAdInstance string

// Deploy Key Vault
module keyVault 'keyvault.bicep' = {
  name: 'keyVaultDeployment'
  params: {
    keyVaultName: keyVaultName
    location: location
    tags: tags
  }
}

// Deploy App Service Plan
module appServicePlan 'appserviceplan.bicep' = {
  name: 'appServicePlanDeployment'
  params: {
    appServicePlanName: appServicePlanName
    location: location
    tags: tags
  }
}

// Deploy API Gateway
module gatewayApp 'webapp.bicep' = {
  name: 'gatewayAppDeployment'
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
      {
        name: 'Services:AircraftService'
        value: 'https://${aircraftServiceApp.outputs.appUrl}'
      }
    ]
  }
}

// Deploy Aircraft Service
module aircraftServiceApp 'webapp.bicep' = {
  name: 'aircraftServiceDeployment'
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
output gatewayUrl string = gatewayApp.outputs.appUrl
output aircraftServiceUrl string = aircraftServiceApp.outputs.appUrl 
