// App Service Plan module - Free tier (F1)
@description('Name of the App Service Plan')
param appServicePlanName string

@description('Location for the App Service Plan')
param location string

@description('Resource tags')
param tags object

// Create App Service Plan - Free tier (F1)
resource appServicePlan 'Microsoft.Web/serverfarms@2022-03-01' = {
  name: appServicePlanName
  location: location
  tags: tags
  sku: {
    name: 'F1'
    tier: 'Free'
    size: 'F1'
    family: 'F'
    capacity: 1
  }
  kind: 'windows'
  properties: {
    perSiteScaling: false
    elasticScaleEnabled: false
    maximumElasticWorkerCount: 1
    isSpot: false
    reserved: false
    isXenon: false
    hyperV: false
    targetWorkerCount: 0
    targetWorkerSizeId: 0
    zoneRedundant: false
  }
}

// Output the App Service Plan ID
output appServicePlanId string = appServicePlan.id 
