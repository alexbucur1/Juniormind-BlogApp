import {
  Component, EventEmitter, Input, OnInit, Output,
} from '@angular/core';
import { CommentService } from 'src/app/services/comment.service';
import { Comment } from 'src/app/models/comment.model';
import { HttpErrorService } from 'src/app/services/http-error.service';

@Component({
  selector: 'app-edit-comment',
  templateUrl: './edit-comment.component.html',
  styleUrls: ['./edit-comment.component.css'],
})
export class EditCommentComponent implements OnInit {
  @Input()
  model!: Comment;

  updatedContent!: string;

  @Output() editedEvent = new EventEmitter<any>();

  constructor(private commService: CommentService, private errorService: HttpErrorService) {
  }

  ngOnInit(): void {
    this.updatedContent = this.model.content;
  }

  async onSubmit() {
    let initialContent = this.model.content;
    this.model.content = this.updatedContent;
    await this.commService.put(this.model);
    if (this.errorService.errorType != ''){
      this.model.content = initialContent;
    }
  }
}
