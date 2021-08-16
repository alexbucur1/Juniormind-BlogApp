import { Module } from '@nestjs/common';
import { LoginController } from './controllers/login/login.controller';
import { AppService } from './app.service';
import { ServeStaticModule } from '@nestjs/serve-static';
import { join } from 'path';
import { CallbackController } from './controllers/callback/callback.controller';
import { AuthService } from './services/auth.service';
import { HttpModule } from '@nestjs/axios';

@Module({
  imports: [
    ServeStaticModule.forRoot({
      rootPath: join(__dirname, '..', 'views'),
      serveRoot: "login.hbs"
    }),
    HttpModule
  ],
  controllers: [LoginController, CallbackController],
  providers: [
    AppService,
     AuthService],
})
export class AppModule {}
