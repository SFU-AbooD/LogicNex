using JudgeSystem.Models;
using RabbitMQ.Client;
namespace JudgeSystem.Consumer
{
    internal class JudgeConsumer
    {
        private readonly IModel _channel;
        private readonly IConnection _connection;
        public JudgeConsumer(IModel channel, IConnection connection)
        {
            _channel = channel;
            _connection = connection;
            initRabbitMQ();
        }
        private void initRabbitMQ()
        {
            _channel.ExchangeDeclare(
                exchange:"SubmissionExchanage",
                type:ExchangeType.Direct,
                durable:true,
                autoDelete:false
                );
            _channel.QueueDeclare(
                queue: "SubmissionQueue",
                exclusive:false,
                autoDelete:false,
                durable:true
                );
            _channel.QueueBind(
                queue:"SubmissionQueue",
                exchange:"SubmissionExchanage", 
                routingKey:"Submission");
            ConsumerDefinition consumer = new ConsumerDefinition(_channel,_connection);
            _channel.BasicConsume("SubmissionQueue", false, consumer);
        }
    }
}
