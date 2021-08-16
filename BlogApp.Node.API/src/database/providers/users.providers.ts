import { Connection, Repository } from 'typeorm';
import { User } from '../../models/entities/user.entity';
import { constants } from '../../constants';

export const usersProviders = [
  {
    provide: constants.users_repository,
    useFactory: (connection: Connection) => connection.getRepository(User),
    inject: [constants.db_connection],
  },
];