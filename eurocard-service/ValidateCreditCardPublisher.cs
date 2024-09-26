using System;
using System.Text;
using RabbitMQ.Client;

namespace ValidateCreditCardPublisherDotnet
{
    internal class ValidateCreditCardPublisher
    {
        private const string VALIDATION_QUEUE = "creditcard_queue_checker_response";

        public void PublishValidationResult(byte[] validationMessage)
        {
            var factory = new ConnectionFactory { HostName = "localhost" };

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
    }
}
