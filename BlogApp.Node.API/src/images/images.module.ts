import { Module } from '@nestjs/common';
import { ImagesController } from './images.controller';
import { ImagesService } from '../services/images.service';
import { PostsService } from '../services/posts.service';
import { MulterModule } from '@nestjs/platform-express';

@Module({
  controllers: [ImagesController],
  providers: [
    ImagesService,
    PostsService
  ],
  imports:[
    MulterModule.register({
    dest: "../static/Assets/Uploads"
  })],
  exports:[ImagesService]
})
export class ImagesModule {}
