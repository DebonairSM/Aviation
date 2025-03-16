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

  environment          = "qa"
  resource_group      = "rg-aviation-qa"
  location            = "eastus2"
  app_service_plan_sku = "B1"

  apps = {
    identity = {
      name = "aviation-identity-qa"
      settings = {
        "KeyVaultName" = "kv-aviation-qa"
        "ASPNETCORE_ENVIRONMENT" = "QA"
      }
    }
    aircraft = {
      name = "aviation-aircraft-qa"
      settings = {
        "KeyVaultName" = "kv-aviation-qa"
        "ASPNETCORE_ENVIRONMENT" = "QA"
      }
    }
    customers = {
      name = "aviation-customers-qa"
      settings = {
        "KeyVaultName" = "kv-aviation-qa"
        "ASPNETCORE_ENVIRONMENT" = "QA"
      }
    }
    subscriptions = {
      name = "aviation-subscriptions-qa"
      settings = {
        "KeyVaultName" = "kv-aviation-qa"
        "ASPNETCORE_ENVIRONMENT" = "QA"
      }
    }
  }

  tags = {
    Environment = "QA"
    ManagedBy   = "Terraform"
    Project     = "Aviation"
  }
} 