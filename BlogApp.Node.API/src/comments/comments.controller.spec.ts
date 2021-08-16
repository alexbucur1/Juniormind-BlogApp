import { Test, TestingModule } from '@nestjs/testing';
import { CommentsController } from './comments.controller';
import { CommentsService } from '../services/comments.service';
import { AuthService } from '../services/auth.service';

describe('CommentsController', () => {
  let controller: CommentsController;
  let commentsService = {
    create: () => {},
    getTopComments: () => {},
    getReplies: () => {},
    put: () => {},
    delete: () => {},
   }

   let authService = {
     receiveUserInfo: () => {},
     isOwner: () => {return true;}
   }

  beforeEach(async () => {
    const module: TestingModule = await Test.createTestingModule({
      controllers: [CommentsController],
      providers: [CommentsService, AuthService]
    }).overrideProvider(CommentsService).useValue(commentsService)
    .overrideProvider(AuthService).useValue(authService)
    .compile();

    controller = module.get<CommentsController>(CommentsController);
  });

  it('should be defined', () => {
    expect(controller).toBeDefined();
  });
});
