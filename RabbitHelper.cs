using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RabbitMQ.Client;

namespace Asp
{
    static class RabbitHelper
    {
        private static IConnection rabbitConnection;
        private static IModel rabbitChannel;

        // TODO: Allow this to be set via environmental variable
        private static string rabbitHost = "localhost";
        private static string rabbitCommandQueue = "commands";

        static RabbitHelper() 
        {
            // Ensure Rabbit Queue is set up
            var factory = new ConnectionFactory() { 
                HostName = rabbitHost,
                UserName = "scaley",
                Password = "abilities"
            };

            rabbitConnection = factory.CreateConnection();
            rabbitChannel = rabbitConnection.CreateModel();

            rabbitChannel.QueueDeclare(
                queue: rabbitCommandQueue,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );
        }

        public static void PushCommand(JObject properties) {
            rabbitChannel.BasicPublish(
                exchange: "",
                routingKey: rabbitCommandQueue,
                basicProperties: null,
                body: Encoding.UTF8.GetBytes(properties.ToString(Formatting.None))
            );
        }
    }
}