using System;
using System.Text;
using Newtonsoft.Json.Linq;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ValidateCreditCardPublisherDotnet
{
    public class ValidateCreditCardSubscriber
    {
        private const string QUEUE_NAME = "creditcard_checker";
        private const string RABBITMQ_HOST = "localhost";

        public void Run()
        {
            var factory = new ConnectionFactory()
            {
                HostName = RABBITMQ_HOST,
                UserName = "guest",
                Password = "guest"
            };

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: QUEUE_NAME,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);


            Console.WriteLine($"[*] Waiting for messages in queue: {QUEUE_NAME}. Press [enter] to exit.");

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                Console.WriteLine($"[x] Received message: {message}");

                bool isValid = ValidateCreditCard(message);
                string validationResult = isValid ? "valid" : "invalid";

                JObject validationMessage = new()

                {       { "username", JObject.Parse(message)["username"] },
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
            };

            channel.BasicConsume(queue: QUEUE_NAME, autoAck: false, consumer: consumer);

            Console.ReadLine();
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
