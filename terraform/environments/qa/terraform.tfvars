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

subscription_id = "08398d64-5d63-4da9-8daf-e15b00f4d227"
tenant_id       = "621db766-1ccd-40f1-80b0-ef469bd8f081"
client_id       = "e96126b4-d4f9-4716-adac-afa697eee97d"
client_secret   = "cVs8Q~KqdWhBZARCdcCHa6IGqHQiDQG8O2_sgb5o" 