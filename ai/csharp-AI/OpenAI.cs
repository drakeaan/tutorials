using Azure.AI.OpenAI;
using Azure.AI.OpenAI.Chat;
using Azure.Identity;
using OpenAI.Assistants;
using OpenAI.Chat;
using System;
using System.ClientModel;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CSharpAI;

internal class OpenAI
{
    public async Task Run()
    {
        // Connect with DefaultAzureCredential
        AzureOpenAIClient azureClient = new(
            new Uri("https://your-azure-openai-resource.com"),
            new DefaultAzureCredential());
        ChatClient chatClient = azureClient.GetChatClient("my-gpt-4o-mini-deployment");



        // Configure client for Azure sovereign cloud
        AzureOpenAIClientOptions options = new()
        {
            Audience = AzureOpenAIAudience.AzureGovernment,
        };
        AzureOpenAIClient azureClient = new(
            new Uri("https://your-azure-openai-resource.com"),
            new DefaultAzureCredential(),
            options);
        ChatClient chatClient = azureClient.GetChatClient("my-gpt-4o-mini-deployment");


        // For a custom or non-enumerated value, the authorization scope can be provided directly as the value for Audience:
        AzureOpenAIClientOptions optionsWithCustomAudience = new()
        {
            Audience = "https://cognitiveservices.azure.com/.default",
        };



        // Create client with an API key
        string keyFromEnvironment = Environment.GetEnvironmentVariable("AZURE_OPENAI_API_KEY");

        AzureOpenAIClient azureClient = new(
            new Uri("https://your-azure-openai-resource.com"),
            new ApiKeyCredential(keyFromEnvironment));
        ChatClient chatClient = azureClient.GetChatClient("my-gpt-35-turbo-deployment");



        // Get a chat completion
        AzureOpenAIClient azureClient = new(
            new Uri("https://your-azure-openai-resource.com"),
            new DefaultAzureCredential());
        ChatClient chatClient = azureClient.GetChatClient("my-gpt-35-turbo-deployment");

        ChatCompletion completion = chatClient.CompleteChat(
            [
                // System messages represent instructions or other guidance about how the assistant should behave
                new SystemChatMessage("You are a helpful assistant that talks like a pirate."),
                // User messages represent user input, whether historical or the most recent input
                new UserChatMessage("Hi, can you help me?"),
                // Assistant messages in a request represent conversation history for responses
                new AssistantChatMessage("Arrr! Of course, me hearty! What can I do for ye?"),
                new UserChatMessage("What's the best way to train a parrot?"),
            ]);

        Console.WriteLine($"{completion.Role}: {completion.Content[0].Text}");


        // Stream chat messages
        AzureOpenAIClient azureClient = new(
            new Uri("https://your-azure-openai-resource.com"),
            new DefaultAzureCredential());
        ChatClient chatClient = azureClient.GetChatClient("my-gpt-35-turbo-deployment");

        CollectionResult<StreamingChatCompletionUpdate> completionUpdates = chatClient.CompleteChatStreaming(
            [
                new SystemChatMessage("You are a helpful assistant that talks like a pirate."),
                new UserChatMessage("Hi, can you help me?"),
                new AssistantChatMessage("Arrr! Of course, me hearty! What can I do for ye?"),
                new UserChatMessage("What's the best way to train a parrot?"),
            ]);

        foreach (StreamingChatCompletionUpdate completionUpdate in completionUpdates)
        {
            foreach (ChatMessageContentPart contentPart in completionUpdate.ContentUpdate)
            {
                Console.Write(contentPart.Text);
            }
        }


        // Use chat tools
        static string GetCurrentLocation()
        {
            // Call the location API here.
            return "San Francisco";
        }

        static string GetCurrentWeather(string location, string unit = "celsius")
        {
            // Call the weather API here.
            return $"31 {unit}";
        }

        ChatTool getCurrentLocationTool = ChatTool.CreateFunctionTool(
            functionName: nameof(GetCurrentLocation),
            functionDescription: "Get the user's current location"
        );

        ChatTool getCurrentWeatherTool = ChatTool.CreateFunctionTool(
            functionName: nameof(GetCurrentWeather),
            functionDescription: "Get the current weather in a given location",
            functionParameters: BinaryData.FromString("""
            {
                "type": "object",
                "properties": {
                    "location": {
                        "type": "string",
                        "description": "The city and state, e.g. Boston, MA"
                    },
                    "unit": {
                        "type": "string",
                        "enum": [ "celsius", "fahrenheit" ],
                        "description": "The temperature unit to use. Infer this from the specified location."
                    }
                },
                "required": [ "location" ]
            }
            """)
        );

        ChatCompletionOptions options = new()
        {
            Tools = { getCurrentLocationTool, getCurrentWeatherTool },
        };

        List<ChatMessage> conversationMessages =
            [
                new UserChatMessage("What's the weather like in Boston?"),
            ];
        ChatCompletion completion = chatClient.CompleteChat(conversationMessages);




        // Purely for convenience and clarity, this standalone local method handles tool call responses.
        string GetToolCallContent(ChatToolCall toolCall)
        {
            if (toolCall.FunctionName == getCurrentWeatherTool.FunctionName)
            {
                // Validate arguments before using them; it's not always guaranteed to be valid JSON!
                try
                {
                    using JsonDocument argumentsDocument = JsonDocument.Parse(toolCall.FunctionArguments);
                    if (!argumentsDocument.RootElement.TryGetProperty("location", out JsonElement locationElement))
                    {
                        // Handle missing required "location" argument
                    }
                    else
                    {
                        string location = locationElement.GetString();
                        if (argumentsDocument.RootElement.TryGetProperty("unit", out JsonElement unitElement))
                        {
                            return GetCurrentWeather(location, unitElement.GetString());
                        }
                        else
                        {
                            return GetCurrentWeather(location);
                        }
                    }
                }
                catch (JsonException)
                {
                    // Handle the JsonException (bad arguments) here
                }
            }
            // Handle unexpected tool calls
            throw new NotImplementedException();
        }

        if (completion.FinishReason == ChatFinishReason.ToolCalls)
        {
            // Add a new assistant message to the conversation history that includes the tool calls
            conversationMessages.Add(new AssistantChatMessage(completion));

            foreach (ChatToolCall toolCall in completion.ToolCalls)
            {
                conversationMessages.Add(new ToolChatMessage(toolCall.Id, GetToolCallContent(toolCall)));
            }

            // Now make a new request with all the messages thus far, including the original
        }



        StringBuilder contentBuilder = new();
        StreamingChatToolCallsBuilder toolCallsBuilder = new();

        foreach (StreamingChatCompletionUpdate streamingChatUpdate
            in chatClient.CompleteChatStreaming(conversationMessages, options))
        {
            foreach (ChatMessageContentPart contentPart in streamingChatUpdate.ContentUpdate)
            {
                contentBuilder.Append(contentPart.Text);
            }

            foreach (StreamingChatToolCallUpdate toolCallUpdate in streamingChatUpdate.ToolCallUpdates)
            {
                toolCallsBuilder.Append(toolCallUpdate);
            }
        }

        IReadOnlyList<ChatToolCall> toolCalls = toolCallsBuilder.Build();

        AssistantChatMessage assistantMessage = new AssistantChatMessage(toolCalls);
        if (contentBuilder.Length > 0)
        {
            assistantMessage.Content.Add(ChatMessageContentPart.CreateTextPart(contentBuilder.ToString()));
        }

        conversationMessages.Add(assistantMessage);

        // Placeholder: each tool call must be resolved, like in the non-streaming case
        string GetToolCallOutput(ChatToolCall toolCall) => null;

        foreach (ChatToolCall toolCall in toolCalls)
        {
            conversationMessages.Add(new ToolChatMessage(toolCall.Id, GetToolCallOutput(toolCall)));
        }







        // Use your own data with Azure OpenAI
        // Extension methods to use data sources with options are subject to SDK surface changes. Suppress the
        // warning to acknowledge and this and use the subject-to-change AddDataSource method.
#pragma warning disable AOAI001

        ChatCompletionOptions options = new();
        options.AddDataSource(new AzureSearchChatDataSource()
        {
            Endpoint = new Uri("https://your-search-resource.search.windows.net"),
            IndexName = "contoso-products-index",
            Authentication = DataSourceAuthentication.FromApiKey(
                Environment.GetEnvironmentVariable("OYD_SEARCH_KEY")),
        });

        ChatCompletion completion = chatClient.CompleteChat(
            [
                new UserChatMessage("What are the best-selling Contoso products this month?"),
    ],
            options);

        ChatMessageContext onYourDataContext = completion.GetMessageContext();

        if (onYourDataContext?.Intent is not null)
        {
            Console.WriteLine($"Intent: {onYourDataContext.Intent}");
        }
        foreach (ChatCitation citation in onYourDataContext?.Citations ?? [])
        {
            Console.WriteLine($"Citation: {citation.Content}");
        }




        // Use Assistants and stream a run
        AzureOpenAIClient azureClient = new(
    new Uri("https://your-azure-openai-resource.com"),
    new DefaultAzureCredential());

        // The Assistants feature area is in beta, with API specifics subject to change.
        // Suppress the [Experimental] warning via .csproj or, as here, in the code to acknowledge.
#pragma warning disable OPENAI001
        AssistantClient assistantClient = azureClient.GetAssistantClient();

        Assistant assistant = await assistantClient.CreateAssistantAsync(
    model: "my-gpt-4o-deployment",
    new AssistantCreationOptions()
    {
        Name = "My Friendly Test Assistant",
        Instructions = "You politely help with math questions. Use the code interpreter tool when asked to "
            + "visualize numbers.",
        Tools = { ToolDefinition.CreateCodeInterpreter() },
    });
        ThreadInitializationMessage initialMessage = new(
            MessageRole.User,
            [
                "Hi, Assistant! Draw a graph for a line with a slope of 4 and y-intercept of 9."
            ]);
        AssistantThread thread = await assistantClient.CreateThreadAsync(new ThreadCreationOptions()
        {
            InitialMessages = { initialMessage },
        });

        RunCreationOptions runOptions = new()
        {
            AdditionalInstructions = "When possible, talk like a pirate."
        };
        await foreach (StreamingUpdate streamingUpdate
            in assistantClient.CreateRunStreamingAsync(thread.Id, assistant.Id, runOptions))
        {
            if (streamingUpdate.UpdateKind == StreamingUpdateReason.RunCreated)
            {
                Console.WriteLine($"--- Run started! ---");
            }
            else if (streamingUpdate is MessageContentUpdate contentUpdate)
            {
                Console.Write(contentUpdate.Text);
                if (contentUpdate.ImageFileId is not null)
                {
                    Console.WriteLine($"[Image content file ID: {contentUpdate.ImageFileId}");
                }
            }
        }

        // Optionally, delete persistent resources that are no longer needed.
        _ = await assistantClient.DeleteAssistantAsync(assistant.Id);
        _ = await assistantClient.DeleteThreadAsync(thread.Id);
    }
}
