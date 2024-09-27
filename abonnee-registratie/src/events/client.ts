import { Connection } from 'rabbitmq-client';
import dotenv from 'dotenv';

dotenv.config();

const client = new Connection({
  hostname: process.env.RABBITMQ_HOST || 'localhost',
  port: process.env.RABBITMQ_PORT || 5672,
  username: process.env.RABBITMQ_USERNAME || 'guest',
  password: process.env.RABBITMQ_PASSWORD || 'guest',
});

client.on('error', (err) => {
  console.log('====================================');
  console.log('Connection error', err);
  console.log('====================================');
});
client.on('connection', () => {
  console.log('====================================');
  console.log('Connection successfully (re)established');
  console.log('====================================');
});

export default client;
