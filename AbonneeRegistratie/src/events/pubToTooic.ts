import client from './client';

const pubToTopic = client.createPublisher({
  confirm: true,
});

const publishToTopic = async (
  exchange: string,
  routingKey: string,
  topic: string,
  message: string
) => {
  await pubToTopic.send(
    {
      exchange,
      routingKey,
    },
    { message }
  );
};
export default pubToTopic;
