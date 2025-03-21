variable "location" {
  description = "The location where the API Management instance will be created"
  type        = string
}

variable "resource_group_name" {
  description = "The name of the resource group"
  type        = string
}

variable "tags" {
  description = "Tags to be applied to the API Management instance"
  type        = map(string)
  default     = {}
}

# OAuth2 Configuration
variable "tenant_id" {
  description = "The Azure AD tenant ID for JWT validation"
  type        = string
}

variable "api_id" {
  description = "The API ID for JWT validation"
  type        = string
}

variable "client_id" {
  description = "The OAuth2 client ID"
  type        = string
}

variable "client_secret" {
  description = "The OAuth2 client secret"
  type        = string
  sensitive   = true
}

variable "scope" {
  description = "The OAuth2 scope"
  type        = string
}

# API Configuration
variable "backend_host" {
  description = "The backend service host"
  type        = string
}

variable "authorization_url" {
  description = "The OAuth2 authorization URL"
  type        = string
}

variable "token_url" {
  description = "The OAuth2 token URL"
  type        = string
}

# Policy Configuration
variable "policy_file_path" {
  description = "Path to the policy XML file"
  type        = string
  default     = "policies/jwt-validation.xml"
}

variable "rate_limit_policy_path" {
  description = "Path to the rate limit policy XML file"
  type        = string
  default     = "policies/rate-limit.xml"
}

variable "ssl_offload_policy_path" {
  description = "Path to the SSL offload policy XML file"
  type        = string
  default     = "policies/ssl-offload.xml"
}

variable "oauth2_policy_path" {
  description = "Path to the OAuth2 policy XML file"
  type        = string
  default     = "policies/oauth2.xml"
}

# API Configuration
variable "apis" {
  description = "Map of API configurations"
  type = map(object({
    name         = string
    display_name = string
    path         = string
    protocols    = list(string)
    service_url  = string
    swagger_file = string
    is_public    = bool
    requires_auth = bool
  }))
  default = {}
} 