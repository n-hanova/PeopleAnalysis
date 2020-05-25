using AnalyticAPI.Services.Settings;
using CommonCoreLibrary.Services;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace PeopleAnalysis.Services
{
    public interface ISender
    {
        void Send<T>(T message, object args = null);
    }

    public class RabbitMQClient : ISender
    {
        private readonly ConnectionFactory factory;

        public RabbitMQClient(RabbitMQSettings rabbitMQSettings)
        {
            factory = new ConnectionFactory() { HostName = rabbitMQSettings.Host };
        }

        public void Send<T>(T message, object args = null)
        {
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.QueueDeclare(queue: "task_queue",
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new { message, args }));
            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;

            channel.BasicPublish(exchange: "",
                                 routingKey: "task_queue",
                                 basicProperties: properties,
                                 body: body);
        }
    }
}
