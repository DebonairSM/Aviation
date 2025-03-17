terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~> 3.0"
    }
  }
}

provider "azurerm" {
  subscription_id = var.subscription_id
  tenant_id       = var.tenant_id
  client_id       = var.client_id
  client_secret   = var.client_secret
  features {}
}

module "web_apps" {
  source = "../../modules/app-service"

  environment          = "qa"
  resource_group      = "aviation-rg"
  location            = "westus"
  app_service_plan_sku = "F1"

  apps = {
    identity = {
      name = "aviation-identity-qa"
      settings = {
        "KeyVaultName" = "vsol-aviation-kv"
        "ASPNETCORE_ENVIRONMENT" = "QA"
      }
    }
    aircraft = {
      name = "aviation-aircraft-qa"
      settings = {
        "KeyVaultName" = "vsol-aviation-kv"
        "ASPNETCORE_ENVIRONMENT" = "QA"
      }
    }
    customers = {
      name = "aviation-customers-qa"
      settings = {
        "KeyVaultName" = "vsol-aviation-kv"
        "ASPNETCORE_ENVIRONMENT" = "QA"
      }
    }
    subscriptions = {
      name = "aviation-subscriptions-qa"
      settings = {
        "KeyVaultName" = "vsol-aviation-kv"
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

module "api_management" {
  source = "../../modules/api-management"

  location            = "westus"
  resource_group_name = "aviation-rg"

  tags = {
    Environment = "QA"
    ManagedBy   = "Terraform"
    Project     = "Aviation"
  }
} 