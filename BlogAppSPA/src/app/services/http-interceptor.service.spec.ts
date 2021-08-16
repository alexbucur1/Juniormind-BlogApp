import { TestBed } from '@angular/core/testing';

import { HttpErrorInterceptor } from '../services/http-interceptor.service';
import { HttpErrorService } from './http-error.service';

describe('HttpInterceptorService', () => {
  let service: HttpErrorInterceptor;
  let httpErrorServiceSpy: jasmine.SpyObj<HttpErrorService>;

  beforeEach(() => {
    const errorSpy = jasmine.createSpyObj('HttpErrorService', ['manageError']);
    TestBed.configureTestingModule({
      providers: [
        HttpErrorInterceptor,
        { provide: HttpErrorService, useValue: errorSpy }
      ]
    });
    service = TestBed.inject(HttpErrorInterceptor);
    httpErrorServiceSpy = TestBed.inject(HttpErrorService) as jasmine.SpyObj<HttpErrorService>;
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
