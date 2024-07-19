using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SignalRService.Interfaces;
using SignalRService.Models;
using System.Text;
namespace SignalRService.BackgroundServices
{
    public class RabbitMQService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IConnectionFactory _connectionFactory;
        public RabbitMQService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _connectionFactory = new ConnectionFactory()
            {
                UserName = "guest",
                Password = "guest",
                HostName = "127.0.0.1",
            };
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            IConnection conn = _connectionFactory.CreateConnection();
            IModel channel = conn.CreateModel();
            channel.QueueDeclare("SignalRQueue", 
                durable: true, 
                exclusive: false, 
                autoDelete: false);
            channel.QueueBind(exchange: "SubmissionExchanage", queue: "SignalRQueue", routingKey: "SignalRQueueE");
            EventingBasicConsumer consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (sender, args) =>
            {
                channel.BasicAck(args.DeliveryTag, false);
                JudgeResponse response = JsonConvert.DeserializeObject<JudgeResponse>(Encoding.UTF8.GetString(args.Body.ToArray()))!;
                using (var scope = _serviceProvider.CreateScope())
                {
                    IHubContext<SubmissionHub,ISubmissionHub> _hub = scope.ServiceProvider.GetRequiredService<IHubContext<SubmissionHub, ISubmissionHub>>();
                    Console.WriteLine(response.Time_usage);
                    await _hub.Clients.Group(response.submissionID).UpdateSubmission(response);
                }
            };
            channel.BasicQos(0, 1, true);
            channel.BasicConsume(
                queue: "SignalRQueue",
                consumer: consumer,
                autoAck: false
                );
        }
    }
}
