import { ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule } from '@angular/forms';
import { CreateReplyComponent } from './create-reply.component';
import { CommentService } from 'src/app/services/comment.service';
import { HttpErrorService } from 'src/app/services/http-error.service';

describe('CreateReplyComponent', () => {
  let component: CreateReplyComponent;
  let fixture: ComponentFixture<CreateReplyComponent>;

  beforeEach(async () => {
    const commentSpy = jasmine.createSpyObj('CommentService', ['post']);
    const errorSpy= jasmine.createSpyObj('HttpErrorService', ['errorType'])
    await TestBed.configureTestingModule({
      declarations: [CreateReplyComponent],
      providers: [
        { provide: CommentService, useValue: commentSpy },
        { provide: HttpErrorService, useValue: errorSpy}
      ],
      imports: [FormsModule]
    })
      .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateReplyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
