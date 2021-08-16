import { ComponentFixture, TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { PostEditComponent } from './edit-post.component';
import { ActivatedRoute } from '@angular/router';
import { of } from 'rxjs';
import { convertToParamMap } from '@angular/router';
import { ImageService } from 'src/app/services/image.service';
import { PostService } from 'src/app/services/post.service';
import { HttpErrorService } from 'src/app/services/http-error.service';
import { FormsModule } from '@angular/forms';

describe('PostEditComponent', () => {
  let component: PostEditComponent;
  let fixture: ComponentFixture<PostEditComponent>;

  beforeEach(async () => {
    const postSpy = jasmine.createSpyObj('PostsService', ['put', 'get']);
    const errorSpy = jasmine.createSpyObj('HttpErrorService', ['errorType']);
    const imageSpy = jasmine.createSpyObj('ImageService', ['put']);
    await TestBed.configureTestingModule({
      declarations: [PostEditComponent],
      imports: [RouterTestingModule, FormsModule],
      providers: [
        { provide: PostService, useValue: postSpy },
        { provide: HttpErrorService, useValue: errorSpy },
        { provide: ImageService, useValue: imageSpy},
        {
          provide: ActivatedRoute,
          useValue: { paramMap: of(convertToParamMap({id: 1})) }
        }
      ]
    })
      .compileComponents();
  });

  beforeEach(() => {
    const post = {
      id: 0,
      title: '',
      content: 'content',
      createdAt: '',
      modifiedAt: '',
      imageURL: '',
      userID: '',
      owner: '',
    }
    fixture = TestBed.createComponent(PostEditComponent);
    component = fixture.componentInstance;
    component.post = post;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
