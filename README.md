# BigBankWealthManagement
[![Build Status](https://dev.azure.com/giuliovdev/demos/_apis/build/status/giuliov.BigBankWealthManagement?branchName=master)](https://dev.azure.com/giuliovdev/demos/_build/latest?definitionId=6&branchName=master)

Sample code for fictitious BigBank WealthManagement API.

### build folder
Contains the Terraform code to setup the run-time environment for the sample code.
You can download Terraform from https://www.terraform.io/.
See [Building environment](./doc/building.md) for additional details.
The CI is accessible in [Azure DevOps](https://dev.azure.com/giuliovdev/demos/_build?definitionId=6&_a=summary).

### deploy folder
Contains the YAML file that defines the Azure DevOps pipeline that builds and deploys the Functions.
It is pretty straightforward and you need to change the `azureSubscription` property to your own Azure Resource Manager Service connection.
This pipeline will continuously deploy changes in the `src` directory to Azure Function.

### doc folder
Contains additional documentation files.

### src folder
Holds all application source code. It requires an Azure Subscription and a valid account.
Written using Visual Studio 2017 Enterprise (v15.9.8) but should work with Community Edition, which is downloadable from https://visualstudio.microsoft.com/vs/community/.
`bbwm` is the Azure Function project.
See [API Design](./doc/design.md) for additional details.
The [API Configuration](./doc/configuration.md) describes how to run and test.
