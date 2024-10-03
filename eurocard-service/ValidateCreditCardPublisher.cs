using System;
using System.Text;
using RabbitMQ.Client;

using DotNetEnv;

namespace ValidateCreditCardPublisherDotnet
{
    internal class ValidateCreditCardPublisher
    {
        private static readonly string VALIDATION_QUEUE = Environment.GetEnvironmentVariable("VALIDATION_QUEUE") ?? "creditcard_queue_checker_response";
        
        private static readonly string RABBITMQ_PORT = Environment.GetEnvironmentVariable("RABBITMQ_PORT") ?? "5672";
        private static readonly string RABBITMQ_USERNAME = Environment.GetEnvironmentVariable("RABBITMQ_USERNAME") ?? "guest";
        private static readonly string RABBITMQ_PASSWORD = Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD") ?? "guest";

        public void PublishValidationResult(byte[] validationMessage)
        {
            var factory = new ConnectionFactory()
            {
                HostName = RABBITMQ_HOST,
                Port = int.Parse(RABBITMQ_PORT),
                UserName = RABBITMQ_USERNAME,
                Password = RABBITMQ_PASSWORD
            };

            try
            {
                using var connection = factory.CreateConnection();

                using var channel = connection.CreateModel();

                channel.QueueDeclare(queue: VALIDATION_QUEUE,
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                channel.BasicPublish(exchange: "",
                                     routingKey: VALIDATION_QUEUE,
                                     basicProperties: null,
                                     body: validationMessage);

                string messageString = Encoding.UTF8.GetString(validationMessage);
                Console.WriteLine($"[+] Published validation result: {messageString}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[!] Failed to publish message: {ex.Message}");
            }
        }
    }
}
