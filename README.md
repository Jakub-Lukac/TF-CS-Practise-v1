# Terraform-Practise-v1

## Project Overview
This project leverages Terraform as Infrastructure as Code (IaC) to deploy Azure resources, including a Windows Function App autonomously. A GitHub Actions pipeline is used to manage the CI/CD process. .NET part of this project consists of Azure Function, Dependency Injection (Interface), and XUnit test.

## Terraform service principal Setup
To run this code, you need to get an Azure Subscription ID. Run the az login command to get a list of available subscriptions.

To create a Terraform service principal, go to portal.azure.com open up the cloud power shell, and run the following command:
```text
az ad sp create-for-rbac -n AzTf --role="Contributor" --scopes="/subscriptions/{your-subcription-id}"
```

From the command output, note the `appId` value and `password` and store them in variables.tf file for **the time being.** Note them as `env_` variables together with *subscription_id* and *tenant_id*.

```text
{
  "appId": "b194bcf7-****-****-****-5a8fed8448ff",
  "displayName": "AzTf",
  "password": "**************************",
  "tenant": "ab22e0f4-****-****-****-9be459c33fb2"
}
```

```text
variable "env_client_id" {
  type    = string
  default = ""
}

variable "env_client_secret" {
  type    = string
  default = ""
}

variable "tenant_id" {
  type    = string
  default = ""
}

variable "subscription_id" {
  type    = string
  default = ""
}
```

## Before run Terraform
Terraform uses Azure AD Application to run commands. For creating new resources, **AzTf** Application needs API Permissions as follows:

```text
Microsoft Graph

  User.Read                 Application
```

After adding permissions use the **Grant admin consent** button to commit permissions.

## Run Terraform

Prepare backend.conf file with the following attributes for storing your terraform.tfstate

```terraform
resource_group_name  = "Your-RG-Name"
storage_account_name = "yourstorageaccount"
container_name       = "your-container-name"
key                  = "terraform.tfstate"
access_key           = "your-access-key"
```

**Later on this file won't be needed, and will be deleted/ignored**</br>
**Its purpose is only for the initial run.**

When you're ready, run the following commands:

```text
terraform init -backend-config=backend.conf
terraform validate
terraform plan -out="plan_main.out" 
```

If there is no error reported, run the `apply` command to deploy the solution for the customer.

```text
terraform apply "plan_main.out"
```

# CI/CD pipeline Setup

## GitHub Variables and Secrets
Navigate to your repository, go to Settings -> Secrets and Variables -> Actions.

### Secrets
In here create secrets **ARM_CLIENT_SECRET** and **BACKEND_ACCESS_KEY**</br>
ARM_CLIENT_SECRET is represented in var.tf file by the env_client_secret variable.]

### Variables

**Important to note**, like client secret, app ID, tenant ID, subscription ID, **MUST** start with phrase **ARM**
```text
ARM_CLIENT_ID
ARM_SUBSCRIPTION_ID
ARM_TENANT_ID
```

**Populate the values from the backend.conf file**
```text
BACKEND_RESOURCE_GROUP_NAME
BACKEND_STORAGE_ACCOUNT_NAME
BACKEND_STORAGE_CONTAINER_NAME
TF_BACKEND_KEY</br>
```

# PostMan

by targeting this endpoint https://{NAME_OF_THE_FUNCTION_APP_FROM_AZURE_PORTAL}/api/foo
we get a message :
This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response.

by targeting this endpoint https://{NAME_OF_THE_FUNCTION_APP_FROM_AZURE_PORTAL}/api/foo?name=Jakub
we get a message :
Hello, Jakub. This HTTP triggered function executed successfully.
