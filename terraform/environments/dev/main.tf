terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~> 3.0"
    }
  }
}

provider "azurerm" {
  features {}
  skip_provider_registration = true
}

module "web_apps" {
  source = "../../modules/app-service"

  environment          = "dev"
  resource_group      = "rg-aviation-dev"
  location            = "eastus2"
  app_service_plan_sku = "B1"  # Using B1 for dev, can be changed to a smaller SKU if needed

  apps = {
    identity = {
      name = "aviation-identity-dev"
      settings = {
        "KeyVaultName" = "kv-aviation-dev"
        "ASPNETCORE_ENVIRONMENT" = "Development"
      }
    }
    aircraft = {
      name = "aviation-aircraft-dev"
      settings = {
        "KeyVaultName" = "kv-aviation-dev"
        "ASPNETCORE_ENVIRONMENT" = "Development"
      }
    }
    customers = {
      name = "aviation-customers-dev"
      settings = {
        "KeyVaultName" = "kv-aviation-dev"
        "ASPNETCORE_ENVIRONMENT" = "Development"
      }
    }
    subscriptions = {
      name = "aviation-subscriptions-dev"
      settings = {
        "KeyVaultName" = "kv-aviation-dev"
        "ASPNETCORE_ENVIRONMENT" = "Development"
      }
    }
  }

  tags = {
    Environment = "Development"
    ManagedBy   = "Terraform"
    Project     = "Aviation"
  }
} 