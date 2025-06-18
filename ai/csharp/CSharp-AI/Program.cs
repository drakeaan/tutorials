namespace CSharp_AI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            AzureOpenAIClient openAIClient = new(
    new Uri("https://your-azure-openai-resource.com"),
    new DefaultAzureCredential());
            ChatClient chatClient = openAIClient.GetChatClient("my-gpt-4o-mini-deployment");
        }
    }
}
