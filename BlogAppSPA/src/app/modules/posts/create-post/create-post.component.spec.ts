import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ImageService } from 'src/app/services/image.service';
import { PostService } from 'src/app/services/post.service';
import { HttpErrorService } from 'src/app/services/http-error.service';
import { PostCreateComponent } from './create-post.component';
import { RouterTestingModule } from '@angular/router/testing';
import { FormsModule } from '@angular/forms';

describe('CreateComponent', () => {
  let component: PostCreateComponent;
  let fixture: ComponentFixture<PostCreateComponent>;

  beforeEach(async () => {
    const postSpy = jasmine.createSpyObj('PostsService', ['post']);
    const errorSpy = jasmine.createSpyObj('HttpErrorService', ['errorType']);
    const imageSpy = jasmine.createSpyObj('ImageService', ['put']);
    await TestBed.configureTestingModule({
      declarations: [PostCreateComponent],
      imports: [
        RouterTestingModule,
        FormsModule
      ],
      providers: [
        { provide: PostService, useValue: postSpy },
        { provide: HttpErrorService, useValue: errorSpy },
        { provide: ImageService, useValue: imageSpy},
      ]
    })
      .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PostCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
