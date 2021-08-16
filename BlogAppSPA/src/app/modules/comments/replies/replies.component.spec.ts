import { ComponentFixture, TestBed } from '@angular/core/testing';
import { EventEmitter } from '@angular/core';
import { ReplyComponent } from './replies.component';
import { AuthService } from 'src/app/services/auth.service';
import { CommentService } from 'src/app/services/comment.service';

describe('ReplyComponent', () => {
  let component: ReplyComponent;
  let fixture: ComponentFixture<ReplyComponent>;



  beforeEach(async () => {
    let emitter = new EventEmitter();
    const authSpy = jasmine.createSpyObj('AuthService', ['userClient']);
    const commentSpy = jasmine.createSpyObj('CommentService', ['delete', 'getReplies'], {'update': emitter});
    await TestBed.configureTestingModule({
      declarations: [ReplyComponent],
      providers: [
        { provide: AuthService, useValue: authSpy },
        { provide: CommentService, useValue: commentSpy }
      ]
    })
      .compileComponents();

  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ReplyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
