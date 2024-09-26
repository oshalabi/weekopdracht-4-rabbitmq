import express from 'express';
import { regCtrl } from '../controllers/regCtrl';

const router = express.Router();
router.post('/register', regCtrl);

export { router as registrRouter };
