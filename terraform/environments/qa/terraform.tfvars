environment = "qa"
location = "eastus"
resource_group_name = "rg-aviation-qa"
app_service_plan_sku = "B1"

# OAuth2 Configuration
tenant_id          = "your-tenant-id"
api_id             = "api://your-api-id"
client_id          = "your-client-id"
client_secret      = "your-client-secret"
scope              = "api://your-api-id/access_as_user"
authorization_url  = "https://login.microsoftonline.com/your-tenant-id/oauth2/v2.0/authorize"
token_url          = "https://login.microsoftonline.com/your-tenant-id/oauth2/v2.0/token"

# Backend Configuration
backend_host       = "api.aviation.com"

# API Configuration
apis = {
  aircraft = {
    name         = "aircraft-api"
    display_name = "Aircraft API"
    path         = "aircraft"
    protocols    = ["https"]
    service_url  = "http://localhost:5001"
    swagger_file = "aircraft.json"
    is_public    = false
    requires_auth = true
  }
  customers = {
    name         = "customers-api"
    display_name = "Customers API"
    path         = "customers"
    protocols    = ["https"]
    service_url  = "http://localhost:5003"
    swagger_file = "customers.json"
    is_public    = false
    requires_auth = true
  }
  subscriptions = {
    name         = "subscriptions-api"
    display_name = "Subscriptions API"
    path         = "subscriptions"
    protocols    = ["https"]
    service_url  = "http://localhost:5004"
    swagger_file = "subscriptions.json"
    is_public    = false
    requires_auth = true
  }
  identity = {
    name         = "identity-api"
    display_name = "Identity API"
    path         = "identity"
    protocols    = ["https"]
    service_url  = "http://localhost:5001"
    swagger_file = "identity.json"
    is_public    = true
    requires_auth = false
  }
}

tags = {
  Environment = "QA"
  ManagedBy   = "Terraform"
  Project     = "Aviation"
}

subscription_id = "08398d64-5d63-4da9-8daf-e15b00f4d227"
tenant_id       = "621db766-1ccd-40f1-80b0-ef469bd8f081"
client_id       = "e96126b4-d4f9-4716-adac-afa697eee97d"
client_secret   = "cVs8Q~KqdWhBZARCdcCHa6IGqHQiDQG8O2_sgb5o" 