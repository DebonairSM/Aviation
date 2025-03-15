environment = "qa"
location = "eastus2"
resource_group = "rg-aviation-qa"
app_service_plan_sku = "B1"

apps = {
  identity = {
    name = "aviation-identity-qa"
    settings = {
      "ASPNETCORE_ENVIRONMENT" = "Development"
      "KeyVaultName" = "kv-aviation-qa"
    }
  }
  aircraft = {
    name = "aviation-aircraft-qa"
    settings = {
      "ASPNETCORE_ENVIRONMENT" = "Development"
      "KeyVaultName" = "kv-aviation-qa"
    }
  }
  customers = {
    name = "aviation-customers-qa"
    settings = {
      "ASPNETCORE_ENVIRONMENT" = "Development"
      "KeyVaultName" = "kv-aviation-qa"
    }
  }
  subscriptions = {
    name = "aviation-subscriptions-qa"
    settings = {
      "ASPNETCORE_ENVIRONMENT" = "Development"
      "KeyVaultName" = "kv-aviation-qa"
    }
  }
}

tags = {
  Environment = "QA"
  ManagedBy = "Terraform"
  Project = "Aviation"
} 