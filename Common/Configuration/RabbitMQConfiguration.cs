namespace Common.Configuration
{
    public class RabbitMQConfiguration
    {
        public string QueueName { get; set; } = null!;

        public string Hostname { get; set; } = null!;

        public int Port { get; set; }

        public string UserName { get; set; } = null!;

        public string Password { get; set; } = null!;
    }
}
