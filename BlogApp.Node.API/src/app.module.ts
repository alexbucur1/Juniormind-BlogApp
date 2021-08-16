import {  Module } from '@nestjs/common';
import { PostsModule } from './posts/posts.module';
import { CommentsModule } from './comments/comments.module';
import { DatabaseModule } from './database/database.module';
import { AuthModule } from './auth/auth.module';
import { ImagesModule } from './images/images.module';

@Module({
  imports: [
    PostsModule,
    CommentsModule,
    DatabaseModule,
    AuthModule,
    ImagesModule
]
})
export class AppModule {}
