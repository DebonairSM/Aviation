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
    enable_backend_ssl30  = false
    enable_triple_des_ciphers = false
  }
  virtual_network_type = "None"
  tags = var.tags
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