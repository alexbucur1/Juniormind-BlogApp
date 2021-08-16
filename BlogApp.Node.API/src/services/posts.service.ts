import { Inject, Injectable } from '@nestjs/common';
import { BlogPostDto } from '../models/dtos/blogpost.dto';
import { Repository } from 'typeorm';
import { BlogPost } from '../models/entities/blogpost.entity';
import { constants } from '../constants';
import { Paginated } from '../models/dtos/paginated.dto';
import { User } from '../models/entities/user.entity';

@Injectable()
export class PostsService {

  constructor(
    @Inject(constants.posts_repository)
    private postsRepository: Repository<BlogPost>,
    @Inject(constants.users_repository)
    private usersRepository: Repository<User>
  ) {}

  async create(post: BlogPostDto): Promise<BlogPostDto> {
    let postModel = this.getModel(post);
    const user = await this.usersRepository.findOne({id: post.userID});
    if (user == undefined){
      return undefined;
    }
    postModel.id = null;
    postModel.user = user;
    return this.getDto(await this.postsRepository.save(postModel));
  }

  async getAll(search: string = "", page: number = 1): Promise<Paginated> {
    let posts = await this.postsRepository
    .createQueryBuilder("post")
    .leftJoinAndSelect("post.user", "user")
      .where("post.title like :title", {title: '%' + search + '%'})
      .orWhere("post.content like :content", {content: '%' + search + '%'})
      .orWhere("user.firstName like :firstName", {firstName: '%' + search + '%'})
      .orWhere("user.lastName like :lastName", {lastName: '%' + search + '%'})
      .orderBy("post.createdAt", "DESC")
      .skip((page - 1) * constants.page_size)
      .take(constants.page_size + 1)
      .getMany();
    return this.getPage(posts, page);
  }

  async get(id: number): Promise<BlogPostDto> {
    const post = await this.postsRepository.findOne({id: id})
      return this.getDto(post);
  }

  async put(post: BlogPostDto) {
     await this.postsRepository.save(this.getModel(post));
  }

  async delete(id: number): Promise<BlogPostDto>{
    const post = await this.postsRepository.findOne({id: id});
    if (post == undefined){
      return undefined;
    }

    await this.postsRepository.remove(post);
    return this.getDto(post);
  }

  private getModel(dto: BlogPostDto): BlogPost{
    return dto == undefined ? undefined : {
      id: dto.id,
      title: dto.title,
      content: dto.content,
      createdAt: dto.createdAt,
      modifiedAt: dto.modifiedAt,
      imageURL: dto.imageURL,
      user: new User()
    };
  }

  private getDto(model: BlogPost): BlogPostDto{
    return model == undefined ? undefined : {
      id: model.id,
      title: model.title,
      content: model.content,
      createdAt: model.createdAt,
      modifiedAt: model.modifiedAt,
      imageURL: model.imageURL,
      userID: model.user.id,
      owner: model.user.firstName + ' ' +  model.user.lastName
    };
  }

  private getPage(posts: BlogPost[], pageIndex: number): Paginated{
    let hasNextPage = false;
    if (posts.length > constants.page_size){
      hasNextPage = true;
      posts.pop();
    }

    return {
      pageIndex:  pageIndex,
      pageSize: constants.page_size,
      hasNextPage: hasNextPage,
      hasPreviousPage: pageIndex > 1,
      items: posts.map(post => this.getDto(post))
    };
  }
}
