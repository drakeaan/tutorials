using Azure;
using Azure.AI.OpenAI.Assistants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CSharp_AI
{
    internal class OpenAIAssistant
    {

        public async Task Run()
        {
            // Authenticate the client
            AssistantsClient client = isAzureOpenAI
            ? new AssistantsClient(new Uri(azureResourceUrl), new AzureKeyCredential(azureApiKey))
            : new AssistantsClient(nonAzureApiKey);

            Response<Assistant> assistantResponse = await client.CreateAssistantAsync(
    new AssistantCreationOptions("gpt-4-1106-preview")
    {
        Name = "Math Tutor",
        Instructions = "You are a personal math tutor. Write and run code to answer math questions.",
        Tools = { new CodeInterpreterToolDefinition() }
    });
            Assistant assistant = assistantResponse.Value;


            // Create the thread
            Response<AssistantThread> threadResponse = await client.CreateThreadAsync();
            AssistantThread thread = threadResponse.Value;

            // Create a message on the thread
            Response<ThreadMessage> messageResponse = await client.CreateMessageAsync(
                thread.Id,
                MessageRole.User,
                "I need to solve the equation `3x + 11 = 14`. Can you help me?");
            ThreadMessage message = messageResponse.Value;


            // Start the run against assistant
            Response<ThreadRun> runResponse = await client.CreateRunAsync(
                thread.Id,
                new CreateRunOptions(assistant.Id)
                {
                    AdditionalInstructions = "Please address the user as Jane Doe. The user has a premium account.",
                });
            ThreadRun run = runResponse.Value;


            // loop (while) until terminal status
            do
            {
                await Task.Delay(TimeSpan.FromMilliseconds(500));
                runResponse = await client.GetRunAsync(thread.Id, runResponse.Value.Id);
            }
            while (runResponse.Value.Status == RunStatus.Queued
                || runResponse.Value.Status == RunStatus.InProgress);



            // List messages from run response
            Response<PageableList<ThreadMessage>> afterRunMessagesResponse
                = await client.GetMessagesAsync(thread.Id);
            IReadOnlyList<ThreadMessage> messages = afterRunMessagesResponse.Value.Data;

            // Note: messages iterate from newest to oldest, with the messages[0] being the most recent
            foreach (ThreadMessage threadMessage in messages)
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

            // Using files
            File.WriteAllText(
                path: "sample_file_for_upload.txt",
                contents: "The word 'apple' uses the code 442345, while the word 'banana' uses the code 673457.");
            Response<OpenAIFile> uploadAssistantFileResponse = await client.UploadFileAsync(
                localFilePath: "sample_file_for_upload.txt",
                purpose: OpenAIFilePurpose.Assistants);
            OpenAIFile uploadedAssistantFile = uploadAssistantFileResponse.Value;

            Response<Assistant> assistantResponse = await client.CreateAssistantAsync(
                new AssistantCreationOptions("gpt-4-1106-preview")
                {
                    Name = "SDK Test Assistant - Retrieval",
                    Instructions = "You are a helpful assistant that can help fetch data from files you know about.",
                    Tools = { new RetrievalToolDefinition() },
                    FileIds = { uploadedAssistantFile.Id },
                });
            Assistant assistant = assistantResponse.Value;


            // Example of a function that defines no parameters
            string GetUserFavoriteCity() => "Seattle, WA";
            FunctionToolDefinition getUserFavoriteCityTool = new("getUserFavoriteCity", "Gets the user's favorite city.");
            // Example of a function with a single required parameter
            string GetCityNickname(string location) => location switch
            {
                "Seattle, WA" => "The Emerald City",
                _ => throw new NotImplementedException(),
            };
            FunctionToolDefinition getCityNicknameTool = new(
                name: "getCityNickname",
                description: "Gets the nickname of a city, e.g. 'LA' for 'Los Angeles, CA'.",
                parameters: BinaryData.FromObjectAsJson(
                    new
                    {
                        Type = "object",
                        Properties = new
                        {
                            Location = new
                            {
                                Type = "string",
                                Description = "The city and state, e.g. San Francisco, CA",
                            },
                        },
                        Required = new[] { "location" },
                    },
                    new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }));
            // Example of a function with one required and one optional, enum parameter
            string GetWeatherAtLocation(string location, string temperatureUnit = "f") => location switch
            {
                "Seattle, WA" => temperatureUnit == "f" ? "70f" : "21c",
                _ => throw new NotImplementedException()
            };
            FunctionToolDefinition getCurrentWeatherAtLocationTool = new(
                name: "getCurrentWeatherAtLocation",
                description: "Gets the current weather at a provided location.",
                parameters: BinaryData.FromObjectAsJson(
                    new
                    {
                        Type = "object",
                        Properties = new
                        {
                            Location = new
                            {
                                Type = "string",
                                Description = "The city and state, e.g. San Francisco, CA",
                            },
                            Unit = new
                            {
                                Type = "string",
                                Enum = new[] { "c", "f" },
                            },
                        },
                        Required = new[] { "location" },
                    },
                    new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }));


            // With the functions defined in their appropriate tools, an assistant can be now created that has those tools enabled:
            Response<Assistant> assistantResponse = await client.CreateAssistantAsync(
                // note: parallel function calling is only supported with newer models like gpt-4-1106-preview
                new AssistantCreationOptions("gpt-4-1106-preview")
                {
                    Name = "SDK Test Assistant - Functions",
                    Instructions = "You are a weather bot. Use the provided functions to help answer questions. "
                        + "Customize your responses to the user's preferences as much as possible and use friendly "
                        + "nicknames for cities whenever possible.",
                    Tools =
                    {
                        getUserFavoriteCityTool,
                        getCityNicknameTool,
                        getCurrentWeatherAtLocationTool,
                    },
                });
            Assistant assistant = assistantResponse.Value;


            // If the assistant calls tools, the calling code will need to resolve ToolCall instances into matching ToolOutput instances. For convenience, a basic example is extracted here:
            ToolOutput GetResolvedToolOutput(RequiredToolCall toolCall)
            {
                if (toolCall is RequiredFunctionToolCall functionToolCall)
                {
                    if (functionToolCall.Name == getUserFavoriteCityTool.Name)
                    {
                        return new ToolOutput(toolCall, GetUserFavoriteCity());
                    }
                    using JsonDocument argumentsJson = JsonDocument.Parse(functionToolCall.Arguments);
                    if (functionToolCall.Name == getCityNicknameTool.Name)
                    {
                        string locationArgument = argumentsJson.RootElement.GetProperty("location").GetString();
                        return new ToolOutput(toolCall, GetCityNickname(locationArgument));
                    }
                    if (functionToolCall.Name == getCurrentWeatherAtLocationTool.Name)
                    {
                        string locationArgument = argumentsJson.RootElement.GetProperty("location").GetString();
                        if (argumentsJson.RootElement.TryGetProperty("unit", out JsonElement unitElement))
                        {
                            string unitArgument = unitElement.GetString();
                            return new ToolOutput(toolCall, GetWeatherAtLocation(locationArgument, unitArgument));
                        }
                        return new ToolOutput(toolCall, GetWeatherAtLocation(locationArgument));
                    }
                }
                return null;
            }




            do
            {
                await Task.Delay(TimeSpan.FromMilliseconds(500));
                runResponse = await client.GetRunAsync(thread.Id, runResponse.Value.Id);

                if (runResponse.Value.Status == RunStatus.RequiresAction
                    && runResponse.Value.RequiredAction is SubmitToolOutputsAction submitToolOutputsAction)
                {
                    List<ToolOutput> toolOutputs = new();
                    foreach (RequiredToolCall toolCall in submitToolOutputsAction.ToolCalls)
                    {
                        toolOutputs.Add(GetResolvedToolOutput(toolCall));
                    }
                    runResponse = await client.SubmitToolOutputsToRunAsync(runResponse.Value, toolOutputs);
                }
            }
            while (runResponse.Value.Status == RunStatus.Queued
                || runResponse.Value.Status == RunStatus.InProgress);



        }
    }
}
