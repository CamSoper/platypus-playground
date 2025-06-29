using Pulumi;
using Pulumi.Automation;
using Pulumi.AzureNative.Resources;
using Pulumi.AzureNative.Storage;
using Pulumi.AzureNative.Storage.Inputs;

var program = PulumiFn.Create(() =>
{
    // Create an Azure Resource Group
    var resourceGroup = new ResourceGroup("resourceGroup");

    // Create an Azure Storage account
    var storageAccount = new StorageAccount("sa", new StorageAccountArgs
    {
        ResourceGroupName = resourceGroup.Name,
        Sku = new SkuArgs
        {
            Name = SkuName.Standard_LRS
        },
        Kind = Kind.StorageV2
    });

    // Enable static website support
    var staticWebsite = new StorageAccountStaticWebsite("staticWebsite", new StorageAccountStaticWebsiteArgs
    {
        AccountName = storageAccount.Name,
        ResourceGroupName = resourceGroup.Name,
        IndexDocument = "index.html",
    });

    // Upload the file
    var indexHtml = new Blob("index.html", new BlobArgs
    {
        ResourceGroupName = resourceGroup.Name,
        AccountName = storageAccount.Name,
        ContainerName = staticWebsite.ContainerName,
        Source = new FileAsset("./index.html"),
        ContentType = "text/html",
    });

    var storageAccountKeys = ListStorageAccountKeys.Invoke(new ListStorageAccountKeysInvokeArgs
    {
        ResourceGroupName = resourceGroup.Name,
        AccountName = storageAccount.Name
    });

    var primaryStorageKey = storageAccountKeys.Apply(accountKeys =>
    {
        var firstKey = accountKeys.Keys[0].Value;
        return Output.CreateSecret(firstKey);
    });

    // Get the primary storage key and static website endpoint
    return new Dictionary<string, object?>
    {
        ["primaryStorageKey"] = primaryStorageKey,
        ["staticEndpoint"] = storageAccount.PrimaryEndpoints.Apply(primaryEndpoints => primaryEndpoints.Web)
    };
});

// Arguments
var projectName = "dotnet-azure-automation";
var stackName = "dev";
var stackArgs = new InlineProgramArgs(projectName, stackName, program);

// Get a reference to the Stack
var stack = await LocalWorkspace.CreateOrSelectStackAsync(stackArgs);

// Set the location to be used when the stack is built
await stack.SetConfigAsync("azure-native:location", new ConfigValue("eastus"));

// Deploy the stack
var result = await stack.UpAsync(new UpOptions { OnStandardOutput = Console.WriteLine });
        