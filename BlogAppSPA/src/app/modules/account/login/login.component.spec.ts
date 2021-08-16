import { ComponentFixture, TestBed } from '@angular/core/testing';
import { LoginComponent } from './login.component';
import { PreviousPageService } from 'src/app/services/previous-page.service';

describe('LoginComponent', () => {
  let component: LoginComponent;
  let fixture: ComponentFixture<LoginComponent>;
  let previousPageServiceSpy: jasmine.SpyObj<PreviousPageService>;

  beforeEach(async () => {
    const previousPageSpy = jasmine.createSpyObj('PreviousPageService', ['previousPage']);
    await TestBed.configureTestingModule({
      declarations: [ LoginComponent ],
      providers: [
        { provide: PreviousPageService, useValue: previousPageSpy},
      ]
    })
    .compileComponents();
    previousPageServiceSpy = TestBed.inject(PreviousPageService) as jasmine.SpyObj<PreviousPageService>;
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(LoginComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
