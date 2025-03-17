variable "location" {
  description = "The location where the APIM instance will be created"
  type        = string
}

variable "resource_group_name" {
  description = "The name of the resource group"
  type        = string
}

variable "tags" {
  description = "Tags to be applied to the APIM instance"
  type        = map(string)
  default = {
    Environment = "QA"
    ManagedBy   = "Terraform"
    Project     = "Aviation"
  }
} 