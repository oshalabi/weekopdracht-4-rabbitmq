import express from 'express';
import { registrRouter } from './routes/registation';
import { json, urlencoded } from 'body-parser';

const app = express();

app.use(json());
app.use(urlencoded({ extended: true }));
app.use(registrRouter);

export { app };
