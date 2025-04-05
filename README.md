# csharp-func
Trying Azure Functions in various scenarios using C# language.

## Trigger Azure Functions on blob containers using an event subscription
https://learn.microsoft.com/en-us/azure/azure-functions/functions-event-grid-blob-trigger?pivots=programming-language-csharp

Send feedback to MS Learn:
https://microsoft.qualtrics.com/jfe/form/SV_6hUVpRBU3hQVnZY?original_url=https%3A%2F%2Flearn.microsoft.com%2Fen-us%2Fazure%2Fazure-functions%2Ffunctions-event-grid-blob-trigger%3Fpivots%3Dprogramming-language-csharp&locale=en-us&pageTemplate=Conceptual

### Local setup
1. Install VS Code.
2. Install `C# Dev Kit` extension. It brings `C#` extension with it. Follow the "Get Started with C# Dev Kit" steps:
   - Connect account: Sign in with your Microsoft account.
   - Set up your environment: Install .NET SDK. 
   - ...check out the other options.
   - Mark Done
3. Install [Azure Functions extension](https://marketplace.visualstudio.com/items?itemName=ms-azuretools.vscode-azurefunctions) for Visual Studio Code.
   - Or if you're using Rider, install Azure functions core tools:
     - Install
       ```bash
       brew tap azure/functions
       ```
     - Check the location where it's installed.
       ```bash
       $ which func
       /opt/homebrew/bin/func
       ```
4. Install the [Azurite v3 extension](https://marketplace.visualstudio.com/items?itemName=Azurite.azurite) for Visual Studio Code.
   - It gets installed at: `/Users/ashishkhanal/.vscode/extensions/azurite.azurite-3.34.0`
   - Or if you're using Rider, install Azurite using npm (not necessary if you're using VSCode).
      - Install
        ```bash
        npm install -g azurite
        ```
      - Check the location where it's installed. You'll use this to tell Rider where to find Azurite (Settings -> Tools -> Azure -> Azurite).
        ```bash
        $ which azurite
        /Users/ashishkhanal/.nvm/versions/node/v23.10.0/bin/azurite
        ```
      - Go to Services (Cmd+8) -> Azurite emulator -> Start/Stop Azurite
      - Reference 1: https://learn.microsoft.com/en-us/azure/storage/common/storage-use-azurite?tabs=npm%2Cblob-storage#command-line-options
      - Reference 2: https://archive.ph/51s5G
5. Install [Azure Storage extension](https://marketplace.visualstudio.com/items?itemName=ms-azuretools.vscode-azurestorage) for Visual Studio Code.
6. Install Azure Storage explorer.
   - https://azure.microsoft.com/en-us/products/storage/storage-explorer

### Create a blob triggered function
Using Visual Studio makes this process quite easy.

### Prepare local storage emulation
Make sure your `local.settings.json` has `AzureWebJobsStorage` pointing to local storage.
```json
{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet-isolated",
    "5f2f77_STORAGE": "UseDevelopmentStorage=true"
  }
}
```

#### Start Azurite Blob Storage service emulator
Press F1 to open the command palette, type `Azurite: Start Blob Service`, and press enter.

<img width="350" alt="image" src="screenshots/blob-service-listens.png">

#### Upload file to Azurite
Azure Icon in the left side bar > **Workspace** > **Attached Storage Accounts** > **Local Emulator** >, right click **Blob Containers** > **Create Blob Container**

Enter the name: `samples-workitems` > Press Enter.

Expand **Blob Containers** > **samples-workitems** and select **Upload files** > `/` > Enter > Select the file.

<img width="400" alt="image" src="screenshots/upload-file-to-azurite.png">

#### Browse using Azure Storage Explorer (not necessary though)
Use this to view files if you'd like. 

<img width="1000" alt="image" src="screenshots/az-storage-explorer.png">

### Run the function locally
1. Set a breakpoint inside `EventGridBlobTrigger.Run()` method and press F5 to start your project for local debugging.  
   Azure Functions Core tools will run in your Terminal window.

   But we have a problem!
   ```bash
   Azure Functions Core Tools
   Core Tools Version:       4.0.7030 Commit hash: N/A +bb4c949899cd5659d6bfe8b92cc923453a2e8f88 (64-bit)
   Function Runtime Version: 4.1037.0.23568
   
   [2025-04-05T03:39:20.168Z] Found /Users/ashishkhanal/RiderProjects/csharp-func/csharp-func.csproj. Using for user secrets file configuration.
   [2025-04-05T03:39:21.161Z] Worker process started and initialized.
   
   Functions:
   
           EventGridBlobTrigger: blobTrigger
   
   For detailed output, run func with --verbose flag.
   [2025-04-05T03:39:26.162Z] Host lock lease acquired by instance ID '0000000000000000000000006845BABF'.
   [2025-04-05T03:39:45.423Z] An unhandled exception has occurred. Host is shutting down.
   [2025-04-05T03:39:45.424Z] Azure.Core: Connection refused (127.0.0.1:10001). System.Net.Http: Connection refused (127.0.0.1:10001). System.Net.Sockets: Connection refused.
   [2025-04-05T03:39:51.450Z] Unable to get table reference or create table. Aborting write operation.
   [2025-04-05T03:39:51.450Z] Azure.Core: Connection refused (127.0.0.1:10002). System.Net.Http: Connection refused (127.0.0.1:10002). System.Net.Sockets: Connection refused.
    *  Terminal will be reused by tasks, press any key to close it. 
   ```
   
   **Cause of the issue:**

   The instruction in the docs to only start blob service causes this.
   
   **Solution:**

   The solution was to start all services with: `F1 -> Azurite: Start`

   <img width="450" alt="image" src="screenshots/start-all-azurite-svcs.png">

   To turn them all off: `F1 -> Azurite: Close`

2. Azure Icon in the left side bar > **Workspace** > **Local Project** > **Functions**, right click the function, and select **Execute Function Now**.
3. Put the correct file name (`test.txt`) when it asks you to enter the request body.
   
   <img width="600" alt="image" src="screenshots/enter-request-body.png">
4. Press Enter to run the function. The value you provided is the path to your blob in the local emulator. 
   This string gets passed to your trigger in the request payload, which simulates the payload when an event 
   subscription calls your function to report a blob being added to the container.
5. You'll see the breakpoint being hit and when you continue, you'll see in the output the name of the file and its contents logged.
   
   <img width="1000" alt="image" src="screenshots/breakpoint-is-hit.png">
6. Stop using `^C`.
7. More info on debugging in VS Code: https://code.visualstudio.com/docs/csharp/debugging

### Azure extension issues in VS Code
The extension gets stuck on infinite loading screen and is extremely slow in VS Code.  
So I tried logging out and logging in.

1. Ran into issues

   <img width="450" alt="image" src="screenshots/element-already-registered.png">
2. Find Azure account and log out
   <p>
     <img alt="image" src="screenshots/find-azure-account.png" width="450">
    &nbsp;
     <img alt="image" src="screenshots/logout-from-azure.png" width="200">
   </p>
3. Sign in to Azure using az cli within VS Code terminal. I'm not sure if this step helped or not.
4. Sign in to Azure using VS Code Azure extension.

   <img width="300" alt="image" src="screenshots/sign-into-azure.png">

This solved the issue!

#### Azure extension works great in Rider
It's works normally in JetBrains Rider when you login through az cli (`az login`) before you login to Rider.

<p>
  <img alt="image" src="screenshots/azure-ext-in-rider.png" width="300">
&nbsp;
  <img alt="image" src="screenshots/sign-into-rider-through-az-cli.png" width="300">
</p>

### Prepare to publish the function to Azure
https://learn.microsoft.com/en-us/azure/azure-functions/functions-event-grid-blob-trigger?pivots=programming-language-csharp#prepare-the-azure-storage-account

Just follow along the guide linked above.

#### Create Azure Storage account
Creating storage account from VS Code creates a new storage account under a new resource group it creates that has the same name as the storage account name.

That's why I opted to do this using JetBrains Rider Azure extension.

<p>
  <img alt="image" src="screenshots/rg-create.png" width="385">
&nbsp;
  <img alt="image" src="screenshots/create-storage-account.png" width="330">
</p>

Also create a new container inside **rg-func-example** > **ashk12** > **Blob Containers** > Name it: **samples-workitems**

#### Create Azure function
F1 > enter **Azure Functions: Create function app in Azure...(Advanced)**

<p>
  <img alt="image" src="screenshots/func-app-create-1.png" width="450">
&nbsp;
  <img alt="image" src="screenshots/func-app-create-2.png" width="400">
</p>

<p>
  <img alt="image" src="screenshots/func-app-create-3.png" width="450">
&nbsp;
  <img alt="image" src="screenshots/func-app-create-4.png" width="400">
</p>

<p>
  <img alt="image" src="screenshots/func-app-create-5.png" width="450">
&nbsp;
  <img alt="image" src="screenshots/func-app-create-6.png" width="420">
</p>

<p>
  <img alt="image" src="screenshots/func-app-create-7.png" width="450">
&nbsp;
  <img alt="image" src="screenshots/func-app-create-8.png" width="420">
</p>

<p>
  <img alt="image" src="screenshots/func-app-create-9.png" width="450">
&nbsp;
  <img alt="image" src="screenshots/func-app-create-10.png" width="420">
</p>

<img alt="image" src="screenshots/func-app-create-11.png" width="450">

### Deploy your function code
https://learn.microsoft.com/en-us/azure/azure-functions/functions-event-grid-blob-trigger?pivots=programming-language-csharp#deploy-your-function-code

<p>
  <img alt="image" src="screenshots/func-app-deploy-1.png" width="390">
&nbsp;
  <img alt="image" src="screenshots/func-app-deploy-2.png" width="350">
</p>

<p>
  <img alt="image" src="screenshots/func-app-deploy-3.png" width="230">
&nbsp;
  <img alt="image" src="screenshots/func-app-deploy-4.png" width="650">
</p>

<p>
  <img alt="image" src="screenshots/func-app-deploy-5.png" width="400">
&nbsp;
  <img alt="image" src="screenshots/func-app-deploy-6.png" width="420">
</p>

### Update application settings
https://learn.microsoft.com/en-us/azure/azure-functions/functions-event-grid-blob-trigger?pivots=programming-language-csharp#update-application-settings

Because required application settings from the local.settings.json file aren't automatically published, you must upload them to your function app so that your function runs correctly in Azure.

<p>
  <img alt="image" src="screenshots/download-remote-settings-1.png" width="350">
&nbsp;
  <img alt="image" src="screenshots/download-remote-settings-2.png" width="350">
</p>

<img alt="image" src="screenshots/download-remote-settings-3.png" width="200">

Now the `local.settings.json` file will look like below:
```json
{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "DefaultEndpointsProtocol=https;AccountName=ashk12;AccountKey=<key1>;EndpointSuffix=core.windows.net",
    // Remove this as it isn't supported in Flex consumption plan
    "FUNCTIONS_WORKER_RUNTIME": "dotnet-isolated",
    // Replace this setting value with the one from AzureWebJobsStorage above.
    "5f2f77_STORAGE": "UseDevelopmentStorage=true",
    "DEPLOYMENT_STORAGE_CONNECTION_STRING": "DefaultEndpointsProtocol=https;AccountName=ashk12;AccountKey=<key1>;EndpointSuffix=core.windows.net",
    "APPLICATIONINSIGHTS_CONNECTION_STRING": "InstrumentationKey=<key2>;IngestionEndpoint=https://eastus-8.in.applicationinsights.azure.com/;LiveEndpoint=https://eastus.livediagnostics.monitor.azure.com/;ApplicationId=<key3>"
  }
}
```

Now upload this using:

<p>
  <img alt="image" src="screenshots/upload-local-settings-1.png" width="380">
&nbsp;
  <img alt="image" src="screenshots/upload-local-settings-2.png" width="380">
&nbsp;
  <img alt="image" src="screenshots/upload-local-settings-3.png" width="400">
</p>

### Build endpoint url to create an event subscription
Grab blob extension access key from your function app in Azure portal:

<img alt="image" src="screenshots/func-open-in-portal.png" width="350">

<img alt="image" src="screenshots/upload-local-settings-3.png" width="800">

```
FUNCTION_APP_NAME = ashk-func-app
FUNCTION_NAME = EventGridBlobTrigger
BLOB_EXTENSION_KEY = <from above>
```

`https://<FUNCTION_APP_NAME>.azurewebsites.net/runtime/webhooks/blobs?functionName=Host.Functions.<FUNCTION_NAME>&code=<BLOB_EXTENSION_KEY>`

### Create event subscription
An event subscription, powered by Azure Event Grid, raises events based on changes in the subscribed blob container. 

This event is then sent to the blob extension endpoint for your function.
After you create an event subscription, you can't update the endpoint URL.

1. Go to your storage account you created earlier in Azure Portal.
2. "ashk12" > **Events** > **+ Event Subscription**

   <img alt="image" src="screenshots/create-event-sub-1.png" width="800">

   <img alt="image" src="screenshots/create-event-sub-2.png" width="800">
3. Hit create.

   <img alt="image" src="screenshots/sub-not-registered-error.png" width="300">

#### Fix "subscription not registered to use namespace" error
https://learn.microsoft.com/en-us/azure/azure-resource-manager/troubleshooting/error-register-resource-provider?tabs=azure-portal

<img alt="image" src="screenshots/az-portal-rsc-providers.png" width="800">

<img alt="image" src="screenshots/check-sub-reg-status-cli.png" width="800">

Either use CLI or Portal to register the resource provider.

```bash
$ az provider register --namespace Microsoft.EventGrid
Registering is still on-going. You can monitor using 'az provider show -n Microsoft.EventGrid'
```

Try step 3 from above. It succeeds this time.

<img alt="image" src="screenshots/event-sub-creation-success.png" width="350">

Tip: You can visualize resources in Portal.

<img alt="image" src="screenshots/resource-visualizer.png" width="1000">

### Upload file to the container
Upload `test.txt` and `test1.txt` files through Azure portal.

<img alt="image" src="screenshots/upload-blob-portal.png" width="1000">

See that the event was delivered:

<img alt="image" src="screenshots/blob-event-delivered.png" width="1000">

### Verify function in Azure
Open the function app and click "Invocation and more" in the function name.

<img alt="image" src="screenshots/func-name-portal.png" width="1000">

Check out the function invocation.

<img alt="image" src="screenshots/function-invocations.png" width="1000">

### Clean up resources
Delete the resource group: `rg-func-example`.

<img alt="image" src="screenshots/rg-func-example.png" width="1000">
