#pragma warning disable SKEXP0010
#pragma warning disable SKEXP0070

using Demo.Models;

using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;

const string ByeCommand = "bye";
const string WelcomeMessage = "\nWelcome aboard the Nautilus submarine, the esteemed abode of the «List of Extraordinary AIs».\nWe take upon ourselves the most arduous of missions, and it is not uncommon for us to rescue the world once or twice a week.\n\nWith which of our distinguished agents do you wish to make acquaintance?\n";
const string FarewellMessage = "\nDo not forget to say «bye» when leaving...\n";
const string MissionPrompt = "Mission: ";
const string MissionResults = "\nMission results: ";

const ConsoleColor colorMistral = ConsoleColor.Yellow;
const ConsoleColor colorLlama = ConsoleColor.Blue;
const ConsoleColor colorPhi = ConsoleColor.Green;

var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(@"appsettings.json", optional: false)
                .AddJsonFile($@"appsettings.{Environment.UserName}.json", optional: true, reloadOnChange: true)
                .AddJsonFile(@"prompts.json", optional: false)
                .AddEnvironmentVariables();

IConfiguration config = builder.Build();
var configMistral = config.GetSection(@"MistralModelConfiguration").Get<Config>()!;
var configLlama = config.GetSection(@"LlamaModelConfiguration").Get<Config>()!;
var configPhi3 = config.GetSection(@"Phi3ModelConfiguration").Get<Config>()!;
var promptsValues = config.GetSection(@"prompts").Get<Prompts>()!;

var kernelMistral = CreateKernel(configMistral);
var kernelLlama = CreateKernel(configLlama);
var kernelPhi = CreateKernel(configPhi3);

var modelMap = new Dictionary<string, (Kernel kernel, string prompt, ConsoleColor color, string agentName, string intro)>
{
    { @"1", (kernelMistral, promptsValues.Mistral, colorMistral, @"Lady Mistral", "Good day, this is Lady Mistral speaking. How may I assist you today?") },
    { @"2", (kernelLlama, promptsValues.Llama, colorLlama, @"Mister Llama", "Good day, this is Mister Llama speaking, Intelligence Manager. What is the mission?") },
    { @"3", (kernelPhi, promptsValues.Phi3, colorPhi, @"The incredible Phi-3", "You've reached Phi-3. Speak swiftly and state your purpose.") }
};

var userInput = string.Empty;
CancellationTokenSource cts = new();
var cancellationToken = cts.Token;

while (!string.Equals(userInput, ByeCommand, StringComparison.OrdinalIgnoreCase))
{
    DisplayWelcomeMessage();

    userInput = Console.ReadLine();

    if (!string.IsNullOrWhiteSpace(userInput) && modelMap.TryGetValue(userInput, out var model))
    {
        Console.ForegroundColor = model.color;
        Console.WriteLine($"\n{model.intro}");
        Console.ResetColor();
        Console.Write(MissionPrompt);
        userInput = Console.ReadLine();
        Console.Write(MissionResults);
        var result = CallModelAsync(model.kernel, model.prompt, userInput!, cancellationToken);
        Console.ForegroundColor = model.color;

        if (result != null)
        {
            await foreach (var message in result)
            {
                Console.Write(message.Content);
            }

            Console.ResetColor();
            Console.WriteLine("\n\n——————————————");
        }
    }
}

cts.Dispose();

static Kernel CreateKernel(Config config)
{
    return Kernel.CreateBuilder()
                 .AddOllamaChatCompletion(modelId: config.ModelId, endpoint: new Uri(config.Endpoint), serviceId: config.ServiceId)
                 .Build();
}

static IAsyncEnumerable<StreamingChatMessageContent> CallModelAsync(Kernel kernel, string modelSystemPrompt, string mission, CancellationToken cancellationToken)
{
    var chatService = kernel.GetRequiredService<IChatCompletionService>();

    var chatHistory = new ChatHistory();
    chatHistory.AddSystemMessage(modelSystemPrompt);
    chatHistory.AddUserMessage(mission);

    return chatService.GetStreamingChatMessageContentsAsync(chatHistory, kernel: kernel, cancellationToken: cancellationToken);
}

static void DisplayWelcomeMessage()
{
    Console.ResetColor();
    Console.WriteLine(WelcomeMessage);
    Console.ForegroundColor = colorMistral;
    Console.WriteLine(@" 1. Lady Mistral");
    Console.ForegroundColor = colorLlama;
    Console.WriteLine(@" 2. Mister Llama");
    Console.ForegroundColor = colorPhi;
    Console.WriteLine(@" 3. The incredible Phi-3");
    Console.ResetColor();
    Console.WriteLine(FarewellMessage);
}

#pragma warning restore SKEXP0070
#pragma warning restore SKEXP0010
