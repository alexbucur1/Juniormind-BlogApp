import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Post } from 'src/app/models/post.model';
import { ImageService } from 'src/app/services/image.service';
import { PostService } from 'src/app/services/post.service';
import { environment } from 'src/environments/environment';
import { HttpErrorService } from 'src/app/services/http-error.service';
import { ModalComponent } from '../../shared/modal/modal.component';

@Component({
  selector: 'app-edit-post',
  templateUrl: './edit-post.component.html',
  styleUrls: ['./edit-post.component.css'],
})
export class PostEditComponent implements OnInit {
  id: any = '';

  isLoading = false;

  post: Post;

  imgURL: any;

  public message: string = '';

  public image!: File;

  baseUrl = environment.url_node;

  imageBaseUrl: string;

  initialContent = '';

  @ViewChild(ModalComponent) modal!: ModalComponent;

  constructor(private route: ActivatedRoute,
     private postService: PostService,
      private router: Router,
       private imageService: ImageService,
       public errorService: HttpErrorService) {
    this.post = {
      id: 0,
      title: '',
      content: '',
      createdAt: '',
      modifiedAt: '',
      imageURL: '',
      userID: '',
      owner: '',
    };

    this.imageBaseUrl = this.baseUrl == environment.url_node ? this.baseUrl + '/api/image' : this.baseUrl;
  }

  ngOnInit(): void {
    this.isLoading = true;
    this.route.paramMap.subscribe((params) => {
      this.id = params.get('id');
      this.get(this.id);
    });
  }

  async showModal() {
    if (this.post.content == this.initialContent){
      this.router.navigate([`/details/${this.post.id}`])
      return;
    }

    const response = await this.modal.open();

    if (response) {
      this.router.navigate([`/details/${this.post.id}`])
    }
  }

  async get(id: string): Promise<void> {
    var post = await this.postService.get(id);
    this.post = post;
    this.initialContent = post.content;
    if (this.post.imageURL != null) {
      this.imgURL = this.imageBaseUrl + this.post.imageURL;
    }
    this.isLoading = false;
  }

  async onSubmit() {
    const response = await this.postService.put(this.id, this.post);

    if (this.errorService.errorType == 'unauthorized'){
      return;
    }

    if (this.image) {
      await this.uploadImage(this.post.id);
    }

    if (this.errorService.errorType != 'server_not_responding'){
      await this.router.navigate(['/details/', this.post.id]);
    }
  }

  async uploadImage(id: number) {
    const formData: any = new FormData();
    formData.append('File', this.image);
    formData.append('PostID', id);
    await this.imageService.put(id, formData);
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
