import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { AuthService } from 'src/app/services/auth.service';
import { catchError } from 'rxjs/operators';
import { HttpErrorService } from './http-error.service';

@Injectable({
  providedIn: 'root'
})
export class HttpRequestsService {

  constructor(private client: HttpClient,
     private authService: AuthService,
     private errorService: HttpErrorService) { }

  public get(url: string): Promise<any> {
    this.errorService.clearErrorState();
    let options = {headers: this.getHeaders()};
    return this.client.get<any>(url, options).pipe(catchError(this.handleError<any>('getAll'))).toPromise();
  }

  public post(url: string, body: any): Promise<any> {
    this.errorService.clearErrorState();
    let options = {headers: this.getHeaders()};
    return this.client.post<any>(url, body, options).pipe(catchError(this.handleError<any>('post'))).toPromise();
  }

  public put(url: string, body: any): Promise<any> {
    this.errorService.clearErrorState();
    let options = {headers: this.getHeaders()};
    return this.client.put<any>(url, body, options).pipe(catchError(this.handleError<any>('put'))).toPromise();
  }

  public delete(url: string): Promise<any> {
    this.errorService.clearErrorState();
    let options = {headers: this.getHeaders()};
    return this.client.delete(url, options).pipe(catchError(this.handleError('delete'))).toPromise();
  }

  private handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {
      console.error(error);

      return of(result as T);
    };
  }

  private getHeaders(): HttpHeaders{
    return new HttpHeaders(
      {'Content-Type': 'application/json',
    'Authorization': `Bearer ${this.authService.userClient.token}`});
  }
}
