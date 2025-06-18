import os
import asyncio
from azure.identity import DefaultAzureCredential
from azure.core.credentials import AzureKeyCredential
from azure.ai.agents import AgentsClient
from azure.ai.agents.aio import AgentsClient
from opentelemetry import trace
from azure.monitor.opentelemetry import configure_azure_monitor
from azure.ai.agents.tools.azure_function import AzureFunctionTool, AzureFunctionStorageQueue

from azure.ai.agents.telemetry import enable_telemetry

enable_telemetry(destination=sys.stdout)


agents_client = AgentsClient(
    endpoint=os.environ["PROJECT_ENDPOINT"],
    credential=DefaultAzureCredential(),
)



import os

agent_client = AgentsClient(
   endpoint=os.environ["PROJECT_ENDPOINT"],
   credential=DefaultAzureCredential(),
)


# Create Agent
def run():
    agent = agents_client.create_agent(
        model=os.environ["MODEL_DEPLOYMENT_NAME"],
        name="my-agent",
        instructions="You are helpful agent",
    )

    functions = FunctionTool(user_functions)
    code_interpreter = CodeInterpreterTool()

    toolset = ToolSet()
    toolset.add(functions)
    toolset.add(code_interpreter)

    # To enable tool calls executed automatically
    agents_client.enable_auto_function_calls(toolset)

    agent = agents_client.create_agent(
        model=os.environ["MODEL_DEPLOYMENT_NAME"],
        name="my-agent",
        instructions="You are a helpful agent",
        toolset=toolset,
    )


    file_search_tool = FileSearchTool(vector_store_ids=[vector_store.id])

    # Notices that FileSearchTool as tool and tool_resources must be added or the agent unable to search the file
    agent = agents_client.create_agent(
        model=os.environ["MODEL_DEPLOYMENT_NAME"],
        name="my-agent",
        instructions="You are helpful agent",
        tools=file_search_tool.definitions,
        tool_resources=file_search_tool.resources,
    )


    # Create Agent with File Search
    file = agents_client.files.upload_and_poll(file_path=asset_file_path, purpose=FilePurpose.AGENTS)
    print(f"Uploaded file, file ID: {file.id}")

    vector_store = agents_client.vector_stores.create_and_poll(file_ids=[file.id], name="my_vectorstore")
    print(f"Created vector store, vector store ID: {vector_store.id}")

    # Create file search tool with resources followed by creating agent
    file_search = FileSearchTool(vector_store_ids=[vector_store.id])

    agent = agents_client.create_agent(
        model=os.environ["MODEL_DEPLOYMENT_NAME"],
        name="my-agent",
        instructions="Hello, you are helpful agent and can search information from uploaded files",
        tools=file_search.definitions,
        tool_resources=file_search.resources,
    )



    # Create Agent with Enterprise File Search
    # We will upload the local file to Azure and will use it for vector store creation.
    asset_uri = os.environ["AZURE_BLOB_URI"]

    # Create a vector store with no file and wait for it to be processed
    ds = VectorStoreDataSource(asset_identifier=asset_uri, asset_type=VectorStoreDataSourceAssetType.URI_ASSET)
    vector_store = agents_client.vector_stores.create_and_poll(data_sources=[ds], name="sample_vector_store")
    print(f"Created vector store, vector store ID: {vector_store.id}")

    # Create a file search tool
    file_search_tool = FileSearchTool(vector_store_ids=[vector_store.id])

    # Notices that FileSearchTool as tool and tool_resources must be added or the agent unable to search the file
    agent = agents_client.create_agent(
        model=os.environ["MODEL_DEPLOYMENT_NAME"],
        name="my-agent",
        instructions="You are helpful agent",
        tools=file_search_tool.definitions,
        tool_resources=file_search_tool.resources,
    )


    # Create a vector store with no file and wait for it to be processed
    vector_store = agents_client.vector_stores.create_and_poll(data_sources=[], name="sample_vector_store")
    print(f"Created vector store, vector store ID: {vector_store.id}")

    ds = VectorStoreDataSource(asset_identifier=asset_uri, asset_type=VectorStoreDataSourceAssetType.URI_ASSET)
    # Add the file to the vector store or you can supply data sources in the vector store creation
    vector_store_file_batch = agents_client.vector_store_file_batches.create_and_poll(
        vector_store_id=vector_store.id, data_sources=[ds]
    )
    print(f"Created vector store file batch, vector store file batch ID: {vector_store_file_batch.id}")

    # Create a file search tool
    file_search_tool = FileSearchTool(vector_store_ids=[vector_store.id])


    # Create Agent with Code Interpreter
    file = agents_client.files.upload_and_poll(file_path=asset_file_path, purpose=FilePurpose.AGENTS)
    print(f"Uploaded file, file ID: {file.id}")

    code_interpreter = CodeInterpreterTool(file_ids=[file.id])

    # Create agent with code interpreter tool and tools_resources
    agent = agents_client.create_agent(
        model=os.environ["MODEL_DEPLOYMENT_NAME"],
        name="my-agent",
        instructions="You are helpful agent",
        tools=code_interpreter.definitions,
        tool_resources=code_interpreter.resources,
    )


    # Create Agent with Bing Grounding
    conn_id = os.environ["AZURE_BING_CONNECTION_ID"]

    # Initialize agent bing tool and add the connection id
    bing = BingGroundingTool(connection_id=conn_id)

    # Create agent with the bing tool and process agent run
    with agents_client:
        agent = agents_client.create_agent(
            model=os.environ["MODEL_DEPLOYMENT_NAME"],
            name="my-agent",
            instructions="You are a helpful agent",
            tools=bing.definitions,
        )

    # Create Agent with Azure AI Search
    conn_id = os.environ["AI_AZURE_AI_CONNECTION_ID"]

    print(conn_id)

    # Initialize agent AI search tool and add the search index connection id
    ai_search = AzureAISearchTool(
        index_connection_id=conn_id, index_name="sample_index", query_type=AzureAISearchQueryType.SIMPLE, top_k=3, filter=""
    )

    # Create agent with AI search tool and process agent run
    with agents_client:
        agent = agents_client.create_agent(
            model=os.environ["MODEL_DEPLOYMENT_NAME"],
            name="my-agent",
            instructions="You are a helpful agent",
            tools=ai_search.definitions,
            tool_resources=ai_search.resources,
        )

    
    # Fetch and log all messages
    messages = agents_client.messages.list(thread_id=thread.id, order=ListSortOrder.ASCENDING)
    for message in messages:
        if message.role == MessageRole.AGENT and message.url_citation_annotations:
            placeholder_annotations = {
                annotation.text: f" [see {annotation.url_citation.title}] ({annotation.url_citation.url})"
                for annotation in message.url_citation_annotations
            }
            for message_text in message.text_messages:
                message_str = message_text.text.value
                for k, v in placeholder_annotations.items():
                    message_str = message_str.replace(k, v)
                print(f"{message.role}: {message_str}")
        else:
            for message_text in message.text_messages:
                print(f"{message.role}: {message_text.text.value}")

    
    # Create Agent with Function Call
    functions = FunctionTool(user_functions)
    toolset = ToolSet()
    toolset.add(functions)
    agents_client.enable_auto_function_calls(toolset)

    agent = agents_client.create_agent(
        model=os.environ["MODEL_DEPLOYMENT_NAME"],
        name="my-agent",
        instructions="You are a helpful agent",
        toolset=toolset,
    )

    # For asynchronous functions, you must import AgentsClient from azure.ai.agents.aio and use AsyncFunctionTool. Here is an example using asynchronous user functions:
    functions = AsyncFunctionTool(user_async_functions)

    toolset = AsyncToolSet()
    toolset.add(functions)
    agents_client.enable_auto_function_calls(toolset)

    agent = await agents_client.create_agent(
        model=os.environ["MODEL_DEPLOYMENT_NAME"],
        name="my-agent",
        instructions="You are a helpful agent",
        toolset=toolset,
    )


    # Create Agent With Azure Function Call
    azure_function_tool = AzureFunctionTool(
        name="foo",
        description="Get answers from the foo bot.",
        parameters={
            "type": "object",
            "properties": {
                "query": {"type": "string", "description": "The question to ask."},
                "outputqueueuri": {"type": "string", "description": "The full output queue uri."},
            },
        },
        input_queue=AzureFunctionStorageQueue(
            queue_name="azure-function-foo-input",
            storage_service_endpoint=storage_service_endpoint,
        ),
        output_queue=AzureFunctionStorageQueue(
            queue_name="azure-function-tool-output",
            storage_service_endpoint=storage_service_endpoint,
        ),
    )

    agent = agents_client.create_agent(
        model=os.environ["MODEL_DEPLOYMENT_NAME"],
        name="azure-function-agent-foo",
        instructions=f"You are a helpful support agent. Use the provided function any time the prompt contains the string 'What would foo say?'. When you invoke the function, ALWAYS specify the output queue uri parameter as '{storage_service_endpoint}/azure-function-tool-output'. Always responds with \"Foo says\" and then the response from the tool.",
        tools=azure_function_tool.definitions,
    )
    print(f"Created agent, agent ID: {agent.id}")



    # Create and Deploy Azure Function
