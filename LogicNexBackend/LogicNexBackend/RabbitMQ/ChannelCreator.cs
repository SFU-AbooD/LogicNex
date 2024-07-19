using RabbitMQ.Client;

namespace LogicNexBackend.RabbitMQ
{
    public class ChannelCreator
    {
        public IModel channel;
        public ChannelCreator()
        {
            IConnectionFactory connectionFactory = new ConnectionFactory()
            {
                UserName = "guest",
                Password = "guest",
                HostName = "127.0.0.1",
                DispatchConsumersAsync = true
            };
            IConnection connection = connectionFactory.CreateConnection();
            channel = connection.CreateModel();

        }
    }
}
