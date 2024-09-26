jest.mock('../events/client', () => ({
    createPublisher: jest.fn(() => ({
      send: jest.fn().mockResolvedValue(null),
    })),
    createConsumer: jest.fn(() => ({
      on: jest.fn(),
    })),
  }));

beforeEach(() => {
  jest.resetAllMocks();
});

export {};
