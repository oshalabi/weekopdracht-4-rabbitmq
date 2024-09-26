import { Connection } from 'rabbitmq-client';


const client = new Connection('amqp://guest:guest@localhost:5672');

client.on('error', (err) => {
    console.log('====================================');
    console.log("Connection error", err);
    console.log('====================================');
})
client.on('connection', () => {
    console.log('====================================');
    console.log("Connection successfully (re)established");
    console.log('====================================');
})

export default client;