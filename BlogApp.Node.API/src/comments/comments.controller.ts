import { Controller, Get, Post, Body, Put, Param, Delete, ParseIntPipe, BadRequestException, HttpException, Query } from '@nestjs/common';
import { CommentsService } from '../services/comments.service';
import { CommentDto } from '../models/dtos/comment.dto';
import { HttpStatus } from '@nestjs/common';
import { AuthService } from '../services/auth.service';

@Controller('comments')
export class CommentsController {

    constructor(
      private commentsService: CommentsService,
      private authService: AuthService) {}

  @Post()
  async create(@Body() input: CommentDto) {
      input.date = new Date().toString();
      input.userID = this.authService.user.id;
      const createdComm = await this.commentsService.create(input);
      if (createdComm == undefined){
        throw new HttpException('Bad request.', HttpStatus.BAD_REQUEST);
      }
  }

  @Get()
  async getTopComments(@Query() query: {postid: number, search: string, page: number}) {
    const comments = await this.commentsService.getTopComments(query.postid, query.page, query.search);
    return comments;
  }

  @Get(':id')
  async getReplies(
    @Param('id', ParseIntPipe) id: number,
    @Query() query: {postID: number, page: number
    }) {
    const replies = await this.commentsService.getReplies(query.postID, id, query.page);
    return replies;
  }


  @Put(':id')
  async update(@Param('id', ParseIntPipe) id: number, @Body() input: CommentDto) {
    if (id != input.id){
      throw new HttpException('Bad Request', HttpStatus.BAD_REQUEST);
    }

    const comment = await this.commentsService.put(input);
    if (comment == undefined){
      throw new HttpException('There is no comment with the given id.', HttpStatus.NOT_FOUND)
    }

    if (!this.authService.isOwner(comment.userID)){
      throw new HttpException('Cant touch this.', HttpStatus.FORBIDDEN);
    }
  }

  @Delete(':id')
  async remove(@Param('id', ParseIntPipe) id: number) {
     const comment = await this.commentsService.delete(id);
    if (comment == undefined){
      throw new HttpException('There is no comment with the given id.', HttpStatus.NOT_FOUND)
    }

    if (!this.authService.isOwner(comment.userID)){
      throw new HttpException('Cant touch this.', HttpStatus.FORBIDDEN);
    }
  }
}