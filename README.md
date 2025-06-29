# Pulumi .NET Azure Automation Example

This project demonstrates a simple Infrastructure as Code (IaC) workflow using the Pulumi Automation API with Azure and .NET. It provisions an Azure Resource Group and a Storage Account configured for static website hosting, and uploads an `index.html` file.

## Features

- Uses Pulumi Automation API in C# to define and deploy Azure resources programmatically
- Provisions an Azure Resource Group and Storage Account
- Enables static website hosting and uploads a sample HTML file
- Outputs the website endpoint and storage key

## Dev Container Environment

The included dev container is preconfigured with:

- Pulumi CLI
- Azure CLI
- .NET SDK

## Getting Started

1. Open this project in VS Code (or GitHub Codespace).
1. Ensure you are authenticated with Azure (`az login`). Make sure you've selected the correct subscription if you have multiple (`az account set --subscription <your-subscription-id>`).
1. Login to Pulumi (`pulumi login`).
1. Run the program using the .NET CLI:
   
   ```sh
   cd DotNetAzureAutomation
   dotnet run
   ```
1. On completion, the static website endpoint will be displayed in the output.
