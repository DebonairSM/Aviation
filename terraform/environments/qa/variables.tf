variable "environment" {
  description = "Environment name"
  type        = string
}

variable "location" {
  description = "Azure region"
  type        = string
}

variable "resource_group" {
  description = "Resource group name"
  type        = string
}

variable "app_service_plan_sku" {
  description = "SKU for App Service Plan"
  type        = string
}

variable "apps" {
  description = "Map of web apps to create"
  type = map(object({
    name     = string
    settings = map(string)
  }))
}

variable "tags" {
  description = "Tags to apply to all resources"
  type        = map(string)
}

variable "subscription_id" {
  description = "The Azure Subscription ID"
  type        = string
}

variable "tenant_id" {
  description = "The Azure Tenant ID"
  type        = string
}

variable "client_id" {
  description = "The Azure Client ID"
  type        = string
}

variable "client_secret" {
  description = "The Azure Client Secret"
  type        = string
  sensitive   = true
} 