import { Test, TestingModule } from '@nestjs/testing';
import { PostsService } from './posts.service';
import { constants } from '../constants';
import { User } from '../models/entities/user.entity';
import { BlogPost } from '../models/entities/blogpost.entity';
import { Repository } from 'typeorm';
import { blogPostEx } from '../unit-test-utilities/blogPostExample';
import { blogPostDtoEx} from '../unit-test-utilities/blogPostDtoExample';
import { postsPageEx } from '../unit-test-utilities/postsPageExample';
import { postsArrayEx } from '../unit-test-utilities/postsArrayExample'

describe('PostsService', () => {
  let service: PostsService;
  let postsRepo: Repository<BlogPost>;
  let usersRepo: Repository<User>;
  let postsRepositoryMock = {
    save: jest.fn((postModel: BlogPost) => {return postModel}),
    findOne: jest.fn(() => {return blogPostEx}),
    remove: jest.fn(),
    createQueryBuilder: jest.fn(() => ({
      leftJoinAndSelect: jest.fn().mockReturnThis(),
      where: jest.fn().mockReturnThis(),
      orWhere: jest.fn().mockReturnThis(),
      orderBy: jest.fn().mockReturnThis(),
      skip: jest.fn().mockReturnThis(),
      take: jest.fn().mockReturnThis(),
      getMany: jest.fn().mockReturnValue(Promise.resolve(postsArrayEx))
    }))
  };
  let usersRepositoryMock = {
    findOne: (id: string) => new User(),
  };

  beforeEach(async () => {
    const module: TestingModule = await Test.createTestingModule({
      providers: [
        PostsService,
        {
          provide: constants.posts_repository,
          useValue: postsRepositoryMock
        },
        {
          provide: constants.users_repository,
          useValue: usersRepositoryMock
        }],
    }).compile();

    service = module.get<PostsService>(PostsService);
    postsRepo = module.get<Repository<BlogPost>>(constants.posts_repository);
    usersRepo = module.get<Repository<User>>(constants.users_repository);
  });

  it('should be defined', () => {
    expect(service).toBeDefined();
  });
  it('should save post if given user is found', async () => {
    expect(await service.create(blogPostDtoEx)).toBeDefined();
  });
  it('should not save post if given user is not found', async () => {
    jest.spyOn(usersRepo, 'findOne').mockImplementation(() => undefined);
    expect(await service.create(blogPostDtoEx)).toBeUndefined();
  });
  it('should get page of posts', async () => {
    expect(await service.getAll()).toEqual(postsPageEx);
  });
  it('should get one post', async () => {
    expect(await service.get(1)).toEqual(blogPostDtoEx);
  });
  it('should edit post', async () => {
    await service.put(blogPostDtoEx);
    expect(jest.spyOn(postsRepo, 'save')).toBeCalled();
  });
  it('should delete post found by given id', async () => {
    expect(await service.delete(1)).toEqual(blogPostDtoEx);
  });
  it('should return undefined when tyring to delete a post which is not found', async () => {
    jest.spyOn(postsRepo, 'findOne').mockImplementation(() => undefined);
    expect(await service.delete(1)).toBeUndefined();
  })
});
