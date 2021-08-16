import { ValidationPipe } from '@nestjs/common';
import { NestFactory } from '@nestjs/core';
import { AppModule } from './app.module';
import * as fs from 'fs';

async function bootstrap() {
  const app = await NestFactory.create(AppModule);
  app.enableCors({
    origin: "http://localhost:4200"
  })
  app.useGlobalPipes(new ValidationPipe());
  app.setGlobalPrefix('api')
  await app.listen(3000);
}
bootstrap();
