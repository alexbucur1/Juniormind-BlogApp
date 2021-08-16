import { Component, Injectable, OnInit } from '@angular/core';
import { Page } from 'src/app/models/page.model';
import { Post } from 'src/app/models/post.model';
import { PostService } from 'src/app/services/post.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-posts',
  templateUrl: './posts.component.html',
  styleUrls: ['./posts.component.css'],
})

@Injectable({ providedIn: 'root' })

export class PostsComponent implements OnInit {
  posts: Page<Post>;

  isLoading = false;

  baseUrl = environment.url_node;

  imageBaseUrl: string;

  search ='';

  constructor(private postService : PostService) {
    this.posts = {
      hasNextPage: false,
      hasPreviousPage: false,
      pageIndex: 1,
      items: [],
      pageSize: 0,
    };

    this.imageBaseUrl = this.baseUrl == environment.url_node ?
     this.baseUrl + '/api/image' :
      this.baseUrl;
  }

  async ngOnInit() {
    this.isLoading = true;
    this.getAll();
  }

  async getAll(pageType: string = ''): Promise<void> {
    if (pageType == 'next'){
      this.posts.pageIndex++;
    }
    if (pageType == 'previous'){
      this.posts.pageIndex--;
    }
    this.posts =  await this.postService.getAll(this.posts.pageIndex, this.search);
    this.isLoading = false;
  }
}
