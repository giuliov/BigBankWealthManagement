### MAIN

terraform {
  required_version = "~> 0.11.1"

  backend "azurerm" {
    resource_group_name  = "pro-demo"
    storage_account_name = "terraformfun"
    container_name       = "bigbankwealthmanagement"
    key                  = "terraform.tfstate"
  }
}

provider "azurerm" {
  version = "~> 1.0"
}

provider "external" {
  version = "~> 1.0"
}

# EOF

