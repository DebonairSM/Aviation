# Use existing resource group
data "azurerm_resource_group" "rg" {
  name = "aviation-rg"
}

resource "azurerm_service_plan" "plan" {
  name                = "asp-qa"
  resource_group_name = data.azurerm_resource_group.rg.name
  location           = "westus"
  os_type            = "Windows"
  sku_name           = "F1"

  tags = var.tags
}

resource "azurerm_windows_web_app" "apps" {
  for_each = var.apps

  name                = each.value.name
  resource_group_name = data.azurerm_resource_group.rg.name
  location           = "westus"
  service_plan_id    = azurerm_service_plan.plan.id

  site_config {
    always_on = false
    application_stack {
      dotnet_version = "v8.0"
    }
  }

  app_settings = merge(
    {
      "WEBSITE_RUN_FROM_PACKAGE" = "1"
      "ASPNETCORE_ENVIRONMENT"   = title(var.environment)
      "WEBSITE_NODE_DEFAULT_VERSION" = "~18"
      "WEBSITE_WEBDEPLOY_USE_SCM" = "true"
    },
    each.value.settings
  )

  tags = var.tags
} 