import azure.functions as func
import logging
import json

app = func.FunctionApp()

@app.function_name(name="GetWeather")
@app.queue_trigger(arg_name="inputQueue",
                   queue_name="input",
                   connection="DEPLOYMENT_STORAGE_CONNECTION_STRING")
@app.queue_output(arg_name="outputQueue",
                  queue_name="output",
                  connection="DEPLOYMENT_STORAGE_CONNECTION_STRING")
def queue_trigger(inputQueue: func.QueueMessage, outputQueue: func.Out[str]):
    try:
        messagepayload = json.loads(inputQueue.get_body().decode("utf-8"))
        location = messagepayload["location"]
        weather_result = f"Weather is 82 degrees and sunny in {location}."

        response_message = {
            "Value": weather_result,
            "CorrelationId": messagepayload["CorrelationId"]
        }

        outputQueue.set(json.dumps(response_message))

        logger.info(f"Sent message to output queue with message {response_message}")
    except Exception as e:
        logging.error(f"Error processing message: {e}")
        return
    


    # Create Agent With Logic Apps
    # Create the agents client
    agents_client = AgentsClient(
        endpoint=os.environ["PROJECT_ENDPOINT"],
        credential=DefaultAzureCredential(),
    )

    # Extract subscription and resource group from the project scope
    subscription_id = os.environ["SUBSCRIPTION_ID"]
    resource_group = os.environ["resource_group_name"]

    # Logic App details
    logic_app_name = "<LOGIC_APP_NAME>"
    trigger_name = "<TRIGGER_NAME>"

    # Create and initialize AzureLogicAppTool utility
    logic_app_tool = AzureLogicAppTool(subscription_id, resource_group)
    logic_app_tool.register_logic_app(logic_app_name, trigger_name)
    print(f"Registered logic app '{logic_app_name}' with trigger '{trigger_name}'.")

    # Create the specialized "send_email_via_logic_app" function for your agent tools
    send_email_func = create_send_email_function(logic_app_tool, logic_app_name)

    # Prepare the function tools for the agent
    functions_to_use: Set = {
        fetch_current_datetime,
        send_email_func,  # This references the AzureLogicAppTool instance via closure
    }


    # Create Agent With OpenAPI
    with open(weather_asset_file_path, "r") as f:
    openapi_weather = jsonref.loads(f.read())

    with open(countries_asset_file_path, "r") as f:
        openapi_countries = jsonref.loads(f.read())

    # Create Auth object for the OpenApiTool (note that connection or managed identity auth setup requires additional setup in Azure)
    auth = OpenApiAnonymousAuthDetails()

    # Initialize agent OpenApi tool using the read in OpenAPI spec
    openapi_tool = OpenApiTool(
        name="get_weather", spec=openapi_weather, description="Retrieve weather information for a location", auth=auth
    )
    openapi_tool.add_definition(
        name="get_countries", spec=openapi_countries, description="Retrieve a list of countries", auth=auth
    )

    # Create agent with OpenApi tool and process agent run
    with agents_client:
        agent = agents_client.create_agent(
            model=os.environ["MODEL_DEPLOYMENT_NAME"],
            name="my-agent",
            instructions="You are a helpful agent",
            tools=openapi_tool.definitions,
        )

    thread = agents_client.threads.create()


    # Create Thread with Tool Resource
    file = agents_client.files.upload_and_poll(file_path=asset_file_path, purpose=FilePurpose.AGENTS)
    print(f"Uploaded file, file ID: {file.id}")

    vector_store = agents_client.vector_stores.create_and_poll(file_ids=[file.id], name="my_vectorstore")
    print(f"Created vector store, vector store ID: {vector_store.id}")

    # Create file search tool with resources followed by creating agent
    file_search = FileSearchTool(vector_store_ids=[vector_store.id])

    agent = agents_client.create_agent(
        model=os.environ["MODEL_DEPLOYMENT_NAME"],
        name="my-agent",
        instructions="Hello, you are helpful agent and can search information from uploaded files",
        tools=file_search.definitions,
    )

    print(f"Created agent, ID: {agent.id}")

    # Create thread with file resources.
    # If the agent has multiple threads, only this thread can search this file.
    thread = agents_client.threads.create(tool_resources=file_search.resources)

    # List Threads
    threads = agents_client.threads.list()

    # Create Message
    message = agents_client.messages.create(thread_id=thread.id, role="user", content="Hello, tell me a joke")

    # Create Message with File Search Attachment
    attachment = MessageAttachment(file_id=file.id, tools=FileSearchTool().definitions)
    message = agents_client.messages.create(
        thread_id=thread.id, role="user", content="What feature does Smart Eyewear offer?", attachments=[attachment]
    )

    # Create Message with Code Interpreter Attachment
    # Notice that CodeInterpreter must be enabled in the agent creation,
    # otherwise the agent will not be able to see the file attachment for code interpretation
    agent = agents_client.create_agent(
        model=os.environ["MODEL_DEPLOYMENT_NAME"],
        name="my-agent",
        instructions="You are helpful agent",
        tools=CodeInterpreterTool().definitions,
    )
    print(f"Created agent, agent ID: {agent.id}")

    thread = agents_client.threads.create()
    print(f"Created thread, thread ID: {thread.id}")

    # Create an attachment
    attachment = MessageAttachment(file_id=file.id, tools=CodeInterpreterTool().definitions)

    # Create a message
    message = agents_client.messages.create(
        thread_id=thread.id,
        role="user",
        content="Could you please create bar chart in TRANSPORTATION sector for the operating profit from the uploaded csv file and provide file to me?",
        attachments=[attachment],
    )

    # Azure blob storage can be used as a message attachment. In this case, use VectorStoreDataSource as a data source:
    # We will upload the local file to Azure and will use it for vector store creation.
    asset_uri = os.environ["AZURE_BLOB_URI"]
    ds = VectorStoreDataSource(asset_identifier=asset_uri, asset_type=VectorStoreDataSourceAssetType.URI_ASSET)

    # Create a message with the attachment
    attachment = MessageAttachment(data_source=ds, tools=code_interpreter.definitions)
    message = agents_client.messages.create(
        thread_id=thread.id, role="user", content="What does the attachment say?", attachments=[attachment]
    )


