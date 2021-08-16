import { Module, Global } from '@nestjs/common';
import { databaseProviders } from './providers/database.providers';
import { postProviders } from './providers/post.providers';
import { commentProviders } from './providers/comment.providers';
import { usersProviders } from './providers/users.providers';

@Global()
@Module({
  providers: [
    ...databaseProviders,
    ...commentProviders,
    ...postProviders,
    ...usersProviders],
  exports: [...databaseProviders,
            ...commentProviders,
            ...postProviders,
            ...usersProviders],
})
export class DatabaseModule {}