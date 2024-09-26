import client from './client';
import dotenv from 'dotenv';

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

    // Log the parsed message
    console.log(`Received message: ${JSON.stringify(parsedMessage)}`);
  }
);
