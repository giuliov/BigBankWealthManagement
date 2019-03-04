resource "azurerm_resource_group" "bbwm" {
  name     = "${var.env_name}"
  location = "${var.resource_group_location}"

  tags {
    environment = "${var.env_name}"
  }
}
resource "random_id" "bbwm" {
  keepers = {
    # Generate a unique id based on Resource Group name
    rg_id = "${azurerm_resource_group.bbwm.name}"
  }

  byte_length = 8
}

resource "azurerm_storage_account" "bbwm" {
  name                     = "bbwm${random_id.bbwm.hex}"
  resource_group_name      = "${azurerm_resource_group.bbwm.name}"
  location                 = "${azurerm_resource_group.bbwm.location}"
  account_tier             = "Standard"
  account_replication_type = "LRS"

  tags {
    environment = "${var.env_name}"
  }
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

  tags {
    environment = "${var.env_name}"
  }
}

resource "azurerm_application_insights" "bbwm" {
  name                = "bbwm-appinsights"
  location            = "${azurerm_resource_group.bbwm.location}"
  resource_group_name = "${azurerm_resource_group.bbwm.name}"
  application_type    = "Web"

  tags {
    environment = "${var.env_name}"
  }
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

  tags {
    environment = "${var.env_name}"
  }
}

resource "azurerm_cosmosdb_account" "bbwm" {
  name                = "bbwm${random_id.bbwm.hex}-cosmosdb"
  location            = "${azurerm_resource_group.bbwm.location}"
  resource_group_name = "${azurerm_resource_group.bbwm.name}"
  offer_type          = "Standard"
  kind                = "GlobalDocumentDB"

  enable_automatic_failover = true

  consistency_policy {
    consistency_level       = "BoundedStaleness"
    max_interval_in_seconds = 10
    max_staleness_prefix    = 200
  }

  geo_location {
    location          = "${var.paired_location}"
    failover_priority = 1
  }

  geo_location {
    prefix            = "bbwm${random_id.bbwm.hex}-cosmosdb-customid"
    location          = "${azurerm_resource_group.bbwm.location}"
    failover_priority = 0
  }

  tags {
    environment = "${var.env_name}"
  }
}