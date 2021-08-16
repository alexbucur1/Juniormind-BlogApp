import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ActivatedRoute } from '@angular/router';
import { of } from 'rxjs';
import { convertToParamMap } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CreateCommentComponent } from './create-comment.component';
import { CommentService } from 'src/app/services/comment.service';
import { HttpErrorService } from 'src/app/services/http-error.service';

describe('CreateCommentComponent', () => {
  let component: CreateCommentComponent;
  let fixture: ComponentFixture<CreateCommentComponent>;

  beforeEach(async () => {
    const commentSpy = jasmine.createSpyObj('CommentService', ['post']);
    const errorSpy = jasmine.createSpyObj('HttpErrorService', ['errorType']);
    await TestBed.configureTestingModule({
      declarations: [CreateCommentComponent],
      providers: [
        { provide: CommentService, useValue: commentSpy },
        { provide: HttpErrorService, useValue: errorSpy },
        {
          provide: ActivatedRoute,
          useValue: { paramMap: of(convertToParamMap({id: 1})) }
        }
      ],
      imports: [FormsModule]
    })
      .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateCommentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
