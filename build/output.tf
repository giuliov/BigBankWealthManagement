### OUTPUTS

output "hostname" {
  value = "${azurerm_function_app.bbwm.default_hostname}"
}

output "ip_to_whitelist" {
  value = "${azurerm_function_app.bbwm.outbound_ip_addresses}"
}

output "publish_credential" {
  value = "${azurerm_function_app.bbwm.site_credential}"
}

# EOF

