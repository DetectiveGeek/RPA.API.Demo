using Application.Consumer;
using Application.Publisher;
using Microsoft.Extensions.Configuration;

internal class Program
{
    private static async Task Main(string[] args)
    {
        Console.WriteLine("Hello, World!");

        // Build the configuration
        IConfiguration configuration = new ConfigurationBuilder()
         .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
         .Build();

        // Retrieve the RabbitMQ configuration values
        ConfigModel queueConfig = new ConfigModel()
        {
            QueueHost = configuration["RabbitMQ:HostName"],
            QueuePort = int.Parse(configuration["RabbitMQ:Port"]),
            Username = configuration["RabbitMQ:UserName"],
            QueuePass = configuration["RabbitMQ:Password"],
            ExchangeName = configuration["RabbitMQ:ExchangeName"],
            QueueName = "DummyQueue"
        };

        // Call Publisher with your required Message
        var reply = await Consumer.ConsumerMessage(queueConfig);

        Console.WriteLine($"Message Consumed: {reply}");
        Console.ReadLine();

    }
}