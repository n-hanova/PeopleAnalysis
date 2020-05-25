using AnalyticAPI.ApplicationAPI;
using AnalyticAPI.AuthAPI;
using AnalyticAPI.Services.Settings;
using CommonCoreLibrary.Auth.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PeopleAnalysis.Services
{
    public interface IMessageReceiver
    {
        Task StartAsync();
    }

    public class RabbitMQService : IMessageReceiver, IHostedService
    {
        private readonly IServiceScopeFactory scopeFactory;
        private readonly IAIService aIService;
        private readonly ConnectionFactory factory;
        private readonly ILogger<RabbitMQService> logger;
        private readonly IAuthAPIClient authAPIClient;
        private readonly ServiceAuthConfig serviceAuthConfig;
        private readonly RabbitMQSettings rabbitMQSettings;

        public RabbitMQService(IAIService aIService, IServiceScopeFactory scopeFactory, ILogger<RabbitMQService> logger, IAuthAPIClient authAPIClient, ServiceAuthConfig serviceAuthConfig,
            RabbitMQSettings rabbitMQSettings)
        {
            this.scopeFactory = scopeFactory;
            this.aIService = aIService;
            factory = new ConnectionFactory() { HostName = rabbitMQSettings.Host };
            this.logger = logger;
            this.authAPIClient = authAPIClient;
            this.serviceAuthConfig = serviceAuthConfig;
            this.rabbitMQSettings = rabbitMQSettings;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Task.Run(() =>
            {
                using var connection = factory.CreateConnection();
                using var channel = connection.CreateModel();

                channel.QueueDeclare(queue: "task_queue",
                                     durable: true,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += async (sender, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body.ToArray());
                    var jsonObject = JObject.Parse(message);
                    try
                    {
                        using var scope = scopeFactory.CreateScope();
                        var baseTokenService = scope.ServiceProvider.GetRequiredService<IBaseTokenService>();
                        var loginResult = await authAPIClient.ApiAuthLoginPostAsync(new LoginModel
                        {
                            Login = serviceAuthConfig.Login,
                            Password = serviceAuthConfig.Password
                        });
                        await baseTokenService.SignInAsync(loginResult);
                        await aIService.ProcessTaskAsync(JsonConvert.DeserializeObject<Request>(jsonObject["message"].ToString()));
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, ex.Message);
                    }
                    finally
                    {
                        channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                    }
                };
                channel.BasicConsume(queue: "task_queue",
                                     autoAck: false,
                                     consumer: consumer);
                while (!cancellationToken.IsCancellationRequested) ;
            });
            return Task.CompletedTask;
        }

        public Task StartAsync()
        {
            throw new NotImplementedException();
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
