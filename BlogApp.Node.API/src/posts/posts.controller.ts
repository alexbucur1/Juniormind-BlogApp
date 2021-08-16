import { HttpException } from '@nestjs/common';
import { HttpStatus } from '@nestjs/common';
import { Query } from '@nestjs/common';
import { Controller, Get, Post, Body, Put, Param, Delete, ParseIntPipe } from '@nestjs/common';
import { PostsService } from '../services/posts.service';
import { BlogPostDto } from '../models/dtos/blogpost.dto';
import { ImagesService } from '../services/images.service';
import { AuthService } from '../services/auth.service';

@Controller('posts')
export class PostsController {

    constructor(
      private postsService: PostsService,
      private imageService: ImagesService,
      private authService: AuthService) {}

  @Post()
  async create(@Body() input: BlogPostDto) {
    input.createdAt = new Date().toString();
    input.modifiedAt = input.createdAt;
    input.userID = this.authService.user.id;
    const createdPost =  await this.postsService.create(input);
    if (createdPost == undefined){
      throw new HttpException('', HttpStatus.BAD_REQUEST);
    }
  }

  @Get()
  async getAll(@Query() query: {search: string, page: number}) {
    const posts = await this.postsService.getAll(query.search, query.page);
    return posts;
  }

  @Get(':id')
  async findOne(@Param('id', ParseIntPipe) id: number): Promise<BlogPostDto> {
    const post = await this.postsService.get(id);
    if (post == undefined){
      throw new HttpException('No post was found with the given id.', HttpStatus.NOT_FOUND);
    }

    return post;
  }

  @Put(':id')
  async update(@Param('id', ParseIntPipe) id: number, @Body() input: BlogPostDto) {
    if (id != input.id){
      throw new HttpException('Bad Request', HttpStatus.BAD_REQUEST);
    }

    const post = await this.postsService.get(id);
    if (post == undefined){
      throw new HttpException('There is no post with the given id.', HttpStatus.NOT_FOUND)
    }

    if (!this.authService.isOwner(post.userID)){
      throw new HttpException('Cant touch this.', HttpStatus.FORBIDDEN);
    }

    input.createdAt = post.createdAt;
    input.modifiedAt = new Date().toString();
    await this.postsService.put(input)
  }

  @Delete(':id')
  async remove(@Param('id', ParseIntPipe) id: number) {
    const post = await this.postsService.delete(id);
    if (post == undefined){
      throw new HttpException('No post was found with the given id.', HttpStatus.NOT_FOUND);
    }

    if (!this.authService.isOwner(post.userID)){
      throw new HttpException('Cant touch this.', HttpStatus.FORBIDDEN);
    }

    this.imageService.delete(post.imageURL);
  }
}