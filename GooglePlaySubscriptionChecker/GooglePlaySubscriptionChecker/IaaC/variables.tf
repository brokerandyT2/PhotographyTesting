# variables.tf
variable "resource_group_name" {
  description = "The name of the resource group."
  default     = "aks-rg"
}

variable "aks_name" {
  description = "The name of the AKS cluster."
  default     = "aks-cluster"
}

variable "location" {
  description = "The Azure location."
  default     = "EastUS"
}

variable "dns_zone_name" {
  description = "The DNS zone name."
  default     = "example.com"
}

variable "domain_name" {
  description = "The domain name for the container access."
  default     = "myapp"
}

variable "node_count" {
  description = "The initial number of nodes in the AKS cluster."
  default     = 3
}

variable "node_size" {
  description = "The size of the nodes in the AKS cluster."
  default     = "Standard_DS2_v2"
}

variable "container_image" {
  description = "The container image to run."
  default     = "nginx:latest"
}
