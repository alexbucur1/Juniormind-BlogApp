import { EventEmitter, Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Page } from '../models/page.model';
import { Comment } from '../models/comment.model';
import { HttpRequestsService } from './http-requests.service';
import { HttpErrorService } from './http-error.service';

@Injectable({
  providedIn: 'root',
})
export class CommentService {
  baseUrl = environment.url_node;

  update = new EventEmitter();

  constructor(private httpRequestsService: HttpRequestsService, private errorService: HttpErrorService) { }

  public getAll(postID: number, page: number = 1, search: string = '') : Promise<Page<Comment>> {
    const url = `${this.baseUrl}/api/comments?postid=${postID}&page=${page}&search=${search}`;
    return this.httpRequestsService.get(url);
  }

  public async post(comment: Comment): Promise<unknown> {
    const url = `${this.baseUrl}/api/comments`;
    let response = await this.httpRequestsService.post(url, comment);
    this.emitUpdate();
    return response;
  }

  public async put(comment: Comment): Promise<unknown> {
    const url = `${this.baseUrl}/api/comments/${comment.id}`;
    let response = await this.httpRequestsService.put(url, comment);
    this.emitUpdate();
    return response;
  }

  public getReplies(commId: number, postId: string, pageIndex = 1) : Promise<Page<Comment>> {
    const url = `${this.baseUrl}/api/comments/${commId}?postID=${postId}&page=${pageIndex}`;
    return this.httpRequestsService.get(url);
  }

  public async delete(commId: number) : Promise<unknown> {
    const url = `${this.baseUrl}/api/comments/${commId}`;
    let response = await this.httpRequestsService.delete(url);
    this.emitUpdate();
    return response;
  }

  private emitUpdate(){
    if (this.errorService.errorType == ''){
      this.update.emit();
    }
  }
}
