namespace FunctionProj;

public class Response
{
    public required string Value { get; set; }
    public required string CorrelationId { get; set; }
}

public class Arguments
{
    public required string OutputQueueUri { get; set; }
    public required string CorrelationId { get; set; }
}

public class Foo
{
    private readonly ILogger<Foo> _logger;

    public Foo(ILogger<Foo> logger)
    {
        _logger = logger;
    }

    [Function("Foo")]
    public void Run([QueueTrigger("azure-function-foo-input")] Arguments input, FunctionContext executionContext)
    {
        var logger = executionContext.GetLogger("Foo");
        logger.LogInformation("C# Queue function processed a request.");

        // We have to provide the Managed identity for function resource
        // and allow this identity a Queue Data Contributor role on the storage account.
        var cred = new DefaultAzureCredential();
        var queueClient = new QueueClient(new Uri(input.OutputQueueUri), cred,
                new QueueClientOptions { MessageEncoding = QueueMessageEncoding.Base64 });

        var response = new Response
        {
            Value = "Bar",
            // Important! Correlation ID must match the input correlation ID.
            CorrelationId = input.CorrelationId
        };

        var jsonResponse = JsonSerializer.Serialize(response);
        queueClient.SendMessage(jsonResponse);
    }
}