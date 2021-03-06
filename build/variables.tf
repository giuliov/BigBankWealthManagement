### VARIABLES

variable "env_name" {
  description = "Name of Environment"
  default     = "bbwm"
}

variable "resource_group_location" {
  default = "northeurope"
}

variable "paired_location" {
  default = "westeurope"
}

variable "dns_domain" {
  description = "DNS domain"
  default     = "casavian.eu"
}

variable "alphavantage_apikey" {
  description = "AlphAvantage ApiKey"
}

# EOF #

