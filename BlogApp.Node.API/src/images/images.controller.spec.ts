import { Test, TestingModule } from '@nestjs/testing';
import { ImagesController } from './images.controller';
import { ImagesService } from '../services/images.service';
import { PostsService } from '../services/posts.service';
import { PostsModule } from '../posts/posts.module';

describe('ImagesController', () => {
  let controller: ImagesController;
  let imageService = { delete: () => {} };
  let postsService = {
    create: () => {},
    getAll: () => {},
    get: () => {},
    put: () => {},
    delete: () => {},
   }

  beforeEach(async () => {
    const module: TestingModule = await Test.createTestingModule({
      controllers: [ImagesController],
      providers:[
        ImagesService,
        PostsService
      ],
      imports: [PostsModule]
    }).overrideProvider(ImagesService).useValue(imageService)
    .overrideProvider(PostsService).useValue(postsService)
    .compile();

    controller = module.get<ImagesController>(ImagesController);
  });

  it('should be defined', () => {
    expect(controller).toBeDefined();
  });
});
