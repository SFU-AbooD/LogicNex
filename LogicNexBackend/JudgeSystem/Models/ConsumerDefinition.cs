using JudgeSystem.JudgerFolder;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace JudgeSystem.Models
{
    internal class ConsumerDefinition : AsyncEventingBasicConsumer
    {
        public static SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(500);
        private readonly IModel _channel;
        private readonly IConnection _connection;
        public ConsumerDefinition(IModel channel, IConnection connection) : base(channel) {
            _channel = channel;
             Received += CustomHandler;
            _connection = connection;
        }

        private async Task CustomHandler(object sender, BasicDeliverEventArgs @event)
        {
             await _semaphoreSlim.WaitAsync();
            Console.WriteLine(_semaphoreSlim.CurrentCount);
            _channel.BasicAck(@event.DeliveryTag, false);
            try
            {
                string body = Encoding.UTF8.GetString(@event.Body.ToArray());
                MessageSubmission decoded_body =  JsonConvert.DeserializeObject<MessageSubmission>(body)!;
                _ = Task.Run(() =>
                {
                    _ = new Judger(decoded_body, _connection).Judge();
                });
            }
            finally { 
            }
        }
        
    }
}
