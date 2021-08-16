import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ActivatedRoute } from '@angular/router';
import { of } from 'rxjs';
import { convertToParamMap } from '@angular/router';
import { EventEmitter } from '@angular/core';

import { CommentsComponent } from './comments.component';
import { AuthService } from 'src/app/services/auth.service';
import { CommentService } from 'src/app/services/comment.service';
import { HttpErrorService } from 'src/app/services/http-error.service';

describe('CommentsComponent', () => {
  let component: CommentsComponent;
  let fixture: ComponentFixture<CommentsComponent>;

  beforeEach(async () => {
    let emitter = new EventEmitter();
    const authSpy = jasmine.createSpyObj('AuthService', ['userClient']);
    const commentSpy = jasmine.createSpyObj('CommentService', ['getAll', 'delete'], {'update': emitter});
    const errorSype = jasmine.createSpyObj('HttpErrorService', ['errorType']);
    await TestBed.configureTestingModule({
      declarations: [CommentsComponent],
      providers: [
        { provide: AuthService, useValue: authSpy },
        { provide: CommentService, useValue: commentSpy },
        { provide: HttpErrorService, useValue: errorSype },
        {
          provide: ActivatedRoute,
          useValue: { paramMap: of(convertToParamMap({id: 1})) }
        }
      ]
    })
      .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CommentsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
