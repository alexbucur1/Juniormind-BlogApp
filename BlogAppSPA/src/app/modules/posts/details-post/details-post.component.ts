import {
  Component, ElementRef, OnInit, ViewChild,
} from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';
import { Post } from 'src/app/models/post.model';
import { PostService } from 'src/app/services/post.service';
import { environment } from 'src/environments/environment';
import { ModalComponent } from '../../shared/modal/modal.component';
import { HttpErrorService } from 'src/app/services/http-error.service';

@Component({
  selector: 'app-details-post',
  templateUrl: './details-post.component.html',
  styleUrls: ['./details-post.component.css'],
})
export class PostDetailsComponent implements OnInit {
  id: any = '';

  isLoading = false;

  post: Post;

  baseUrl = environment.url_node;

  imageBaseUrl: string;

  isCollapsed = false;

  @ViewChild(ModalComponent) modal!: ModalComponent;

  constructor(private route: ActivatedRoute,
     private postService: PostService,
      private router: Router,
       public authService: AuthService,
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

  async get(id: string): Promise<void> {
    this.post = await this.postService.get(id);
    this.isLoading = false;
  }

  async delete(): Promise<void> {
    await this.postService.delete(this.id);
    if (this.errorService.errorType == ''){
      await this.router.navigate(['']);
    }
  }

  async showModal() {
    const response = await this.modal.open();

    if (response) {
      this.delete();
    }
  }

  isOwner(userID: string){
    return userID == this.authService.userClient.userID ||
    this.authService.userClient.userIsAdmin;
  }
}
