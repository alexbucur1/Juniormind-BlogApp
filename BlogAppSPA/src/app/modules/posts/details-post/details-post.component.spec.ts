import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ActivatedRoute } from '@angular/router';
import { of } from 'rxjs';
import { convertToParamMap } from '@angular/router';
import { RouterTestingModule } from '@angular/router/testing';
import { PostDetailsComponent } from './details-post.component';
import { PostService } from 'src/app/services/post.service';
import { HttpErrorService } from 'src/app/services/http-error.service';
import { AuthService } from 'src/app/services/auth.service';

describe('DetalisComponent', () => {
  let component: PostDetailsComponent;
  let fixture: ComponentFixture<PostDetailsComponent>;

  beforeEach(async () => {
    const postSpy = jasmine.createSpyObj('PostsService', ['delete', 'get']);
    const errorSpy = jasmine.createSpyObj('HttpErrorService', ['errorType']);
    const authSpy = jasmine.createSpyObj('AuthService', ['signin']);
    await TestBed.configureTestingModule({
      declarations: [PostDetailsComponent],
      imports: [RouterTestingModule],
      providers: [
        { provide: PostService, useValue: postSpy },
        { provide: HttpErrorService, useValue: errorSpy },
        { provide: AuthService, useValue: authSpy },
        {
          provide: ActivatedRoute,
          useValue: { paramMap: of(convertToParamMap({id: 1})) }
        }
      ]
    })
      .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PostDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
