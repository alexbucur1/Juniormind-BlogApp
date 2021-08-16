import { TestBed } from '@angular/core/testing';

import { PostService } from './post.service';
import { HttpRequestsService } from './http-requests.service';

describe('PostService', () => {
  let service: PostService;
  let httpRequestsServiceSpy: jasmine.SpyObj<HttpRequestsService>;

  beforeEach(() => {
    const requestsServiceSpy = jasmine.createSpyObj('HttpRequestsService', ['get', 'post', 'put', 'delete', 'getAll']);
    TestBed.configureTestingModule({
      providers:[
        { provide: HttpRequestsService, useValue: requestsServiceSpy },
      ]
    });
    service = TestBed.inject(PostService);
    httpRequestsServiceSpy = TestBed.inject(HttpRequestsService) as jasmine.SpyObj<HttpRequestsService>;
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
