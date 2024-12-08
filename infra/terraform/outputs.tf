locals {
  ollama_fqdn = "http://${local.ollama_service_name}.${var.location}.cloudapp.azure.com:${var.ollama_port}"
}

output "ollama_fqdn" {
  description = "The fully qualified domain name (FQDN) of the Ollama service."
  value       = local.ollama_fqdn
}

output "resource_group_name" {
  description = "The name of the resource group."
  value       = local.resource_group_name
}

output "aks_name" {
  description = "The name of the Azure Kubernetes Service (AKS)."
  value       = local.aks_name
}
