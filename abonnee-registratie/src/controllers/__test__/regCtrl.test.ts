import request from 'supertest';
import { app } from '../../app';
import { pubValidateCreditCard } from '../../events/pubValidateCreditCard';

// Mock the RabbitMQ-related functions
jest.mock('../../events/pubValidateCreditCard', () => ({
  pubValidateCreditCard: jest.fn(),
}));


it('should return 400 if username or password is missing', async () => {
  const res = await request(app).post('/register').send({
    password: '1234',
    creditCard: '123456789',
  });

  expect(res.status).toBe(400);
  expect(res.body).toEqual({
    status: 'error',
    message: 'username and password are required',
  });

  expect(pubValidateCreditCard).not.toHaveBeenCalled();
});

it('should return 200 if all fields are present', async () => {
  const res = await request(app).post('/register').send({
    username: 'testuser',
    password: '1234',
    creditCard: '123456789',
  });

  expect(res.status).toBe(200);
  expect(res.body).toEqual({
    status: 'success',
    message: 'Credit card validation request sent',
  });

  expect(pubValidateCreditCard).toHaveBeenCalled();
});
