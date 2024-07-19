using JudgeSystem.Consumer;
using RabbitMQ.Client;
namespace JudgeSystem
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IConnectionFactory connectionFactory = new ConnectionFactory() { 
                UserName = "guest",
                Password = "guest",
                HostName="127.0.0.1",
                DispatchConsumersAsync = true
            };
            IConnection connection = connectionFactory.CreateConnection();
            IModel channel = connection.CreateModel();
            channel.BasicQos(0, 1, true);
            JudgeConsumer consumer = new JudgeConsumer(channel,connection);
            Console.ReadLine();
        }
    }
}
