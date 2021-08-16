import { TestBed } from '@angular/core/testing';

import { CommentService } from './comment.service';
import { HttpRequestsService } from './http-requests.service';
import { HttpErrorService } from './http-error.service';

describe('CommentService', () => {
  let service: CommentService;
  let httpRequestsServiceSpy: jasmine.SpyObj<HttpRequestsService>;

  beforeEach(() => {
    const requestsServiceSpy = jasmine.createSpyObj('HttpRequestsService', ['get', 'post', 'put', 'delete']);
    const errorServiceSpy = jasmine.createSpyObj('HttpErrorService', ['errorType'])
    TestBed.configureTestingModule({
      providers:[
        { provide: HttpRequestsService, useValue: requestsServiceSpy },
        { provide: HttpErrorService, useValue: errorServiceSpy },
      ]
    });
    service = TestBed.inject(CommentService);
    httpRequestsServiceSpy = TestBed.inject(HttpRequestsService) as jasmine.SpyObj<HttpRequestsService>;
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
