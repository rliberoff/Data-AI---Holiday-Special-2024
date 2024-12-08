#pragma warning disable SKEXP0001
#pragma warning disable SKEXP0010
#pragma warning disable SKEXP0050

using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;

const string modelPhi3 = @"phi3:medium";
const string endpointOllama = "http://localhost:11434";

var kernelBuilderMistral = Kernel.CreateBuilder()
                                 .AddOpenAIChatCompletion(modelId: modelPhi3, endpoint: new Uri(endpointOllama), apiKey: null, serviceId: modelPhi3)
                                 ;

var kernelBuilderLlama = Kernel.CreateBuilder()
                               .AddOpenAIChatCompletion(modelId: modelPhi3, endpoint: new Uri(endpointOllama), apiKey: null, serviceId: modelPhi3)
                               ;

var kernelBuilderPhi = Kernel.CreateBuilder()
                             .AddOpenAIChatCompletion(modelId: modelPhi3, endpoint: new Uri(endpointOllama), apiKey: null, serviceId: modelPhi3)
                             ;

var kernelMistral = kernelBuilderMistral.Build();
var kernelLlama = kernelBuilderLlama.Build();
var kernelPhi = kernelBuilderPhi.Build();


async Task CallMistralAsync(string mission, CancellationToken cancellationToken)
{
    var chatService = kernelMistral.GetRequiredService<IChatCompletionService>();

    var chatHistory = new ChatHistory();
    chatHistory.AddSystemMessage("profile");
    chatHistory.AddUserMessage(mission);

    var result = chatService.GetStreamingChatMessageContentsAsync(chatHistory, kernel: kernelMistral, cancellationToken: cancellationToken);

    await foreach (var message in result)
    {
        Console.Write(message.Content);
    }
}

var userInput = string.Empty;

while (!userInput.Equals(@"bye", StringComparison.OrdinalIgnoreCase))
{
    // Render menu

    // Get user mission

    // Process user mission

    // Show result

}

// Render Menu


#pragma warning restore SKEXP0050
#pragma warning restore SKEXP0010
#pragma warning restore SKEXP0001
