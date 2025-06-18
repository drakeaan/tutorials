using Azure;
using Azure.AI.Agents.Persistent;
using Azure.Identity;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

//Get configuration from appsettings.json.
IConfigurationRoot configuration = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

var projectEndpoint = configuration["ProjectEndpoint"];
var modelDeploymentName = configuration["ModelDeploymentName"];
var storageQueueUri = configuration["StorageQueueURI"];
//Initialize PersistentAgentsClient.
PersistentAgentsClient client = new(projectEndpoint, new DefaultAzureCredential());

//Define Azure Function tool definition.
AzureFunctionToolDefinition azureFnTool = new(
    name: "foo",
    description: "Get answers from the foo bot.",
    inputBinding: new AzureFunctionBinding(
        new AzureFunctionStorageQueue(
            queueName: "azure-function-foo-input",
            storageServiceEndpoint: storageQueueUri
        )
    ),
    outputBinding: new AzureFunctionBinding(
        new AzureFunctionStorageQueue(
            queueName: "azure-function-tool-output",
            storageServiceEndpoint: storageQueueUri
        )
    ),
    parameters: BinaryData.FromObjectAsJson(
            new
            {
                Type = "object",
                Properties = new
                {
                    query = new
                    {
                        Type = "string",
                        Description = "The question to ask.",
                    },
                    outputqueueuri = new
                    {
                        Type = "string",
                        Description = "The full output queue uri."
                    }
                },
            },
        new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }
    )
);

//Create agent and give it the Azure Function tool.
PersistentAgent agent = client.Administration.CreateAgent(
    model: modelDeploymentName,
    name: "azure-function-agent-foo",
    instructions: "You are a helpful support agent. Use the provided function any "
    + "time the prompt contains the string 'What would foo say?'. When you invoke "
    + "the function, ALWAYS specify the output queue uri parameter as "
    + $"'{storageQueueUri}/azure-function-tool-output'. Always responds with "
    + "\"Foo says\" and then the response from the tool.",
    tools: [azureFnTool]
);