# storage account for functions
# based on official documentation functions need storage account
resource "azurerm_storage_account" "functions" {
  name                     = "st${var.application_name}${var.environment_name}${random_string.name.result}"
  resource_group_name      = azurerm_resource_group.main.name
  location                 = azurerm_resource_group.main.location
  account_tier             = "Standard"
  account_replication_type = "LRS"
}

# important so we can set IAM for our function app!
# by setting managed identity we obtain objectID for our function app
resource "azurerm_user_assigned_identity" "functions" {
  location            = azurerm_resource_group.main.location
  name                = "mi-${var.application_name}-${var.environment_name}-fn"
  resource_group_name = azurerm_resource_group.main.name
}

# required so we know within which plan to create a function app
resource "azurerm_service_plan" "main" {
  name                = "asp-${var.application_name}-${var.environment_name}"
  resource_group_name = azurerm_resource_group.main.name
  location            = azurerm_resource_group.main.location
  os_type             = "Linux"
  sku_name            = "Y1"
}

resource "azurerm_linux_function_app" "foo" {
  name                = "func-${var.application_name}-${var.environment_name}-foo-${random_string.name.result}"
  resource_group_name = azurerm_resource_group.main.name
  location            = azurerm_resource_group.main.location

  storage_account_name       = azurerm_storage_account.functions.name
  storage_account_access_key = azurerm_storage_account.functions.primary_access_key
  service_plan_id            = azurerm_service_plan.main.id

  
  site_config {
    application_insights_key = azurerm_application_insights.main.instrumentation_key
    application_insights_connection_string = azurerm_application_insights.main.connection_string
    application_stack {
      # version of our .NET application
      dotnet_version = "6.0"
    }
    # cross-origin resource sharing
    cors {
      # specifies which origins are allowed to acces the function app
      allowed_origins     = ["https://portal.azure.com"]
      # states whether credentials(cookies) are supported
      support_credentials = true
    }
  }

  # app_settings are key value pairs
  app_settings = {
    # app should run from deployment package
    "WEBSITE_RUN_FROM_PACKAGE"       = 1
    # allocates key for Azure Application Insights so we can monitor our function app
    # "APPINSIGHTS_INSTRUMENTATIONKEY" = azurerm_application_insights.main.instrumentation_key
  }

  # configures managed identity for the function app
  identity {
    # sets both types
    type         = "SystemAssigned, UserAssigned"
    # specifies IDs of the user-assigned identities that will be associated with the function app
    identity_ids = [azurerm_user_assigned_identity.functions.id]
  }
}
