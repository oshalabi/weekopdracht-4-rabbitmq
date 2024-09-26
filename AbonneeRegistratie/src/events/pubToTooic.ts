import client from './client';
import dotenv from 'dotenv';

dotenv.config();

const pubToTopic = client.createPublisher({
  confirm: true,
});

export const publishToTopic = async (routingKey: string, message: string) => {
  try {
    await pubToTopic.send(
      {
        exchange: process.env.TOPIC_EXCHANGE,
        routingKey: routingKey,
        type: 'topic',
      },

      message
    );
    console.log(`Message sent to exchange with routing key: ${routingKey}`);
  } catch (error) {
    console.error('Failed to publish message:', error);
  }
};
