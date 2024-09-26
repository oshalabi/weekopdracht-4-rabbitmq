import client from './client';
import dotenv from 'dotenv';
import { pubToNotification } from './pubToNotification';

dotenv.config();

export const listenerValidateCreditCard = client.createConsumer(
  {
    queue: process.env.RESPONSE_QUEUE_NAME, // Response queue for CreditCard validation
    noAck: true,
  },
  async (message) => {
    // Convert the body buffer into a readable string
    const bodyBuffer = message.body; // Get the buffer from the message body
    const bodyString = Buffer.from(bodyBuffer).toString('utf8'); // Convert buffer to string

    // Parse the JSON string if needed
    let parsedMessage;
    try {
      parsedMessage = JSON.parse(bodyString);
    } catch (err) {
      console.error('Failed to parse message body:', err);
      return;
    }

    console.log('====================================');
    console.log(parsedMessage);
    console.log('====================================');

    if (parsedMessage.isValid) {
      console.log('====================================');
      console.log('Credit card validation successful');
      console.log('====================================');

      console.log('====================================');
      console.log('Sending message to Aboonnee');
      console.log('====================================');

      pubToNotification(process.env.ROUTING_KEY_ABONNEE, parsedMessage.username);

      console.log('====================================');
      console.log('Sending message to Uitgever');
      console.log('====================================');

      pubToNotification(process.env.ROUTING_KEY_UITGEVER, parsedMessage.username);
    }

    // Log the parsed message
    console.log(`Received message: ${JSON.stringify(parsedMessage)}`);
  }
);
