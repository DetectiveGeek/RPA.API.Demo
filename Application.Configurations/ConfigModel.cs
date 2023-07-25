namespace Application.Publisher
{
    public class ConfigModel
    {
        public string? QueueHost { get; set; }
        public int QueuePort { get; set; }
        public string? Username { get; set; }
        public string? QueuePass { get; set; }
        public string? QueueName { get; set; }
        public string? ExchangeName { get; set;}
    }
}
