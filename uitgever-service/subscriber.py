import pika
import os
from dotenv import load_dotenv

load_dotenv()


RABBITMQ_HOST = os.getenv("RABBITMQ_HOST", "localhost")
RABBITMQ_PORT = int(os.getenv("RABBITMQ_PORT", 5672))
RABBITMQ_USERNAME = os.getenv("RABBITMQ_USERNAME", "guest")
RABBITMQ_PASSWORD = os.getenv("RABBITMQ_PASSWORD", "guest")
RABBITMQ_VIRTUAL_HOST = os.getenv("RABBITMQ_VIRTUAL_HOST", "/")
TOPIC_EXCHANGE = os.getenv("TOPIC_EXCHANGE")
QUEUE = os.getenv("QUEUE")
ROUTING_KEY_UITGEVER = os.getenv("ROUTING_KEY_UITGEVER")

print(
    f"Connecting to RabbitMQ at {RABBITMQ_HOST}:{RABBITMQ_PORT} with user {RABBITMQ_USERNAME}"
)


def callback(ch, method, properties, body):
    print(f" [x] Received User:{body.decode('utf-8')}")

    ch.basic_ack(delivery_tag=method.delivery_tag)


def main():
    credentials = pika.PlainCredentials(RABBITMQ_USERNAME, RABBITMQ_PASSWORD)
    connection = pika.BlockingConnection(
        pika.ConnectionParameters(
            host=RABBITMQ_HOST,
            port=RABBITMQ_PORT,
            virtual_host=RABBITMQ_VIRTUAL_HOST,
            credentials=credentials,
        )
    )
    channel = connection.channel()

    channel.exchange_declare(exchange=TOPIC_EXCHANGE, exchange_type="topic")

    channel.queue_declare(queue=QUEUE, durable=True)

    channel.queue_bind(
        exchange=TOPIC_EXCHANGE, queue=QUEUE, routing_key=ROUTING_KEY_UITGEVER
    )

    channel.basic_consume(queue=QUEUE, on_message_callback=callback)

    print(" [*] Waiting for messages. To exit press CTRL+C")

    try:
        # Your service logic here
        channel.start_consuming()
    except KeyboardInterrupt:
        print("Interrupted by user, closing connection.")
    finally:
        if connection.is_open:
            connection.close() 


if __name__ == "__main__":
    main()
