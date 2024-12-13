#pragma warning disable SKEXP0001
#pragma warning disable SKEXP0010
#pragma warning disable SKEXP0050

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;


#region Get Configuration Values

var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile("prompts.json", optional:false);

IConfiguration config = builder.Build();
var configValues = config.GetSection("ModelConfiguration").Get<Demo.Models.Config>();

var promptsValues = config.GetSection("prompts").Get<Demo.Models.Prompts>();

#endregion

var kernelBuilderMistral = Kernel.CreateBuilder()
                                 .AddAzureOpenAIChatCompletion(configValues.ModelId,configValues.Endpoint, apiKey: configValues.ApiKey);


var kernelBuilderLlama = Kernel.CreateBuilder()
                               .AddAzureOpenAIChatCompletion(configValues.ModelId, configValues.Endpoint, apiKey: configValues.ApiKey);

var kernelBuilderPhi = Kernel.CreateBuilder()
                                .AddAzureOpenAIChatCompletion(configValues.ModelId, configValues.Endpoint, apiKey: configValues.ApiKey);

var kernelMistral = kernelBuilderMistral.Build();
var kernelLlama = kernelBuilderLlama.Build();
var kernelPhi = kernelBuilderPhi.Build();



async Task CallModelAsync(Kernel activeModel, string modelSystemPrompt, string mission, CancellationToken cancellationToken)
{
    var chatService = activeModel.GetRequiredService<IChatCompletionService>();

    var chatHistory = new ChatHistory();
    chatHistory.AddSystemMessage(modelSystemPrompt);
    chatHistory.AddUserMessage(mission);

    var result = chatService.GetStreamingChatMessageContentsAsync(chatHistory, kernel: activeModel, cancellationToken: cancellationToken);

    await foreach (var message in result)
    {
        Console.Write(message.Content);
    }
}

var userInput = string.Empty;
CancellationTokenSource cts = new();
CancellationToken cancellationToken = cts.Token;

while (!userInput.Equals(@"bye", StringComparison.OrdinalIgnoreCase))
{

    Console.WriteLine("Welcome aboard the Nautilus submarine, the esteemed abode of the List of Extraordinary AIs. We take upon ourselves the most arduous of missions, and it is not uncommon for us to rescue the world once or twice a week. With which of our distinguished agents do you wish to make acquaintance?");

    Console.WriteLine("1. Lady Mistral");
    Console.WriteLine("2. Mister Llama");
    Console.WriteLine("3. The incredible Phi-3");

    Console.WriteLine("Do not forget to say bye for leaving");

    userInput = Console.ReadLine();
    
    switch (userInput)
    {
        case "1":
            Console.WriteLine("Good day, this is Lady Mistral speaking. How may I assist you today?");
            userInput = Console.ReadLine();
            await CallModelAsync(kernelMistral, promptsValues.Mistral, userInput, cancellationToken);
        break;
        case "2":
            Console.WriteLine("Good day, this is Mister Llama speaking, Intelligence Manager. What is the mission?");
            userInput = Console.ReadLine();
            await CallModelAsync(kernelLlama, promptsValues.Llama, "", cancellationToken);
        break;
        case "3":
            Console.WriteLine("You've reached Phi-3. Speak swiftly and state your purpose.");
            userInput = Console.ReadLine();
            await CallModelAsync(kernelPhi,  promptsValues.Phi3, "", cancellationToken);
        break;
           
    }

}


#pragma warning restore SKEXP0050
#pragma warning restore SKEXP0010
#pragma warning restore SKEXP0001
