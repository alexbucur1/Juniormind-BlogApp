import { Inject, Injectable } from '@nestjs/common';
import { CommentDto } from '../models/dtos/comment.dto';
import { Comment } from '../models/entities/comment.entity';
import { Brackets, Repository } from 'typeorm';
import { constants } from '../constants';
import { User } from '../models/entities/user.entity';
import { BlogPost } from '../models/entities/blogpost.entity';
import { Paginated } from '../models/dtos/paginated.dto';

@Injectable()
export class CommentsService {
  constructor(
    @Inject(constants.comments_repository)
    private commentsRepository: Repository<Comment>,
    @Inject(constants.posts_repository)
    private postsRepository: Repository<BlogPost>,
    @Inject(constants.users_repository)
    private usersRepository: Repository<User>
  ) {}

  async create(comm: CommentDto): Promise<CommentDto> {
    let comment = this.getModel(comm);
    comment.id = null;
    const post = await this.postsRepository.findOne({id: comm.postID});
    const user = await this.usersRepository.findOne({id: comm.userID});
    const parent = await this.getCommentById(comm.parentID);
    if (post == undefined || user == undefined || (comm.parentID != 0 && parent == undefined && comm.parentID != undefined)){
      return undefined
    }

    comment.post = post;
    comment.user = user;
    return this.getDto(await this.commentsRepository.save(comment));
  }

  async getTopComments(postId: number, page: number = 1, search: string = ""): Promise<Paginated> {
    const comments = await this.commentsRepository
    .createQueryBuilder("comment")
    .leftJoinAndSelect("comment.user", "user")
    .leftJoinAndSelect("comment.post", "post")
    .where("post.id = :postId", {postId: postId})
    .andWhere("comment.parentID is null")
    .andWhere(new Brackets(qb => {
      qb.where("comment.content like :content", {content: '%' + search + '%'})
      .orWhere("user.firstName like :firstName", {firstName: '%' + search + '%'})
      .orWhere("user.lastName like :lastName", {lastName: '%' + search + '%'})
    }))
    .orderBy("comment.date", "DESC")
      .skip((page - 1) * constants.page_size)
      .take(constants.page_size + 1)
      .getMany();

      const allIDs = await this.commentsRepository
      .createQueryBuilder("comment")
      .select("comment.parentID")
      .getRawMany();
      return this.getPage(comments, page, 1, allIDs);
  }

  async getReplies(postId: number, parentID: number, repliesPageNumber: number = -1): Promise<Paginated> {
    let pageSize = constants.page_size;
    let replies:Comment[] = [];
    if (repliesPageNumber != -1){
      replies = await this.commentsRepository
      .createQueryBuilder("comment")
      .leftJoinAndSelect("comment.post", "post")
      .leftJoinAndSelect("comment.user", "user")
      .where("post.id = :postId", {postId: postId})
      .andWhere("comment.parentID = :parentID", {parentID: parentID})
      .skip((repliesPageNumber - 1) * constants.page_size)
      .take(constants.page_size + 1)
      .getMany();
    }
    else{
      pageSize = -1;
      replies = await this.commentsRepository
      .createQueryBuilder("comment")
      .leftJoinAndSelect("comment.post", "post")
      .leftJoinAndSelect("comment.user", "user")
      .where("post.id = :postId", {postId: postId})
      .andWhere("comment.parentID = :parentID", {parentID: parentID})
      .getMany();
    }

    return this.getPage(replies, repliesPageNumber, pageSize);
  }

  async put(comm: CommentDto): Promise<CommentDto> {
    let editedComm = undefined;
    if (await this.getCommentById(comm.id) == undefined){
      return editedComm;
    }
     editedComm = await this.commentsRepository.save(this.getModel(comm));
     return this.getDto(editedComm);
  }

  async delete(id: number): Promise<CommentDto>{
    const comm = await this.getCommentById(id);
    if (comm != undefined){
      await this.commentsRepository
      .createQueryBuilder()
      .delete()
      .from(Comment)
      .andWhere("parentID = :parentID", {parentID: id})
      .execute();
      this.commentsRepository.remove(comm);
    }
    return this.getDto(comm);
  }

  private getModel(dto: CommentDto): Comment{
    return dto == undefined ? undefined : {
      id: dto.id,
      user: new User(),
      post: new BlogPost(),
      content: dto.content,
      date: dto.date,
      parentID: dto.parentID,
    };
  }

  private getDto(model: Comment): CommentDto{
    return model == undefined ? undefined : {
      id: model.id,
      userID: model.user.id,
      postID: model.post.id,
      content: model.content,
      date: model.date,
      parentID: model.parentID,
      userFullName: model.user.firstName + ' ' + model.user.lastName,
      repliesCount: 0,
    };
  }

  private getPage(comments: Comment[], pageIndex: number, pageSize: number = constants.page_size, allIDs: any[] = []): Paginated{
    let hasNextPage = false;
    if (comments.length > constants.page_size && pageSize != -1){
      hasNextPage = true;
      comments.pop();
    }

    let page = {
      pageIndex:  pageIndex,
      pageSize: constants.page_size,
      hasNextPage: hasNextPage,
      hasPreviousPage: pageIndex > 1,
      items: comments.map(comment => this.getDto(comment))
      };

      page.items.map(comment => {
        let repliesIDs = allIDs.filter(id => id.comment_parentID == comment.id);
        comment.repliesCount = repliesIDs.length;
      })
      return page
  }

  private async getCommentById(id: number): Promise<Comment>{
    return await this.commentsRepository
    .createQueryBuilder("comment")
    .leftJoinAndSelect("comment.post", "post")
    .leftJoinAndSelect("comment.user", "user")
    .where("comment.id = :commID", {commID: id})
    .getOne();
  }
}