
import { app } from './app';
import dotenv from 'dotenv';
import { listenerValidateCreditCard } from './events/listenerValidateCreditCard';

dotenv.config();

const start = async () => {
  if (!process.env.PORT) {
    throw new Error('PORT must be defined');
  }
  try {
    app.listen(process.env.PORT);
    console.log(`Listening on port ${process.env.PORT}`);

    listenerValidateCreditCard.on('ready', () => {
      console.log('Listener is ready and waiting for messages.');
    });
    
    listenerValidateCreditCard.on('error', (err) => {
      console.error('Error in listener:', err);
    });
  } catch (err) {
    console.error(err);
  }
};

start();