'''
Create Message with Image Inputs
You can send messages to Azure agents with image inputs in following ways:

Using an image stored as a uploaded file
Using a public image accessible via URL
Using a base64 encoded image string
The following examples demonstrate each method:

'''

    # Create message using uploaded image file
    # Upload the local image file
    image_file = agents_client.files.upload_and_poll(file_path="image_file.png", purpose="assistants")

    # Construct content using uploaded image
    file_param = MessageImageFileParam(file_id=image_file.id, detail="high")
    content_blocks = [
        MessageInputTextBlock(text="Hello, what is in the image?"),
        MessageInputImageFileBlock(image_file=file_param),
    ]

    # Create the message
    message = agents_client.messages.create(
        thread_id=thread.id,
        role="user",
        content=content_blocks
    )


    #Create message with an image URL input
    # Specify the public image URL
    image_url = "https://upload.wikimedia.org/wikipedia/commons/thumb/d/dd/Gfp-wisconsin-madison-the-nature-boardwalk.jpg/2560px-Gfp-wisconsin-madison-the-nature-boardwalk.jpg"

    # Create content directly referencing image URL
    url_param = MessageImageUrlParam(url=image_url, detail="high")
    content_blocks = [
        MessageInputTextBlock(text="Hello, what is in the image?"),
        MessageInputImageUrlBlock(image_url=url_param),
    ]

    # Create the message
    message = agents_client.messages.create(
        thread_id=thread.id,
        role="user",
        content=content_blocks
    )


    # Create message with base64-encoded image input
    import base64

    def image_file_to_base64(path: str) -> str:
        with open(path, "rb") as f:
            return base64.b64encode(f.read()).decode("utf-8")

    # Convert your image file to base64 format
    image_base64 = image_file_to_base64("image_file.png")

    # Prepare the data URL
    img_data_url = f"data:image/png;base64,{image_base64}"

    # Use base64 encoded string as image URL parameter
    url_param = MessageImageUrlParam(url=img_data_url, detail="high")
    content_blocks = [
        MessageInputTextBlock(text="Hello, what is in the image?"),
        MessageInputImageUrlBlock(image_url=url_param),
    ]

    # Create the message
    message = agents_client.messages.create(
        thread_id=thread.id,
        role="user",
        content=content_blocks
    )