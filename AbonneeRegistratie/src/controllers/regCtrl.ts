import { Request, Response } from 'express';
import { pubValidateCreditCard } from '../events/pubValidateCreditCard';
export const regCtrl = async (req: Request, res: Response) => {
  console.log(req.body);
  const { username, password, creditCardNumber } = req.body;

  if (!username || !password) {
    return res.status(400).json({
      status: 'error',
      message: 'username and password are required',
    });
  }

  if (!creditCardNumber) {
    return res.status(400).json({
      status: 'error',
      message: 'credit card number is required',
    });
  }
  try {
    await pubValidateCreditCard(creditCardNumber);

    console.log('====================================');
    console.log('Credit card validation request sent');
    console.log('====================================');

    return res.status(200).json({
      status: 'success',
      message: 'Credit card validation request sent',
    });
  } catch (err) {
    console.log(err);
  }
};
