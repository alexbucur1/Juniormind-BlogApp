import {
  HttpHandler,
  HttpRequest,
  HttpEvent,
  HttpErrorResponse,
  HttpInterceptor
} from "@angular/common/http";
import { Observable, throwError } from "rxjs";
import { catchError, finalize } from "rxjs/operators";
import { Injectable } from "@angular/core";
import { HttpErrorService } from "src/app/services/http-error.service";

@Injectable()
export class HttpErrorInterceptor implements HttpInterceptor {

  constructor(private errorService: HttpErrorService) {}

  intercept(
    request: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    return next.handle(request).pipe(
      catchError((error: HttpErrorResponse) => {
          this.errorService.manageError(error.message ?? JSON.stringify(error), error.status, request.method);
          return throwError(error);
      }),
      finalize(() => {
      })
    ) as Observable<HttpEvent<any>>;
  }
}