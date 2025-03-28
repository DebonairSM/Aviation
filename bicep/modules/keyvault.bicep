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
    accessPolicies: [for principalId in accessPolicyPrincipalIds: {
      tenantId: subscription().tenantId
      objectId: principalId
      permissions: {
        secrets: [
          'get'
          'list'
        ]
      }
    }]
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

// Output the Key Vault URI
output keyVaultUri string = keyVault.properties.vaultUri 
