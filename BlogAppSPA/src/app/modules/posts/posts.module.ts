import { NgModule } from '@angular/core';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { PostsComponent } from './posts/posts.component';
import { SharedModule } from '../shared/shared.module';
import { PostDetailsComponent } from './details-post/details-post.component';
import { PostCreateComponent } from './create-post/create-post.component';
import { PostEditComponent } from './edit-post/edit-post.component';
import { CommentsModule } from '../comments/comments.module';

@NgModule({
  declarations: [
    PostsComponent,
    PostDetailsComponent,
    PostCreateComponent,
    PostEditComponent,
  ],
  imports: [
    CommonModule,
    SharedModule,
    RouterModule,
    FormsModule,
    CommentsModule,
    NgbModule
  ],
})
export class PostsModule { }
