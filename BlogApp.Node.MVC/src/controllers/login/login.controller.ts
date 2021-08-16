import { Get, Controller, Render, Query, Body, Post, HttpException, HttpStatus } from '@nestjs/common';
import { AuthService } from 'src/services/auth.service';
import { constants } from 'src/constants';
import { Client } from 'src/models/client';

@Controller()
export class LoginController {
  constructor(private authService: AuthService){}

  @Get('login')
  @Render('login')
  login(@Query() query: {callBackUrl: string}) {
    this.authService.client.callBackUrl = query.callBackUrl;
    return { 
      errorMessage: this.authService.error == '400' ?
      'The given email and password combination is incorrect.' :
      'The server is not responding.',
      hasError: this.authService.error != undefined,
    };
  }

  @Post('signin')
  async getClient(@Body() input: any): Promise<Client>{
    if (input.secret != constants.secret){
      throw new HttpException('', HttpStatus.UNAUTHORIZED);
    }
   let client = await this.authService.client;
   client.callBackUrl = client.callBackUrl == '/register' ? '/' : client.callBackUrl;
   return client;
  }
}