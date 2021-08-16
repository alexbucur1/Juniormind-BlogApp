import { Injectable } from '@nestjs/common';
import { constants } from '../constants';
import { HttpService } from '@nestjs/axios';
import { Client } from '../models/client';
import { firstValueFrom } from 'rxjs';

@Injectable()
export class AuthService {
    client: Client;
    error: string;
    constructor(private httpService: HttpService){
            this.client = {
                token: "",
                fullName: "",
                password: "",
                email: "",
                clientInfoQuery: "",
                callBackUrl: ""
            }
        }
    clientQuery = `client_id=${constants.client_id}&redirect_uri=${constants.redirect_uri}&response_type=${constants.response_type}&scope=${constants.scope}&state=${constants.state}&nonce=${constants.nonce}&response_mode=${constants.response_mode}`;
    public async setClient(client: Client): Promise<boolean>{
        this.error = undefined;
        const url = constants.url_auth + '/account/signin';
        const options = {headers:{'Content-Type': 'application/json'}};
        client.clientInfoQuery = this.clientQuery;
        try{
            let authResponse =  await firstValueFrom(this.httpService.post<Client>(url, client, options));
            if (authResponse.data == undefined){
                return false;
            }
            this.client.token = authResponse.data.token;
            this.client.fullName = authResponse.data.fullName;
            return true;
        }catch(error){
            this.error = error.response == undefined ?
            'server not responding' :
             error.response.status;
        }
    }
}
