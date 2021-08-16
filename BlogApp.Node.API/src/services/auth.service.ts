import { Injectable } from '@nestjs/common';
import { HttpService } from '@nestjs/axios';
import { UserInfo } from '../models/dtos/userInfo.dto';
import { firstValueFrom } from 'rxjs';

@Injectable()
export class AuthService {
     user: UserInfo;
    constructor(private httpService: HttpService){
    }

    public async receiveUserInfo(token: string): Promise<UserInfo>{
        const url = '/connect/userinfo';
        const options = {
            headers:{
            'Content-Type': 'application/json',
            'Authorization': `${token}`,
            }
        };

        let axiosResponse =  await firstValueFrom(this.httpService.get(url, options));
        this.user = {
            id: axiosResponse.data.id,
            isAdmin: axiosResponse.data.role[0] == 'Administrator',
        };
        return this.user;
        }

        public isOwner(userID: string): boolean{
            return this.user.isAdmin || this.user.id == userID;
        }
}
