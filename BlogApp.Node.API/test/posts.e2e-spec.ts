import { Test, TestingModule } from '@nestjs/testing';
import { INestApplication, ValidationPipe } from '@nestjs/common';
import * as request from 'supertest';
import { AppModule } from '../src/app.module';
import { BlogPostDto } from '../src/models/dtos/blogpost.dto';
import { postsPageEx } from '../src/unit-test-utilities/postsPageExample';
import { blogPostDtoEx } from '../src/unit-test-utilities/blogPostDtoExample';
import { PostsService } from '../src/services/posts.service';
import { ImagesService } from '../src/services/images.service';
import { AuthService } from '../src/services/auth.service';
import { constants } from '../src/constants';

describe('PostController (e2e)', () => {
  let app: INestApplication;
  const validUserInfo = {
    id: "admin",
    isAdmin: true
  }
  let imagesService = { delete: () => {} };
  let postsService = {
    create: jest.fn((postDto: BlogPostDto) => postDto),
    getAll: jest.fn(() => postsPageEx),
    get: jest.fn(() => blogPostDtoEx),
    put: jest.fn(),
    delete: jest.fn(() => blogPostDtoEx),
   }
   let authService = {
     user: validUserInfo,

     isOwner: () => true,
     receiveUserInfo: () => {
      return validUserInfo
    },
   }

  beforeEach(async () => {
    const moduleFixture: TestingModule = await Test.createTestingModule({
      imports: [AppModule],
    }).overrideProvider(PostsService).useValue(postsService)
    .overrideProvider(ImagesService).useValue(imagesService)
    .overrideProvider(AuthService).useValue(authService)
    .overrideProvider(constants.comments_repository).useValue(jest.fn())
    .overrideProvider(constants.posts_repository).useValue(jest.fn())
    .overrideProvider(constants.users_repository).useValue(jest.fn())
    .overrideProvider(constants.db_connection).useValue(jest.fn())
    .compile();

    app = moduleFixture.createNestApplication();
    app.useGlobalPipes(new ValidationPipe())
    await app.init();
  });

  afterEach(() => {    
    jest.restoreAllMocks()
  });

  it('should get all posts', () => {
    return request(app.getHttpServer())
      .get('/posts')
      .expect(200)
      .expect(postsPageEx);
  });
  it('should get post with given id', () => {
    return request(app.getHttpServer())
      .get('/posts/1')
      .expect(200)
      .expect(blogPostDtoEx);
  });
  it('get post with given id returns 404 not found if no post is found', () => {
    jest.spyOn(postsService, 'get').mockImplementation(() => undefined);
    return request(app.getHttpServer())
      .get('/posts/1')
      .expect(404)
  });
  it('should create new post with given data', () => {
    jest.spyOn(postsService, 'create').mockImplementation(() => blogPostDtoEx);
    return request(app.getHttpServer())
    .post('/posts')
    .send(blogPostDtoEx)
    .expect(201)
  });
  it('should create new post without title return 400 Bad Request', () => {
    jest.spyOn(postsService, 'create').mockImplementation(() => blogPostDtoEx);
    return request(app.getHttpServer())
    .post('/posts')
    .send({
      id: 1,
      title: "",
      content: "This is a basic content.",
      userID: "user",
      owner: "User One Kenobi",
      createdAt: "",
      modifiedAt: "",
      imageURL: ""
    })
    .expect(400)
  });
  it('should create new post without content return 400 Bad Request', () => {
    jest.spyOn(postsService, 'create').mockImplementation(() => blogPostDtoEx);
    return request(app.getHttpServer())
    .post('/posts')
    .send({
      id: 1,
      title: "title",
      content: "",
      userID: "user",
      owner: "User One Kenobi",
      createdAt: "",
      modifiedAt: "",
      imageURL: ""
    })
    .expect(400)
  });
  it('should update post with given id', () => {
    jest.spyOn(postsService, 'get').mockImplementation(() => blogPostDtoEx);
    return request(app.getHttpServer())
    .put('/posts/1')
    .send(blogPostDtoEx)
    .expect(200)
  });
  it('should update post with given id returns 400 Bad Request if given id does not match with id from body', () => {
    jest.spyOn(postsService, 'get').mockImplementation(() => blogPostDtoEx);
    return request(app.getHttpServer())
    .put('/posts/1')
    .send({
      id: 2,
      title: "title",
      content: "",
      userID: "user",
      owner: "User One Kenobi",
      createdAt: "",
      modifiedAt: "",
      imageURL: ""
    })
    .expect(400)
  });

  it('should update post with given id returns 400 Bad Request if given dto has empty title', () => {
    jest.spyOn(postsService, 'get').mockImplementation(() => blogPostDtoEx);
    return request(app.getHttpServer())
    .put('/posts/1')
    .send({
      id: 1,
      title: "",
      content: "This is a content.",
      userID: "user",
      owner: "User One Kenobi",
      createdAt: "",
      modifiedAt: "",
      imageURL: ""
    })
    .expect(400)
  });
  it('should update post with given id returns 400 Bad Request if given dto has empty content', () => {
    jest.spyOn(postsService, 'get').mockImplementation(() => blogPostDtoEx);
    return request(app.getHttpServer())
    .put('/posts/1')
    .send({
      id: 1,
      title: "title",
      content: "",
      userID: "user",
      owner: "User One Kenobi",
      createdAt: "",
      modifiedAt: "",
      imageURL: ""
    })
    .expect(400)
  });
  it('should update post with given id returns 404 not found if no post was found with given id', () => {
    jest.spyOn(postsService, 'get').mockImplementation(() => undefined);
    return request(app.getHttpServer())
    .put('/posts/1')
    .send(blogPostDtoEx)
    .expect(404)
  });
  it('should update post with given id returns 401 Unauthorized if does not have good token', () => {
    jest.spyOn(authService, 'receiveUserInfo').mockImplementation(() => undefined);
    return request(app.getHttpServer())
    .put('/posts/1')
    .send(blogPostDtoEx)
    .expect(401)
  });
  it('should update post with given id returns 403 Forbidden if user is not owner', () => {
    jest.spyOn(postsService, 'get').mockImplementation(() => blogPostDtoEx);
    jest.spyOn(authService, 'isOwner').mockImplementation(() => false);
    authService.user = {
      id: 'testuser2',
      isAdmin: false
    };
    return request(app.getHttpServer())
    .put('/posts/1')
    .send(blogPostDtoEx)
    .expect(403)
  });
  it('should delete post with given id', () => {
    return request(app.getHttpServer())
    .delete('/posts/1')
    .expect(200)
  });
  it('should delete post returns 404 not found if no post was found with given id', () => {
    jest.spyOn(postsService, 'delete').mockImplementation(() => undefined);
    return request(app.getHttpServer())
    .delete('/posts/1')
    .expect(404)
  });
  it('should delete post returns 401 unauthorized if no token was given', () => {
    jest.spyOn(authService, 'receiveUserInfo').mockImplementation(() => undefined);
    return request(app.getHttpServer())
    .delete('/posts/1')
    .expect(401)
  });
  it('should delete post returns 403 forbidden if user is not owner', () => {
    jest.spyOn(postsService, 'delete').mockImplementation(() => blogPostDtoEx);
    jest.spyOn(authService, 'isOwner').mockImplementation(() => false);
    return request(app.getHttpServer())
    .delete('/posts/1')
    .expect(403)
  });
});
