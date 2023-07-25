using Newtonsoft.Json;
using RabbitMQ.Client;


namespace Application.Publisher
{
    public class Publisher
    {

        public static async Task<bool> PublishMessage(ConfigModel queueConfig, string message)
        {
            var connectionFactory = new ConnectionFactory
            {
                Uri = new Uri($"amqp://{queueConfig.Username}:{queueConfig.QueuePass}@{queueConfig.QueueHost}:{queueConfig.QueuePort}")
            };

            using var connection = connectionFactory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queueConfig.QueueName, true, false, false, null);
            var body = JsonConvert.SerializeObject(message);
            var body2 = System.Text.Encoding.UTF8.GetBytes(body);
            channel.BasicPublish(string.Empty, queueConfig.QueueName, null, body2);
            return true;
        }
    }


}
