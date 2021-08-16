import {
  Component, Input, OnInit, ViewChild,
} from '@angular/core';
import { Page } from 'src/app/models/page.model';
import { CommentService } from 'src/app/services/comment.service';
import { Comment } from 'src/app/models/comment.model';
import { ModalComponent } from '../../shared/modal/modal.component';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-replies',
  templateUrl: './replies.component.html',
  styleUrls: ['./replies.component.css'],
})
export class ReplyComponent implements OnInit {
  replies: Page<Comment>;

  isLoading = false;

  @Input()
  commId!: number;

  @Input()
  postId!: string;

  editID: any;

  @ViewChild(ModalComponent) modal!: ModalComponent;

  constructor(private commsService: CommentService, public authService: AuthService) {
    this.replies = {
      hasNextPage: false,
      hasPreviousPage: false,
      pageIndex: 1,
      items: [],
      pageSize: 0,
    };
  }

  ngOnInit(): void {
    this.isLoading = true;
    this.getAll();
    this.commsService.update.subscribe(() => {
      this.getAll();
    });
  }

  async getAll(pageType: string = '') {
    if (pageType == 'next'){
      this.replies.pageIndex++;
    }
    if (pageType == 'previous'){
      this.replies.pageIndex--;
    }
    this.replies = await this.commsService.getReplies(this.commId, this.postId, this.replies.pageIndex);
    this.isLoading = false;
  }

  async showModal(id: number) {
    const response = await this.modal.open();

    if (response) {
      this.delete(id);
    }
  }

  async delete(id: number) {
    await this.commsService.delete(id);
  }

  showEditor(id: number) {
    if (this.editID == id) {
      this.editID = null;
    } else {
      this.editID = id;
    }
  }
}
