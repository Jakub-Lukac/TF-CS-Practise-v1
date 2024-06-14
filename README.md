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

# PostMan

by targeting this endpoint https://{NAME_OF_THE_FUNCTION_APP_FROM_AZURE_PORTAL}/api/foo
we get a message :
This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response.

by targeting this endpoint https://{NAME_OF_THE_FUNCTION_APP_FROM_AZURE_PORTAL}/api/foo?name=Jakub
we get a message :
Hello, Jakub. This HTTP triggered function executed successfully.
