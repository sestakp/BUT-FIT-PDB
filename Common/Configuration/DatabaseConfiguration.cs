namespace Common.Configuration
{
    public class DatabaseConfiguration
    {
        public string ConnectionString { get; set; } = null!;
        public string Host { get; set; } = null!;
        public int Port { get; set; }

        public string DatabaseName { get; set; } = null!;

        public string CollectionName { get; set; } = null!;
    }
}
