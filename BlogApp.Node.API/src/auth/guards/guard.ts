import { Injectable, CanActivate, ExecutionContext, HttpException, HttpStatus } from '@nestjs/common';
import { AuthService } from '../../services/auth.service';

@Injectable()
export class Guard implements CanActivate {
    constructor(private authService: AuthService){}
  async canActivate(
    context: ExecutionContext,
  ): Promise<boolean> {
      let request = context.switchToHttp().getRequest();
      let token = request.headers['authorization'];
      let url: string = request.originalUrl;
      let method = request.method;
      if (method == 'GET' || url.includes('image')){
        return true;
      }

      let userInfo = await this.authService.receiveUserInfo(token);
      if (userInfo == undefined){
        throw new HttpException('', HttpStatus.UNAUTHORIZED);
      }

      return true;
    }
}