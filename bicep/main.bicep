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
param azureAdInstance string = environment().authentication.loginEndpoint

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

// Create Resource Group at subscription scope
resource resourceGroup 'Microsoft.Resources/resourceGroups@2021-04-01' = {
  name: resourceGroupName
  location: location
  tags: tags
  properties: {
    managedBy: null
  }
}

// Deploy all other resources in the resource group
module resources 'modules/resources.bicep' = {
  name: 'resourcesDeployment'
  scope: resourceGroup
  params: {
    location: location
    tags: tags
    appServicePlanName: appServicePlanName
    gatewayAppName: gatewayAppName
    aircraftServiceAppName: aircraftServiceAppName
    keyVaultName: keyVaultName
    tenantId: tenantId
    audience: audience
    azureAdInstance: azureAdInstance
  }
}

// Output important information
output resourceGroupName string = resourceGroupName
output gatewayUrl string = resources.outputs.gatewayUrl
output aircraftServiceUrl string = resources.outputs.aircraftServiceUrl
output keyVaultName string = keyVaultName 
