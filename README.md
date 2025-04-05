# csharp-func
Trying Azure Functions in various scenarios using C# language.

https://learn.microsoft.com/en-us/azure/azure-functions/functions-event-grid-blob-trigger?pivots=programming-language-csharp

## Local setup
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
   - Or if you're using Rider, install Azurite using npm (not necessary if you're using VSCode).
      - Install
        ```bash
        npm install -g azurite
        ```
      - Check the location where it's installed. You'll use this to tell Rider where to find Azurite.
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

## Trigger Azure Functions on blob containers using an event subscription

### Issues
```bash
Functions:

        EventGridBlobTrigger: blobTrigger

For detailed output, run func with --verbose flag.
[2025-04-04T19:35:27.839Z] Host lock lease acquired by instance ID '0000000000000000000000006845BABF'.
[2025-04-04T19:35:48.015Z] An unhandled exception has occurred. Host is shutting down.
[2025-04-04T19:35:48.015Z] Azure.Core: Connection refused (127.0.0.1:10001). System.Net.Http: Connection refused (127.0.0.1:10001). System.Net.Sockets: Connection refused.
[2025-04-04T19:35:53.466Z] Unable to get table reference or create table. Aborting write operation.
[2025-04-04T19:35:53.467Z] Azure.Core: Connection refused (127.0.0.1:10002). System.Net.Http: Connection refused (127.0.0.1:10002). System.Net.Sockets: Connection refused.
 *  Terminal will be reused by tasks, press any key to close it. 
```
