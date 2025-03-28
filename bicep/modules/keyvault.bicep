// Key Vault module
@description('Name of the Key Vault')
param keyVaultName string

@description('Location for the Key Vault')
param location string

@description('Resource tags')
param tags object

@description('Principal IDs to grant access policies to')
param accessPolicyPrincipalIds array = []

// Create Key Vault
resource keyVault 'Microsoft.KeyVault/vaults@2022-07-01' = {
  name: keyVaultName
  location: location
  tags: tags
  properties: {
    sku: {
      family: 'A'
      name: 'standard'
    }
    tenantId: subscription().tenantId
    enableRbacAuthorization: false
    enableSoftDelete: true
    softDeleteRetentionInDays: 90
    networkAcls: {
      defaultAction: 'Allow'
      bypass: 'AzureServices'
      ipRules: []
      virtualNetworkRules: []
    }
    publicNetworkAccess: 'Enabled'
    accessPolicies: [
      // Allow Azure services to access the Key Vault
      {
        tenantId: subscription().tenantId
        objectId: '00000000-0000-0000-0000-000000000000' // System-assigned managed identity
        permissions: {
          secrets: [
            'get'
            'list'
            'set'
          ]
        }
      }
    ]
  }
}

// Add a placeholder connection string secret
resource defaultConnectionSecret 'Microsoft.KeyVault/vaults/secrets@2022-07-01' = {
  parent: keyVault
  name: 'DefaultConnection'
  properties: {
    value: 'Server=placeholder;Database=Aviation;User Id=aviation;Password=placeholder;'
  }
}

// Enable diagnostic settings
resource keyVaultDiagnostics 'Microsoft.Insights/diagnosticSettings@2021-05-01-preview' = {
  name: '${keyVault.name}-diagnostics'
  scope: keyVault
  properties: {
    logs: [
      {
        category: 'AuditEvent'
        enabled: true
        retentionPolicy: {
          enabled: true
          days: 30
        }
      }
    ]
    metrics: [
      {
        category: 'AllMetrics'
        enabled: true
        retentionPolicy: {
          enabled: true
          days: 30
        }
      }
    ]
    workspaceId: '' // Optional: Add Log Analytics workspace ID if you want to send logs there
  }
}

// Output the Key Vault URI
output keyVaultUri string = keyVault.properties.vaultUri 
