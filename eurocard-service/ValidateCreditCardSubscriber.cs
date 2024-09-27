using System;
using System.Text;
using Newtonsoft.Json.Linq;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ValidateCreditCardPublisherDotnet
{
    public class ValidateCreditCardSubscriber
    {
        private static readonly string QUEUE_NAME = Environment.GetEnvironmentVariable("QUEUE_NAME") ?? "creditcard_checker";
        private static readonly string RABBITMQ_HOST = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? "localhost";
        private static readonly string RABBITMQ_PORT = Environment.GetEnvironmentVariable("RABBITMQ_PORT") ?? "5672";
        private static readonly string RABBITMQ_USERNAME = Environment.GetEnvironmentVariable("RABBITMQ_USERNAME") ?? "guest";
        private static readonly string RABBITMQ_PASSWORD = Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD") ?? "guest";

        private IConnection connection;
        private IModel channel;

        public void Run()
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

                connection = factory.CreateConnection();

                Console.WriteLine($"Connecting to RabbitMQ at {RABBITMQ_HOST}:{RABBITMQ_PORT} with user {RABBITMQ_USERNAME}");

                channel = connection.CreateModel();


                channel.QueueDeclare(queue: QUEUE_NAME, durable: false, exclusive: false, autoDelete: false, arguments: null);


                Console.WriteLine($"[*] Waiting for messages in queue: {QUEUE_NAME}. Press [enter] to exit.");

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += OnMessageReceived;

                channel.BasicConsume(queue: QUEUE_NAME, autoAck: false, consumer: consumer);
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[!] Error in RabbitMQ connection: {ex.Message}");
            }
        }

        private void OnMessageReceived(object model, BasicDeliverEventArgs ea)
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine($"[x] Received message: {message}");

            bool isValid = ValidateCreditCard(message);
            string validationResult = isValid ? "valid" : "invalid";

            JObject validationMessage = new()
            {
                { "username", JObject.Parse(message)["username"] },
                { "creditCard", JObject.Parse(message)["creditCard"] },
                { "isValid", isValid }
            };

            string validationString = validationMessage.ToString();
            byte[] responseMessage = Encoding.UTF8.GetBytes(validationString);

            try
            {
                var publisher = new ValidateCreditCardPublisher();
                publisher.PublishValidationResult(responseMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[!] Failed to publish message: {ex.Message}");
            }

            channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
        }

        private bool ValidateCreditCard(string message)
        {
            try
            {
                JObject json = JObject.Parse(message);
                string creditCard = (string)json["creditCard"];

                if (int.TryParse(creditCard, out int cardNumber))
                {
                    return cardNumber % 2 == 0;
                }

                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[x] Error parsing message: {ex.Message}");
                return false;
            }
        }
    }
}