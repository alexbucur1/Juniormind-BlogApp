import { Injectable } from '@nestjs/common';
import * as fs from 'fs';

@Injectable()
export class ImagesService {
    public delete(imgUrl: string){
        const pathPrefix = '../static/Assets/Uploads/';
        const imgUrlPrefix = '/Assets/Uploads/';
        if (imgUrl != null){
            imgUrl = imgUrl.slice(imgUrlPrefix.length);
            fs.unlink(pathPrefix + imgUrl, () => {});
          }
    }
}
