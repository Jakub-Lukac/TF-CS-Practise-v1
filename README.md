# Terraform-Practise-v1

## Project Overview

This project leverages Terraform as Infrastructure as Code (IaC) to deploy Azure resources, including a Windows Function App autonomously. A GitHub Actions pipeline is used to manage the CI/CD process. .NET part of this project consists of Azure Function, Dependency Injection (Interface), and XUnit test.

## Terraform service principal Setup

To run this code, you need to get an Azure Subscription ID. Run the az login command to get a list of available subscriptions.

To create a Terraform service principal, go to portal.azure.com open up the cloud power shell, and run the following command:

```text
az ad sp create-for-rbac -n AzTf --role="Contributor" --scopes="/subscriptions/{your-subcription-id}"
```

From the command output, note the `appId` value and `password` and store them in variables.tf file for **the time being.** Note them as `env_` variables together with _subscription_id_ and _tenant_id_.

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
ARM_CLIENT_SECRET is represented in variables.tf file by the env_client_secret variable.]

### Variables

**Important to note**, like client secret, app ID, tenant ID, subscription ID, **MUST** start with phrase **ARM.**

```text
ARM_CLIENT_ID
ARM_SUBSCRIPTION_ID
ARM_TENANT_ID
```

**Populate the values from the backend.conf file.**

```text
BACKEND_RESOURCE_GROUP_NAME
BACKEND_STORAGE_ACCOUNT_NAME
BACKEND_STORAGE_CONTAINER_NAME
TF_BACKEND_KEY</br>
```

# Azure Function Setup

## Creating Azure Function

In Visual Studio create Azure Function with HTTP Trigger for starters. Select the .NET 6.0 template. In this project, we will stick to .NET 6 version.

## Startup.cs file

When running a .NET 6 version of Azure Function, we have to manually create the Startup.cs file. To add logging and dependency injection populate your Startup.cs file with the following code snippet.

```text
using GitHubDemo.Services;
using GitHubDemo.Services.Interfaces;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//This startup class would be a startup of our function
[assembly: FunctionsStartup(typeof(GitHubDemo.Startup))]

namespace GitHubDemo
{
    // class for setting up the dependency injection
    // generally the cycle starts here
    // configure dependency injection container with our services and functions
    internal class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder) // == container
        {
            // place for configuring the dependency injection container

            // register my interface with concrete class (concrete == full implementation of the blueprint)

            builder.Services.AddLogging();

            builder.Services.AddSingleton<IBulkRequestProcessor, BulkRequestProcessor>();
        }
    }
}

```

## XUnit Test

Create a new project, and select XUnit test. To create a very simple test use the following code snippet.

```text
namespace GitHubDemo.UnitTests
{
    public class UnitTest1
    {
        // this trait allows us to execute this unit tests through our ci pipeline
        // run: dotnet test --no-restore --verbosity normal --filter Category=Unit
        [Trait("Category", "Unit")]
        [Fact]
        public void Test1()
        {
            int a = 2;
            int b = 3;
            int sum = a + b;
            Assert.True(sum == 5);
        }
    }
}
```

## PostMan

We will use PostMan to test our Azure function.

By targeting the following GET endpoint we get a message: "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."

```text
https://{NAME_OF_THE_FUNCTION_APP_FROM_AZURE_PORTAL}.azurewebsites.net/api/foo
```

The "foo" endpoint is specified in our HTTP trigger Azure function.

By targeting the following GET endpoint, we get a message: "Hello, Jakub. This HTTP triggered function executed successfully."

```text
 https://{NAME_OF_THE_FUNCTION_APP_FROM_AZURE_PORTAL}.azurewebsites.net/api/foo?name=Jakub
```

## NuGet Packages

```text
Microsoft.NET.Sdk.Functions
Microsoft.Extensions.DependencyInjection
Microsoft.Logging.Abstractions
```
