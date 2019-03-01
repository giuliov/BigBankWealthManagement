resource "azurerm_resource_group" "bbwm" {
  name     = "${var.env_name}"
  location = "${var.resource_group_location}"

  tags {
    environment = "${var.env_name}"
  }
}
resource "random_id" "server" {
  keepers = {
    # Generate a unique id based on Resource Group name
    rg_id = "${azurerm_resource_group.bbwm.name}"
  }

  byte_length = 8
}

resource "azurerm_storage_account" "bbwm" {
  name                     = "bbwm${random_id.server.hex}"
  resource_group_name      = "${azurerm_resource_group.bbwm.name}"
  location                 = "${azurerm_resource_group.bbwm.location}"
  account_tier             = "Standard"
  account_replication_type = "LRS"
}

resource "azurerm_app_service_plan" "bbwm" {
  name                = "bbwm-appsvc-plan"
  location            = "${azurerm_resource_group.bbwm.location}"
  resource_group_name = "${azurerm_resource_group.bbwm.name}"
  kind                = "FunctionApp"

  # TODO parametrize
  sku {
    tier = "Dynamic"
    size = "Y1"
  }
}

resource "azurerm_application_insights" "bbwm" {
  name                = "bbwm-appinsights"
  location            = "${azurerm_resource_group.bbwm.location}"
  resource_group_name = "${azurerm_resource_group.bbwm.name}"
  application_type    = "Web"
}

resource "azurerm_function_app" "bbwm" {
  name                      = "bbwm"
  location                  = "${azurerm_resource_group.bbwm.location}"
  resource_group_name       = "${azurerm_resource_group.bbwm.name}"
  app_service_plan_id       = "${azurerm_app_service_plan.bbwm.id}"
  storage_connection_string = "${azurerm_storage_account.bbwm.primary_connection_string}"
  https_only = true
  version = "~2"

  app_settings {
    "APPINSIGHTS_INSTRUMENTATIONKEY" = "${azurerm_application_insights.bbwm.instrumentation_key}"
  }
}