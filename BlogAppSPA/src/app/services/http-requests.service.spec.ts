import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { HttpClient } from '@angular/common/http';

import { HttpRequestsService } from './http-requests.service';
import { AuthService } from 'src/app/services/auth.service';
import { HttpErrorService } from './http-error.service';

describe('HttpRequestsService', () => {
  let service: HttpRequestsService;
  let authServiceSpy: jasmine.SpyObj<AuthService>;
  let httpErrorServiceSpy: jasmine.SpyObj<HttpErrorService>;
  let httpClient: HttpClient;
  let httpTestingController: HttpTestingController;

  beforeEach(() => {
    const authSpy = jasmine.createSpyObj('AuthService', ['userClient']);
    const errorSpy = jasmine.createSpyObj('HttpErrorService', ['clearErrorState']);
    TestBed.configureTestingModule({
      imports: [ HttpClientTestingModule ],
      providers: [
        HttpRequestsService,
        { provide: AuthService, useValue: authSpy },
        { provide: HttpErrorService, useValue: errorSpy }
      ]
    });
    service = TestBed.inject(HttpRequestsService);
    authServiceSpy = TestBed.inject(AuthService) as jasmine.SpyObj<AuthService>;
    httpErrorServiceSpy = TestBed.inject(HttpErrorService) as jasmine.SpyObj<HttpErrorService>;
    httpClient = TestBed.inject(HttpClient);
    httpTestingController = TestBed.inject(HttpTestingController);
  }); 

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
