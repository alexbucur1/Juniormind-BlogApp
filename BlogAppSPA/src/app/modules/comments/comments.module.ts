import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { CommentsComponent } from './comments/comments.component';
import { SharedModule } from '../shared/shared.module';
import { CreateCommentComponent } from './create-comment/create-comment.component';
import { EditCommentComponent } from './edit-comment/edit-comment.component';
import { ReplyComponent } from './replies/replies.component';
import { CreateReplyComponent } from './create-reply/create-reply.component';

@NgModule({
  declarations: [
    CommentsComponent,
    CreateCommentComponent,
    EditCommentComponent,
    ReplyComponent,
    CreateReplyComponent,
  ],
  imports: [
    CommonModule,
    SharedModule,
    FormsModule,
  ],
  exports: [
    CommentsComponent,
    CreateCommentComponent,
  ],
})
export class CommentsModule { }
