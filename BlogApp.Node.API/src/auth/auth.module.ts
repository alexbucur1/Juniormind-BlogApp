import { Module } from '@nestjs/common';
import { AuthService } from '../services/auth.service';
import { Guard } from './guards/guard';
import { APP_GUARD } from '@nestjs/core';
import { HttpModule } from '@nestjs/axios';
import { constants } from '../constants';

@Module({
    imports:[
        HttpModule.register({
            baseURL: constants.url_auth
          }),
    ],
    providers:[
        AuthService,
        {
            provide: APP_GUARD,
            useClass: Guard,
          }
    ],
    exports:[AuthService]
})
export class AuthModule {}
