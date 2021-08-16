import { Controller, Get, HttpException, HttpStatus, Post, Put, Res } from '@nestjs/common';
import { Param } from  '@nestjs/common';
import { UseInterceptors, UploadedFile } from  '@nestjs/common';
import { FileInterceptor } from '@nestjs/platform-express';
import { diskStorage } from  'multer';
import { PostsService } from '../services/posts.service';
import { editFileName } from './utilities/edit-file-name';
import { imageFileFilter } from './utilities/image-filter';
import { ImagesService } from '../services/images.service';

@Controller('image')
export class ImagesController {
    constructor(
      private postService: PostsService,
      private imageService: ImagesService){}

@Put(':postid')
@UseInterceptors(FileInterceptor('File',
  {
      storage: diskStorage({
      destination: '../static/Assets/Uploads',
    filename: editFileName
  }),
      fileFilter: imageFileFilter
  }
))
async upload(@Param('postid') postid, @UploadedFile() file: Express.Multer.File) {
  const pathPrefix = '../static/Assets/Uploads/';
  const imgUrlPrefix = '/Assets/Uploads/';
  let post = await this.postService.get(postid);
  if ( post == undefined){
      throw new HttpException('post not found', HttpStatus.BAD_REQUEST);
  }

  if (file == undefined){
    throw new HttpException('given image is empty', HttpStatus.BAD_REQUEST);
  }

  this.imageService.delete(post.imageURL);

  post.imageURL = imgUrlPrefix + file.path.slice(pathPrefix.length);
  await this.postService.put(post);
}

@Get('Assets/Uploads/:imgpath')
seeUploadedFile(@Param('imgpath') imagePath, @Res() res) {
  return res.sendFile(imagePath, { root: '../static/Assets/Uploads' });
}
}
