import pika

TOPIC_EXCHANGE = 'notification'
QUEUE = "uitgever_queue"  
ROUTING_KEY_UITGEVER = 'notification_uitgever'

def callback(ch, method, properties, body):
    print(f" [x] Received {body.decode('utf-8')}")  

    ch.basic_ack(delivery_tag=method.delivery_tag)

def main():

    connection = pika.BlockingConnection(pika.ConnectionParameters('localhost'))
    channel = connection.channel()

    channel.exchange_declare(exchange=TOPIC_EXCHANGE, exchange_type='topic')

    channel.queue_declare(queue=QUEUE, durable=True)

    channel.queue_bind(exchange=TOPIC_EXCHANGE, queue=QUEUE, routing_key=ROUTING_KEY_UITGEVER)

    channel.basic_consume(queue=QUEUE, on_message_callback=callback)

    print(' [*] Waiting for messages. To exit press CTRL+C')

    channel.start_consuming()

if __name__ == "__main__":
    main()
