import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Image } from '../models/image.model';

@Injectable({
  providedIn: 'root',
})
export class ImageService {
  baseUrl = environment.url_node;

  constructor(private client: HttpClient) {}

  public put(id: number, form: any): Promise<unknown> {
    const url = `${this.baseUrl}/api/image/${id}`;
    return this.client.put<Image>(url, form).pipe(catchError(this.handleError<Image>('put'))).toPromise();
  }

  private handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {
      console.error(error);

      return of(result as T);
    };
  }
}
