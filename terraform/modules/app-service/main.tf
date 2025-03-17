resource "azurerm_resource_group" "rg" {
  name     = var.resource_group
  location = var.location
  tags     = var.tags
}

resource "azurerm_service_plan" "plan" {
  name                = "asp-${var.environment}"
  resource_group_name = azurerm_resource_group.rg.name
  location           = azurerm_resource_group.rg.location
  os_type            = "Windows"
  sku_name           = var.app_service_plan_sku

  tags = var.tags
}

resource "azurerm_windows_web_app" "apps" {
  for_each = var.apps

  name                = each.value.name
  resource_group_name = azurerm_resource_group.rg.name
  location           = azurerm_resource_group.rg.location
  service_plan_id    = azurerm_service_plan.plan.id

  site_config {
    application_stack {
      dotnet_version = "8.0"
    }
    always_on = true
    cors {
      allowed_origins = ["*"]
      allowed_methods = ["GET", "POST", "PUT", "DELETE", "OPTIONS"]
      allowed_headers = ["*"]
    }
  }

  app_settings = merge(
    {
      "WEBSITE_RUN_FROM_PACKAGE" = "1"
      "ASPNETCORE_ENVIRONMENT"   = title(var.environment)
      "WEBSITE_NODE_DEFAULT_VERSION" = "~18"
      "WEBSITE_HTTPLOGGING_RETENTION_DAYS" = "7"
      "WEBSITE_WEBDEPLOY_USE_SCM" = "true"
    },
    each.value.settings
  )

  tags = var.tags
} 