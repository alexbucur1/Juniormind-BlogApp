import { Injectable } from '@angular/core';
import { User } from '../models/user.model';
import { Page } from '../models/page.model';
import { environment } from 'src/environments/environment';
import { HttpRequestsService } from './http-requests.service';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  baseUrl = environment.url_auth;
  constructor(private httpRequestsService: HttpRequestsService) {
   }

   public getAll(sortOrder: string = '',page: number = 1, search: string = ''): Promise<Page<User>> {
    const url = `${this.baseUrl}/users?sortOrder=${sortOrder}&pageNumber=${page}&searchString=${search}`;
    return this.httpRequestsService.get(url);
  }

  public get(userId: String): Promise<User> {
    const url = `${this.baseUrl}/users/${userId}`;
    return this.httpRequestsService.get(url);
  }

   public post(user: User): Promise<any> {
    const url = `${this.baseUrl}/users/register`;
    return this.httpRequestsService.post(url, user);
  }

  public put(user: User): Promise<User>{
    const url = `${this.baseUrl}/users/${user.id}`;
    return this.httpRequestsService.put(url, user);
  }

  public delete(id: string): Promise<User>{
    const url = `${this.baseUrl}/users/${id}`;
    return this.httpRequestsService.delete(url);
  }
}
