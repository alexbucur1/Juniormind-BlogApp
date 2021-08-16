import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Client } from '../models/client.model';
import { UserInfo } from '../models/userInfo.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  userClient: Client;
  baseUrl = environment.url_auth;
  constructor(private client: HttpClient) {
    this.userClient = {
      callBackUrl: '',
      token: this.getFromStorage('token'),
      userID: this.getFromStorage('userId'),
      userIsAdmin: this.getFromStorage('userIsAdmin') === "true",
      fullName: this.getFromStorage('fullName')
    };
   }

  public async signin(): Promise<boolean>{
    var client = await this.getClientDetails();
    var userInfo = await this.getUserInfo(client.token)
    if (client == undefined || userInfo == undefined){
      return false;
    }
    this.setDetails(client, userInfo);
    this.setDetailsOnLocalStorage(client, userInfo);
    return true;
  }

  public logout(){
    this.userClient = {
      callBackUrl: '',
      token: '',
      userID: '',
      userIsAdmin: false,
      fullName: ''
    };
    localStorage.clear();
  }
  
  private setDetails(client: Client, userInfo: UserInfo){
    this.userClient = {
      callBackUrl: client.callBackUrl,
      token: client.token,
      userID: userInfo.id,
      userIsAdmin: userInfo.role[0] == 'Administrator',
      fullName: client.fullName
    };
  }

  private setDetailsOnLocalStorage(client: Client, userInfo: UserInfo){
    localStorage.clear();
    localStorage.setItem("token", client.token);
    localStorage.setItem("userId", userInfo.id);
    localStorage.setItem("userIsAdmin", (userInfo.role[0] == 'Administrator').toString());
    localStorage.setItem("fullName", client.fullName);
  }

  private async getUserInfo(token: string): Promise<UserInfo>{
    const url = `${this.baseUrl}/connect/userinfo`;
    let options = {
      headers: new HttpHeaders(
        {'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`})
    };
    return this.client.get<UserInfo>(url, options).toPromise();
  }

  private async getClientDetails(): Promise<Client>{
    const url = environment.url_login + '/signin';
    const secret = {
      secret: environment.auth_secret
    }
    let client = await this.client.post<Client>(url, secret).toPromise();
    return client;
  }

  private getFromStorage(key: string): any{
    let value = localStorage.getItem(key);
    return value === null ? '' : value;
  }
}
