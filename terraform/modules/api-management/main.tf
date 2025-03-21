resource "azurerm_api_management" "apim" {
  name                = "apim-aviation-qa"
  location            = var.location
  resource_group_name = var.resource_group_name
  publisher_name      = "Aviation"
  publisher_email     = "admin@aviation.com"
  sku_name           = "Developer_1"
  identity {
    type = "SystemAssigned"
  }
  protocols {
    enable_http2 = true
  }
  security {
    enable_backend_ssl30 = false
  }
  virtual_network_type = "None"
  tags = var.tags
}

# Azure AD OAuth2 Server Configuration
resource "azurerm_api_management_oauth2_server" "azure_ad" {
  name                = "azure-ad"
  api_management_name = azurerm_api_management.apim.name
  resource_group_name = var.resource_group_name
  display_name        = "Azure AD"
  client_id           = var.client_id
  client_secret       = var.client_secret
  authorization_endpoint = var.authorization_url
  token_endpoint        = var.token_url
  grant_types          = ["authorizationCode"]
  scopes               = [var.scope]
}

# OAuth2 Server Policy
resource "azurerm_api_management_api_policy" "oauth2_policy" {
  api_name            = azurerm_api_management_api.aircraft.name
  api_management_name = azurerm_api_management.apim.name
  resource_group_name = var.resource_group_name
  xml_content = <<XML
<policies>
  <inbound>
    <oauth2>
      <authorization-code-grant 
        authorization-url="${var.authorization_url}"
        token-url="${var.token_url}"
        client-id="${var.client_id}"
        client-secret="${var.client_secret}"
        scope="${var.scope}"
      />
    </oauth2>
  </inbound>
  <outbound />
  <on-error />
</policies>
XML
}

locals {
  policy_content = templatefile(var.policy_file_path, {
    tenant_id = var.tenant_id
    api_id    = var.api_id
  })
}

resource "azurerm_api_management_api_policy" "aircraft_policy" {
  api_name            = azurerm_api_management_api.aircraft.name
  api_management_name = azurerm_api_management.apim.name
  resource_group_name = var.resource_group_name
  xml_content         = local.policy_content
}

resource "azurerm_api_management_api_policy" "customers_policy" {
  api_name            = azurerm_api_management_api.customers.name
  api_management_name = azurerm_api_management.apim.name
  resource_group_name = var.resource_group_name
  xml_content         = local.policy_content
}

resource "azurerm_api_management_api_policy" "subscriptions_policy" {
  api_name            = azurerm_api_management_api.subscriptions.name
  api_management_name = azurerm_api_management.apim.name
  resource_group_name = var.resource_group_name
  xml_content         = local.policy_content
}

resource "azurerm_api_management_api_policy" "identity_policy" {
  api_name            = azurerm_api_management_api.identity.name
  api_management_name = azurerm_api_management.apim.name
  resource_group_name = var.resource_group_name
  xml_content         = local.policy_content
}

resource "azurerm_api_management_api" "aircraft" {
  name                = "aircraft-api"
  resource_group_name = var.resource_group_name
  api_management_name = azurerm_api_management.apim.name
  revision           = "1"
  display_name       = "Aircraft API"
  path              = "aircraft"
  protocols         = ["https"]
  service_url       = "http://localhost:5001"
  import {
    content_format = "openapi+json"
    content_value  = file("${path.module}/swagger/aircraft.json")
  }
}

resource "azurerm_api_management_api" "customers" {
  name                = "customers-api"
  resource_group_name = var.resource_group_name
  api_management_name = azurerm_api_management.apim.name
  revision           = "1"
  display_name       = "Customers API"
  path              = "customers"
  protocols         = ["https"]
  service_url       = "http://localhost:5003"
  import {
    content_format = "openapi+json"
    content_value  = file("${path.module}/swagger/customers.json")
  }
}

resource "azurerm_api_management_api" "subscriptions" {
  name                = "subscriptions-api"
  resource_group_name = var.resource_group_name
  api_management_name = azurerm_api_management.apim.name
  revision           = "1"
  display_name       = "Subscriptions API"
  path              = "subscriptions"
  protocols         = ["https"]
  service_url       = "http://localhost:5004"
  import {
    content_format = "openapi+json"
    content_value  = file("${path.module}/swagger/subscriptions.json")
  }
}

resource "azurerm_api_management_api" "identity" {
  name                = "identity-api"
  resource_group_name = var.resource_group_name
  api_management_name = azurerm_api_management.apim.name
  revision           = "1"
  display_name       = "Identity API"
  path              = "identity"
  protocols         = ["https"]
  service_url       = "http://localhost:5001"
  import {
    content_format = "openapi+json"
    content_value  = file("${path.module}/swagger/identity.json")
  }
} 