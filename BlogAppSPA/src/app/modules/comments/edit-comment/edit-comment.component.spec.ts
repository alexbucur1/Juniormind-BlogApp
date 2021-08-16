import { ComponentFixture, TestBed } from '@angular/core/testing';
import { CommentService } from 'src/app/services/comment.service';
import { EditCommentComponent } from './edit-comment.component';
import { FormsModule } from '@angular/forms';
import { HttpErrorService } from 'src/app/services/http-error.service';

describe('EditCommentComponent', () => {
  let component: EditCommentComponent;
  let fixture: ComponentFixture<EditCommentComponent>;

  beforeEach(async () => {
    const commentSpy = jasmine.createSpyObj('CommentService', ['put']);
    const errorSpy = jasmine.createSpyObj('HttpErrorService', ['errorType']);
    await TestBed.configureTestingModule({
      declarations: [EditCommentComponent],
      providers: [
        { provide: CommentService, useValue: commentSpy },
        { provide: HttpErrorService, useValue: errorSpy},
      ],
      imports: [FormsModule]
    })
      .compileComponents();
  });

  beforeEach(() => {
    const comment = {
      id: 0,
      userID: '',
      postID: -1,
      content: '',
      date: new Date(),
      parentID: null,
      userFullName: '',
      repliesCount: 0,
    };
    fixture = TestBed.createComponent(EditCommentComponent);
    component = fixture.componentInstance;
    component.model = comment;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
