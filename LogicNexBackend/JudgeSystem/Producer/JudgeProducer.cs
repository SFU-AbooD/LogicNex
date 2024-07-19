using JudgeSystem.Models;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace JudgeSystem.Producer
{
    internal class JudgeProducer
    {
        private readonly IConnection _connection;
        public readonly IModel _channel;
        private readonly string _service_queue;
        public JudgeProducer(IConnection connection,string service_queue)
        {
            _connection = connection;
            _channel = _connection.CreateModel();
            _service_queue = service_queue;
        }
    }
}
