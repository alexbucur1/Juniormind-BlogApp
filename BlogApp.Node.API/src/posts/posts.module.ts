import { Module } from '@nestjs/common';
import { PostsController } from './posts.controller';
import { PostsService } from '../services/posts.service';
import { ImagesModule } from '../images/images.module';
import { AuthModule } from '../auth/auth.module';

@Module({
    controllers: [PostsController],
  providers: [
    PostsService],
    imports:[
      ImagesModule,
      AuthModule]
})
export class PostsModule {}
