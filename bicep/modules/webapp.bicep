// Web App module
@description('Name of the App')
param appName string

@description('Location for the App')
param location string

@description('App Service Plan ID')
param appServicePlanId string

@description('App Settings')
param appSettings array = []

@description('Resource tags')
param tags object

// Create Web App
resource webApp 'Microsoft.Web/sites@2022-03-01' = {
  name: appName
  location: location
  tags: tags
  properties: {
    serverFarmId: appServicePlanId
    siteConfig: {
      netFrameworkVersion: 'v8.0'
      appSettings: appSettings
      webSocketsEnabled: true
      alwaysOn: false // Set to false for Free tier
      http20Enabled: true
      minTlsVersion: '1.2'
      ftpsState: 'Disabled'
      healthCheckPath: '/health'
    }
    httpsOnly: true
    clientAffinityEnabled: false
    publicNetworkAccess: 'Enabled'
  }
  identity: {
    type: 'SystemAssigned'
  }
}

// Output the Web App URL
output appUrl string = webApp.properties.defaultHostName
output appIdentityPrincipalId string = webApp.identity.principalId 
