import client from './client';
import dotenv from 'dotenv';

dotenv.config();

const pubToQueue = client.createPublisher({
  confirm: true,
});

export const pubValidateCreditCard = async (creditCard: string) => {
  client.queueDeclare(process.env.QUEUE_NAME);

  const msg = JSON.stringify({
    creditCard: creditCard,
    operation: 'validate',
  });

  const envelope = {
    routingKey: process.env.QUEUE_NAME,
    replyTo: process.env.RESPONSE_QUEUE_NAME,
  };

  try {
    // Publish the message to the main queue
    await pubToQueue.send(envelope, Buffer.from(msg));
    console.log(`[+] Published message to creditcard_queue: ${msg}`);
  } catch (err) {
    console.error('Error publishing message:', err);
  }
};