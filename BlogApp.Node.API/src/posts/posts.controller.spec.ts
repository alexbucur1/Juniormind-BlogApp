import { Test, TestingModule } from '@nestjs/testing';
import { PostsController } from './posts.controller';
import { PostsService } from '../services/posts.service';
import { ImagesService } from '../services/images.service';
import { BlogPostDto } from '../models/dtos/blogpost.dto';
import { HttpException, HttpStatus } from '@nestjs/common';
import { blogPostDtoEx } from '../unit-test-utilities/blogPostDtoExample';
import { postsPageEx } from '../unit-test-utilities/postsPageExample';
import { AuthService } from '../services/auth.service';

describe('PostsController', () => {
  let controller: PostsController;
  let imagesService = { delete: () => {} };
  let postsService = {
     create: jest.fn((postDto: BlogPostDto) => postDto),
     getAll: jest.fn(() => postsPageEx),
     get: jest.fn(() => blogPostDtoEx),
     put: jest.fn(),
     delete: jest.fn(),
    }

    let authService = {
      receiveUserInfo: () => {},
      isOwner: () => {return true;},
      user: {
        id: "admin",
        isAdmin: true
      }
    }

  beforeEach(async () => {
    const module: TestingModule = await Test.createTestingModule({
      controllers: [PostsController],
      providers: [PostsService, ImagesService, AuthService]
    }).overrideProvider(PostsService).useValue(postsService)
    .overrideProvider(ImagesService).useValue(imagesService)
    .overrideProvider(AuthService).useValue(authService)
    .compile();

    controller = module.get<PostsController>(PostsController);
  });

  it('should be defined', () => {
    expect(controller).toBeDefined();
  });
  it('should create post with given data', async () => {
    await controller.create(blogPostDtoEx);
    expect(jest.spyOn(postsService, 'create')).toBeCalled();
  });
  it('create method should throw exception if posts service returns undefined', async () => {
    jest.spyOn(postsService, 'create').mockImplementation(() => undefined);
    try{
      await controller.create(blogPostDtoEx);
    }catch(error){
      expect(error).toStrictEqual(new HttpException('', HttpStatus.BAD_REQUEST));
    }
  });
  it('should get paginated posts', async () => {
    expect(await controller.getAll({
      search: "",
      page: 1
    })).toBe(postsPageEx);
  });
  it('should get post with given id', async () => {
    expect(await controller.findOne(1)).toBe(blogPostDtoEx);
  });
  it('findOne should throw exception if post is not found', async () => {
    jest.spyOn(postsService, 'get').mockImplementation(() => undefined)
    try{
      await controller.findOne(1);
    }catch(error){
      expect(error).toStrictEqual(new HttpException('No post was found with the given id.', HttpStatus.BAD_REQUEST));
    }
  });
  it('should edit post with given id', async () => {
    jest.spyOn(postsService, 'get').mockImplementation(() => blogPostDtoEx)
    await controller.update(1, blogPostDtoEx);
    expect(jest.spyOn(postsService, 'put')).toBeCalled();
  });
  it('update should throw exception if post with given id is not found', async () => {
    jest.spyOn(postsService, 'get').mockImplementation(() => undefined)
    try{
      await controller.update(1, blogPostDtoEx)
    }catch(error){
      expect(error).toStrictEqual(new HttpException('There is no post with the given id.', HttpStatus.NOT_FOUND));
    }
  });
  it('should delete post with given id', async () => {
    jest.spyOn(postsService, 'delete').mockImplementation(() => blogPostDtoEx)
    await controller.remove(1);
    expect(jest.spyOn(postsService, 'delete')).toBeCalled();
  });
  it('remove should throw exception if post with given id is not found', async () => {
    jest.spyOn(postsService, 'delete').mockImplementation(() => undefined)
    try{
      await controller.remove(1)
    }catch(error){
      expect(error).toStrictEqual(new HttpException('No post was found with the given id.', HttpStatus.NOT_FOUND));
    }
  });
});
