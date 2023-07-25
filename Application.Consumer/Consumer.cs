using Application.Publisher;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Application.Consumer
{
    public class Consumer
    {
        public static async Task<string> ConsumerMessage(ConfigModel queueConfig)
        {
            var connectionFactory = new ConnectionFactory()
            {
                Uri = new Uri($"amqp://{queueConfig.Username}:{queueConfig.QueuePass}@{queueConfig.QueueHost}:{queueConfig.QueuePort}")
            };
            using var connection = connectionFactory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queueConfig.QueueName, true, false, false, null);
            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (sender, e) =>
            {
                var msg = System.Text.Encoding.UTF8.GetString(e.Body.ToArray());
                channel.BasicAck(e.DeliveryTag, true);
            };
            var msg = channel.BasicConsume(queueConfig.QueueName, false, consumer);
            return msg;
        }
    }
}