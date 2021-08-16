import { ComponentFixture, TestBed } from '@angular/core/testing';
import { PostService } from 'src/app/services/post.service';
import { PostsComponent } from './posts.component';

describe('PostsComponent', () => {
  let component: PostsComponent;
  let fixture: ComponentFixture<PostsComponent>;

  beforeEach(async () => {
    const postSpy = jasmine.createSpyObj('PostsService', ['getAll']);
    await TestBed.configureTestingModule({
      declarations: [PostsComponent], 
      providers: [
        { provide: PostService, useValue: postSpy },
      ]
    })
      .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PostsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
