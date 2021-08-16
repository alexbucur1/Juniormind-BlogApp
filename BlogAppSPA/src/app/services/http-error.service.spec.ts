import { TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';

import { HttpErrorService } from '../services/http-error.service';
import { AuthService } from "./auth.service";

describe('HttpErrorServiceService', () => {
  let service: HttpErrorService;
  let authServiceSpy: jasmine.SpyObj<AuthService>;

  beforeEach(() => {
    const authSpy = jasmine.createSpyObj('AuthService', ['logout']);
    TestBed.configureTestingModule({
      imports: [
        RouterTestingModule
      ],
      providers: [
        HttpErrorService,
        { provide: AuthService, useValue: authSpy },
      ]
    });
    service = TestBed.inject(HttpErrorService);
    authServiceSpy = TestBed.inject(AuthService) as jasmine.SpyObj<AuthService>;
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
