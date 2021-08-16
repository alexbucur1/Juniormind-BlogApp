import { TestBed } from '@angular/core/testing';

import { UserService } from './user.service';
import { HttpRequestsService } from './http-requests.service';
import { User } from '../models/user.model';
import { Page } from '../models/page.model';

describe('UserService', () => {
  let service: UserService;
  let httpRequestsServiceSpy: jasmine.SpyObj<HttpRequestsService>;

  beforeEach(() => {
    const requestsServiceSpy = jasmine.createSpyObj('HttpRequestsService', ['get', 'post', 'put', 'delete']);
    TestBed.configureTestingModule({
      providers:[
        { provide: HttpRequestsService, useValue: requestsServiceSpy },
      ]
    });
    service = TestBed.inject(UserService);
    httpRequestsServiceSpy = TestBed.inject(HttpRequestsService) as jasmine.SpyObj<HttpRequestsService>;
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
  it('getAll is working', async () => {
    const users = [
      {
        id: '1',
        firstName: 'user1',
        lastName: 'user1',
        email:'user1@mail.com',
        password: 'User111-'
      },
      {
        id: '2',
        firstName: 'user2',
        lastName: 'user2',
        email:'user2@mail.com',
        password: 'User222-'
      },
      {
        id: '3',
        firstName: 'user3',
        lastName: 'user3',
        email:'user3@mail.com',
        password: 'User333-'
      }
    ];

    const page = {
      hasNextPage: false,
      hasPreviousPage: false,
      pageIndex: 1,
      items: users,
      pageSize: 5,
    };

    const pageAsPromise = new Promise<Page<User>>((resolve) => {
      resolve(page);
    });

    httpRequestsServiceSpy.get.and.returnValue(pageAsPromise);
    expect(await service.getAll()).toBe(page);
    expect(httpRequestsServiceSpy.get.calls.count()).toBe(1);
  });
  it('get is working', async () => {
    const user =  {
      id: '1',
      firstName: 'user1',
      lastName: 'user1',
      email:'user1@mail.com',
      password: 'User111-'
    };

    const userAspromise = new Promise<User>((resolve) => {resolve(user)});
    httpRequestsServiceSpy.get.and.returnValue(userAspromise);
    expect(await service.get('user1')).toBe(user);
    expect(httpRequestsServiceSpy.get.calls.count()).toBe(1);
  });
  it('post is working', async() => {
    const user =  {
      id: '1',
      firstName: 'user1',
      lastName: 'user1',
      email:'user1@mail.com',
      password: 'User111-'
    };
    const userAspromise = new Promise<User>((resolve) => {resolve(user)});
    httpRequestsServiceSpy.post.and.returnValue(userAspromise);
    expect(await service.post(user)).toBe(user);
    expect(httpRequestsServiceSpy.post.calls.count()).toBe(1);
  });
  it('put is working', async() => {
    const user =  {
      id: '1',
      firstName: 'user1',
      lastName: 'user1',
      email:'user1@mail.com',
      password: 'User111-'
    };
    const userAspromise = new Promise<User>((resolve) => {resolve(user)});
    httpRequestsServiceSpy.put.and.returnValue(userAspromise);
    expect(await service.put(user)).toBe(user);
    expect(httpRequestsServiceSpy.put.calls.count()).toBe(1);
  });
  it('delete is working', async() => {
    const user =  {
      id: '1',
      firstName: 'user1',
      lastName: 'user1',
      email:'user1@mail.com',
      password: 'User111-'
    };
    const userAspromise = new Promise<User>((resolve) => {resolve(user)});
    httpRequestsServiceSpy.delete.and.returnValue(userAspromise);
    expect(await service.delete('user1')).toBe(user);
    expect(httpRequestsServiceSpy.delete.calls.count()).toBe(1);
  });
});
