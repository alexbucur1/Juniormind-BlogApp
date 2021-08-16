import { Injectable } from '@angular/core';
import { HttpRequestsService } from './http-requests.service';
import { environment } from 'src/environments/environment';
import { Page } from '../models/page.model';
import { Post } from '../models/post.model';

@Injectable({
  providedIn: 'root',
})
export class PostService {
  baseUrl = environment.url_node;

  constructor(private httpRequestsService: HttpRequestsService) {
  }

  public getAll(page: number = 1, search: string = ''): Promise<Page<Post>> {
    const url = `${this.baseUrl}/api/posts?page=${page}&search=${search}`;
    return this.httpRequestsService.get(url);
  }

  public get(id: string): Promise<Post> {
     const url =`${this.baseUrl}/api/posts/${id}`;
    return this.httpRequestsService.get(url);
  }

  public post(post: Post): Promise<Post> {
    const url = `${this.baseUrl}/api/posts`;
    return this.httpRequestsService.post(url, post);
  }

  public put(id: string, post: Post): Promise<unknown> {
    const url = `${this.baseUrl}/api/posts/${id}`;
    return this.httpRequestsService.put(url, post);
  }

  public delete(id: string): Promise<any> {
    const url = `${this.baseUrl}/api/posts/${id}`;
    return this.httpRequestsService.delete(url);
  }
}
