import { Test, TestingModule } from '@nestjs/testing';
import { CommentsService } from './comments.service';
import { constants } from '../constants';

describe('CommentsService', () => {
  let service: CommentsService;
  let postsRepository = {};
  let usersRepository = {};
  let commentsRepository = {};

  beforeEach(async () => {
    const module: TestingModule = await Test.createTestingModule({
      providers: [
        CommentsService,
        {
          provide: constants.posts_repository,
          useValue: postsRepository
        },
        {
          provide: constants.comments_repository,
          useValue: commentsRepository
        },
        {
          provide: constants.users_repository,
          useValue: usersRepository
        }
      ],
    }).compile();

    service = module.get<CommentsService>(CommentsService);
  });

  it('should be defined', () => {
    expect(service).toBeDefined();
  });
});


