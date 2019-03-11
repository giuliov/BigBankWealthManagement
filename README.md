# BigBankWealthManagement
Sample code

### build folder
Contains the Terraform code to setup the run-time environment for the sample code.
You can download Terraform from https://www.terraform.io/.
It uses Azure back-end which you have to setup in advance.
See at https://www.terraform.io/docs/providers/azurerm/index.html#authenticating-to-azure for credential and authentication.

### deploy folder
Contains the YAML file that defines the Azure DevOps pipeline that builds and deploys the Functions.
It is pretty straightforward and you need to change the `azureSubscription` property to your own Azure Resource Manager Service connection.

### doc folder
Contains additional documentation files.

### src folder
Holds all application source code. It requires an Azure Subscription and a valid account.
Written using Visual Studio 2017 Enterprise (v15.9.8) but should work with Community Edition, which is downloadable from https://visualstudio.microsoft.com/vs/community/.
`bbwm` is the Azure Function project.