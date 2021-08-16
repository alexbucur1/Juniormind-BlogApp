import { Connection, Repository } from 'typeorm';
import { BlogPost } from '../../models/entities/blogpost.entity';
import { constants } from '../../constants';

export const postProviders = [
  {
    provide: constants.posts_repository,
    useFactory: (connection: Connection) => connection.getRepository(BlogPost),
    inject: [constants.db_connection],
  },
];