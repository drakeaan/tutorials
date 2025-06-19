using Azure;
using Azure.AI.Inference;
using Azure.AI.Projects;
using Azure.Core;
using Azure.Identity;
using OpenAI.Assistants;
using OpenAI.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharp_AI;

public class Projects
{
    public async Task Run()
    {
        var endpoint = Environment.GetEnvironmentVariable("PROJECT_ENDPOINT");
        AIProjectClient projectClient = new AIProjectClient(new Uri(endpoint), new DefaultAzureCredential());

        // Performing Agent operations
        var endpoint = System.Environment.GetEnvironmentVariable("PROJECT_ENDPOINT");
        var modelDeploymentName = System.Environment.GetEnvironmentVariable("MODEL_DEPLOYMENT_NAME");
        AIProjectClient projectClient = new(new Uri(endpoint), new DefaultAzureCredential());
        PersistentAgentsClient agentsClient = projectClient.GetPersistentAgentsClient();

        // Step 1: Create an agent
        PersistentAgent agent = agentsClient.Administration.CreateAgent(
            model: modelDeploymentName,
            name: "Math Tutor",
            instructions: "You are a personal math tutor. Write and run code to answer math questions."
        );

        //// Step 2: Create a thread
        PersistentAgentThread thread = agentsClient.Threads.CreateThread();

        // Step 3: Add a message to a thread
        PersistentThreadMessage message = agentsClient.Messages.CreateMessage(
            thread.Id,
            MessageRole.User,
            "I need to solve the equation `3x + 11 = 14`. Can you help me?");

        // Intermission: message is now correlated with thread
        // Intermission: listing messages will retrieve the message just added

        List<PersistentThreadMessage> messagesList = [.. agentsClient.Messages.GetMessages(thread.Id)];
        Assert.AreEqual(message.Id, messagesList[0].Id);

        // Step 4: Run the agent
        ThreadRun run = agentsClient.Runs.CreateRun(
            thread.Id,
            agent.Id,
            additionalInstructions: "Please address the user as Jane Doe. The user has a premium account.");
        do
        {
            Thread.Sleep(TimeSpan.FromMilliseconds(500));
            run = agentsClient.Runs.GetRun(thread.Id, run.Id);
        }
        while (run.Status == RunStatus.Queued
            || run.Status == RunStatus.InProgress);
        Assert.AreEqual(
            RunStatus.Completed,
            run.Status,
            run.LastError?.Message);

        Pageable<PersistentThreadMessage> messages
            = agentsClient.Messages.GetMessages(
                threadId: thread.Id, order: ListSortOrder.Ascending);

        foreach (PersistentThreadMessage threadMessage in messages)
        {
            Console.Write($"{threadMessage.CreatedAt:yyyy-MM-dd HH:mm:ss} - {threadMessage.Role,10}: ");
            foreach (MessageContent contentItem in threadMessage.ContentItems)
            {
                if (contentItem is MessageTextContent textItem)
                {
                    Console.Write(textItem.Text);
                }
                else if (contentItem is MessageImageFileContent imageFileItem)
                {
                    Console.Write($"<image from ID: {imageFileItem.FileId}");
                }
                Console.WriteLine();
            }
        }

        agentsClient.Threads.DeleteThread(threadId: thread.Id);
        agentsClient.Administration.DeleteAgent(agentId: agent.Id);



        // Get an authenticated AzureOpenAI client
        var endpoint = System.Environment.GetEnvironmentVariable("PROJECT_ENDPOINT");
        var modelDeploymentName = System.Environment.GetEnvironmentVariable("MODEL_DEPLOYMENT_NAME");
        AIProjectClient projectClient = new AIProjectClient(new Uri(endpoint), new DefaultAzureCredential());
        ChatClient chatClient = projectClient.GetAzureOpenAIChatClient(deploymentName: modelDeploymentName, connectionName: null, apiVersion: null);

        ChatCompletion result = chatClient.CompleteChat("List all the rainbow colors");
        Console.WriteLine(result.Content[0].Text);




        // Get an authenticated ChatCompletionsClient
        var endpoint = System.Environment.GetEnvironmentVariable("PROJECT_ENDPOINT");
        var modelDeploymentName = System.Environment.GetEnvironmentVariable("MODEL_DEPLOYMENT_NAME");
        AIProjectClient client = new AIProjectClient(new Uri(endpoint), new DefaultAzureCredential());
        ChatCompletionsClient chatClient = client.GetChatCompletionsClient();

        var requestOptions = new ChatCompletionsOptions()
        {
            Messages =
                {
                    new ChatRequestSystemMessage("You are a helpful assistant."),
                    new ChatRequestUserMessage("How many feet are in a mile?"),
                },
            Model = modelDeploymentName
        };
        Response<ChatCompletions> response = chatClient.Complete(requestOptions);
        Console.WriteLine(response.Value.Content);



        // Deployments operations
        var endpoint = System.Environment.GetEnvironmentVariable("PROJECT_ENDPOINT");
        var modelDeploymentName = System.Environment.GetEnvironmentVariable("MODEL_DEPLOYMENT_NAME");
        var modelPublisher = System.Environment.GetEnvironmentVariable("MODEL_PUBLISHER");
        AIProjectClient projectClient = new(new Uri(endpoint), new DefaultAzureCredential());
        Deployments deployments = projectClient.GetDeploymentsClient();

        Console.WriteLine("List all deployments:");
        foreach (var deployment in deployments.GetDeployments())
        {
            Console.WriteLine(deployment);
        }

        Console.WriteLine($"List all deployments by the model publisher `{modelPublisher}`:");
        foreach (var deployment in deployments.GetDeployments(modelPublisher: modelPublisher))
        {
            Console.WriteLine(deployment);
        }

        Console.WriteLine($"Get a single deployment named `{modelDeploymentName}`:");
        var deploymentDetails = deployments.GetDeployment(modelDeploymentName);
        Console.WriteLine(deploymentDetails);




        // Connections operations
        var endpoint = Environment.GetEnvironmentVariable("PROJECT_ENDPOINT");
        var connectionName = Environment.GetEnvironmentVariable("CONNECTION_NAME");
        AIProjectClient projectClient = new(new Uri(endpoint), new DefaultAzureCredential());
        Connections connectionsClient = projectClient.GetConnectionsClient();

        Console.WriteLine("List the properties of all connections:");
        foreach (var connection in connectionsClient.GetConnections())
        {
            Console.WriteLine(connection);
            Console.Write(connection.Name);
        }

        Console.WriteLine("List the properties of all connections of a particular type (e.g., Azure OpenAI connections):");
        foreach (var connection in connectionsClient.GetConnections(connectionType: ConnectionType.AzureOpenAI))
        {
            Console.WriteLine(connection);
        }

        Console.WriteLine($"Get the properties of a connection named `{connectionName}`:");
        var specificConnection = connectionsClient.Get(connectionName, includeCredentials: false);
        Console.WriteLine(specificConnection);

        Console.WriteLine("Get the properties of a connection with credentials:");
        var specificConnectionCredentials = connectionsClient.Get(connectionName, includeCredentials: true);
        Console.WriteLine(specificConnectionCredentials);

        Console.WriteLine($"Get the properties of the default connection:");
        var defaultConnection = connectionsClient.GetDefault(includeCredentials: false);
        Console.WriteLine(defaultConnection);

        Console.WriteLine($"Get the properties of the default connection with credentials:");
        var defaultConnectionCredentials = connectionsClient.GetDefault(includeCredentials: true);
        Console.WriteLine(defaultConnectionCredentials);





        // Dataet Operations
        var endpoint = System.Environment.GetEnvironmentVariable("PROJECT_ENDPOINT");
        var datasetName = System.Environment.GetEnvironmentVariable("DATASET_NAME");
        AIProjectClient projectClient = new(new Uri(endpoint), new DefaultAzureCredential());
        Datasets datasets = projectClient.GetDatasetsClient();

        Console.WriteLine("Uploading a single file to create Dataset version '1'...");
        var datasetResponse = datasets.UploadFile(
            name: datasetName,
            version: "1",
            filePath: "sample_folder/sample_file1.txt"
            );
        Console.WriteLine(datasetResponse);

        Console.WriteLine("Uploading folder to create Dataset version '2'...");
        datasetResponse = datasets.UploadFolder(
            name: datasetName,
            version: "2",
            folderPath: "sample_folder"
        );
        Console.WriteLine(datasetResponse);

        Console.WriteLine("Retrieving Dataset version '1'...");
        DatasetVersion dataset = datasets.GetDataset(datasetName, "1");
        Console.WriteLine(dataset);

        Console.WriteLine($"Listing all versions for Dataset '{datasetName}':");
        foreach (var ds in datasets.GetVersions(datasetName))
        {
            Console.WriteLine(ds);
        }

        Console.WriteLine($"Listing latest versions for all datasets:");
        foreach (var ds in datasets.GetDatasetVersions())
        {
            Console.WriteLine(ds);
        }

        Console.WriteLine("Deleting Dataset versions '1' and '2'...");
        datasets.Delete(datasetName, "1");
        datasets.Delete(datasetName, "2");




        // Indexes Operation
        var endpoint = Environment.GetEnvironmentVariable("PROJECT_ENDPOINT");
        var indexName = Environment.GetEnvironmentVariable("INDEX_NAME") ?? "my-index";
        var indexVersion = Environment.GetEnvironmentVariable("INDEX_VERSION") ?? "1.0";
        var aiSearchConnectionName = Environment.GetEnvironmentVariable("AI_SEARCH_CONNECTION_NAME") ?? "my-ai-search-connection-name";
        var aiSearchIndexName = Environment.GetEnvironmentVariable("AI_SEARCH_INDEX_NAME") ?? "my-ai-search-index-name";
        AIProjectClient projectClient = new(new Uri(endpoint), new DefaultAzureCredential());
        Indexes indexesClient = projectClient.GetIndexesClient();

        RequestContent content = RequestContent.Create(new
        {
            connectionName = aiSearchConnectionName,
            indexName = aiSearchIndexName,
            indexVersion = indexVersion,
            type = "AzureSearch",
            description = "Sample Index for testing",
            displayName = "Sample Index"
        });

        Console.WriteLine($"Create an Index named `{indexName}` referencing an existing AI Search resource:");
        var index = indexesClient.CreateOrUpdate(
            name: indexName,
            version: indexVersion,
            content: content
        );
        Console.WriteLine(index);

        Console.WriteLine($"Get an existing Index named `{indexName}`, version `{indexVersion}`:");
        var retrievedIndex = indexesClient.GetIndex(name: indexName, version: indexVersion);
        Console.WriteLine(retrievedIndex);

        Console.WriteLine($"Listing all versions of the Index named `{indexName}`:");
        foreach (var version in indexesClient.GetVersions(name: indexName))
        {
            Console.WriteLine(version);
        }

        Console.WriteLine($"Listing all Indices:");
        foreach (var version in indexesClient.GetIndices())
        {
            Console.WriteLine(version);
        }

        Console.WriteLine("Delete the Index version created above:");
        indexesClient.Delete(name: indexName, version: indexVersion);



        // Troubleshooting
        try
        {
            projectClient.GetDatasetsClient().GetDataset("non-existent-dataset-name", "non-existent-dataset-version");
        }
        catch (RequestFailedException ex) when (ex.Status == 404)
        {
            Console.WriteLine($"Exception status code: {ex.Status}");
            Console.WriteLine($"Exception message: {ex.Message}");
        }
    }
}