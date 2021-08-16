import { createConnection } from 'typeorm';
import { constants } from '../../constants';
import { BlogPost } from '../../models/entities/blogpost.entity';
import { Comment } from '../../models/entities/comment.entity';
import { seed } from '../seed-data';
import { User } from '../../models/entities/user.entity';
import { usersProviders } from './users.providers';

export const databaseProviders = [
  {
    provide: constants.db_connection,
    useFactory: async () => await createConnection({
      type: 'mysql',
      host: '127.0.0.1',
      port: 3306,
      username: 'root',
      password: 'SQLPassword89$',
      database: 'blogpostcontext-1',
      synchronize: false,
      entities: [
          BlogPost,
          Comment,
          User
      ],
    }).then(async connection => {
      let usersRepository = connection.getRepository(User);
      let postsRepository = connection.getRepository(BlogPost);
      const user = await usersRepository.findOne({id: 'testuser'});
      if (await postsRepository.count() == 0){
        seed.Posts.map(post => post.user = user);
        await postsRepository.save(seed.Posts);
      }
      return connection;
    }).catch(error => console.log(error))
  },
];