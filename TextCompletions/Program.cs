
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using OpenAI;
using OpenAI.Chat;
using System.ClientModel;

// 1. Read configuration from User Secrets
IConfiguration config = new ConfigurationBuilder()
    .AddUserSecrets<Program>()
    .Build();

// 2. Read AI configuration
var apiKey = config["Gemini:ApiKey"];
var endpoint = config["AI:Endpoint"];
var model = config["AI:Model"];

if (string.IsNullOrWhiteSpace(apiKey))
{
    throw new Exception("AI API key not found.");
}

if (string.IsNullOrWhiteSpace(endpoint) ||
    string.IsNullOrWhiteSpace(model))
{
    throw new Exception("AI endpoint or model not configured.");
}

try
{
    // 3. Create the provider-specific client
    var openAIClient = new ChatClient(
        model: model,
        credential: new ApiKeyCredential(apiKey),
        options: new OpenAIClientOptions
        {
            Endpoint = new Uri(endpoint)
        });

    // 4. Convert it into the common IChatClient interface
    IChatClient chatClient =
        openAIClient.AsIChatClient();

    Console.WriteLine("AI Chatbot started!");

    #region Streaming response note 
    // 5. Chat loop
    //while (true)
    //{
    //    Console.Write("\nYou: ");

    //    var question = Console.ReadLine();

    //    if (string.IsNullOrWhiteSpace(question))
    //        continue;

    //    if (question.Equals("exit",
    //        StringComparison.OrdinalIgnoreCase))
    //    {
    //        break;
    //    }

    //    // 6. Send question to the AI model
    //    var response = chatClient.GetStreamingResponseAsync(question);

    //    // 7. Display AI response
    //    await foreach (var message in response)
    //    {
    //        Console.Write(message.Text);
    //    }
    //}


    #endregion

    #region Classification example

    //var classificationPrompt = """
    //Please classify the following sentences into categories: 
    //- 'complaint' 
    //- 'suggestion' 
    //- 'praise' 
    //- 'other'.

    //1) "I love the new layout!"
    //2) "You should add a night mode."
    //3) "When I try to log in, it keeps failing."
    //4) "This app is decent."
    //""";

    //Console.WriteLine($"user >>> {classificationPrompt}");

    //ChatResponse classificationResponse = await chatClient.GetResponseAsync(classificationPrompt);

    //Console.WriteLine($"assistant >>>\n{classificationResponse}");

    #endregion

    #region Summarization example

    //var summaryPrompt = """
    //Summarize the following blog in 1 concise sentences:

    //"Microservices architecture is increasingly popular for building complex applications, but it comes with additional overhead. It's crucial to ensure each service is as small and focused as possible, and that the team invests in robust CI/CD pipelines to manage deployments and updates. Proper monitoring is also essential to maintain reliability as the system grows."
    //""";

    //Console.WriteLine($"user >>> {summaryPrompt}");

    //ChatResponse summaryResponse = await chatClient.GetResponseAsync(summaryPrompt);

    //Console.WriteLine($"assistant >>> \n{summaryResponse}");

    #endregion

    #region Sentiment analysis example

    //var analysisPrompt = """
    //        You will analyze the sentiment of the following product reviews. 
    //        Each line is its own review. Output the sentiment of each review in a bulleted list and then provide a generate sentiment of all reviews.

    //        I bought this product and it's amazing. I love it!
    //        This product is terrible. I hate it.
    //        I'm not sure about this product. It's okay.
    //        I found this product based on the other reviews. It worked for a bit, and then it didn't.
    //        """;

    //Console.WriteLine($"user >>> {analysisPrompt}");

    //ChatResponse responseAnalysis = await chatClient.GetResponseAsync(analysisPrompt);

    //Console.WriteLine($"assistant >>> \n{responseAnalysis}");

    #endregion
}
catch (Exception ex)
{
    Console.WriteLine("Something went wrong:");
    Console.WriteLine(ex.Message);
}
