import { Component, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { Post } from 'src/app/models/post.model';
import { ImageService } from 'src/app/services/image.service';
import { PostService } from 'src/app/services/post.service';
import { HttpErrorService } from 'src/app/services/http-error.service';
import { ModalComponent } from '../../shared/modal/modal.component';

@Component({
  selector: 'app-create-post',
  templateUrl: './create-post.component.html',
  styleUrls: ['./create-post.component.css'],
})
export class PostCreateComponent{
  submitted = false;

  model: Post;

  public image!: File;

  imgURL: any;

  public message: string = '';

  @ViewChild(ModalComponent) modal!: ModalComponent;

  constructor(private postService: PostService,
     private imageService: ImageService,
      private router: Router,
      public errorService: HttpErrorService) {
    this.model = {
      title: '',
      id: 0,
      content: '',
      createdAt: '',
      modifiedAt: '',
      imageURL: null,
      userID: '',
      owner: '',
    };
  }

  async onSubmit() {
    const response = await this.postService.post(this.model);

    if (this.errorService.errorType == 'unauthorized'){
      return;
    }

    if (this.image) {
      await this.uploadImage(response.id);
    }

    if (this.errorService.errorType != 'server_not_responding'){
      await this.router.navigate(['']);
    }
  }

  async showModal() {
    if (this.model.content == ''){
      this.router.navigate(['/'])
      return;
    }

    const response = await this.modal.open();

    if (response) {
      this.router.navigate(['/'])
    }
  }

  async uploadImage(id: number) {
    const image: any = new FormData();
    image.append('File', this.image);
    image.append('PostID', id);

    this.imageService.put(id, image);
  }

  preview(file: FileList | null) {
    this.message = '';
    if (file == null) {
      return;
    }

    if (file.length === 0) {
      this.imgURL = undefined;
      return;
    }

    const mimeType = file[0].type;
    if (mimeType.match(/image\/*/) == null) {
      this.message = 'Only images are supported.';
      return;
    }

    const reader = new FileReader();
    this.image = file[0];
    reader.readAsDataURL(file[0]);
    reader.onload = (_event) => {
      this.imgURL = reader.result;
    };
  }
}
