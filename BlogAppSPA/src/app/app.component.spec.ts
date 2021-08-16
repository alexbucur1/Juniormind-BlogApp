import { TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { AppComponent } from './app.component';
import { PreviousPageService } from './services/previous-page.service';

describe('AppComponent', () => {
  beforeEach(async () => {
    const previousPageSpy = jasmine.createSpyObj('PreviousPageService', ['start']);
    await TestBed.configureTestingModule({
      imports: [
        RouterTestingModule,
      ],
      declarations: [
        AppComponent,
      ],
      providers:[
        { provide: PreviousPageService, useValue: previousPageSpy}
      ]
    }).compileComponents();
  });

  it('should create the app', () => {
    const fixture = TestBed.createComponent(AppComponent);
    const app = fixture.componentInstance;
    expect(app).toBeTruthy();
  });

  it('should have as title \'BlogAppSPA\'', () => {
    const fixture = TestBed.createComponent(AppComponent);
    const app = fixture.componentInstance;
    expect(app.title).toEqual('BlogAppSPA');
  });
});
