import { Connection, Repository } from 'typeorm';
import { Comment } from '../../models/entities/comment.entity';
import { constants } from '../../constants';

export const commentProviders = [
  {
    provide: constants.comments_repository,
    useFactory: (connection: Connection) => connection.getRepository(Comment),
    inject: [constants.db_connection],
  },
];