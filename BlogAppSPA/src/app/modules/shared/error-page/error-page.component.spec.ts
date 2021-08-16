import { ComponentFixture, TestBed } from '@angular/core/testing';
import { PreviousPageService } from 'src/app/services/previous-page.service';
import { HttpErrorService } from 'src/app/services/http-error.service';
import { ErrorPageComponent } from './error-page.component';

describe('ErrorPageComponent', () => {
  let component: ErrorPageComponent;
  let fixture: ComponentFixture<ErrorPageComponent>;

  beforeEach(async () => {
    const errorSpy = jasmine.createSpyObj('HttpErrorService', ['clearErrorState', 'errorType']);
    const previousPageSpy = jasmine.createSpyObj('PreviousPageService', ['previousPage']);
    const locationStub = {
      back: jasmine.createSpy('back')
  }
    await TestBed.configureTestingModule({
      declarations: [ ErrorPageComponent ],
      providers: [
        { provide: HttpErrorService, useValue: errorSpy },
        { provide: PreviousPageService, useValue: previousPageSpy},
        { provide: Location, useValue: locationStub}
      ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ErrorPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
