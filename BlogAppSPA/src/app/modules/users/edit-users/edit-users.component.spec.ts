import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import { Router } from '@angular/router';
import { ActivatedRoute } from '@angular/router';
import { of } from 'rxjs';
import { convertToParamMap } from '@angular/router';
import { EditUsersComponent } from './edit-users.component';
import { UserService } from 'src/app/services/user.service';
import { ValidationService } from 'src/app/services/validation.service';
import { HttpErrorService } from 'src/app/services/http-error.service';
import { FormsModule } from '@angular/forms';
import { User } from 'src/app/models/user.model';
import { PreviousPageService } from 'src/app/services/previous-page.service';

describe('EditUsersComponent', () => {
  let component: EditUsersComponent;
  let fixture: ComponentFixture<EditUsersComponent>;
  let compiled: any;
  let httpErrorServiceSpy: jasmine.SpyObj<HttpErrorService>;
  let userServiceSpy: jasmine.SpyObj<UserService>;
  let validationServiceSpy: jasmine.SpyObj<ValidationService>;
  let previousPageServiceSpy: jasmine.SpyObj<PreviousPageService>;
  let router = {
    navigate: jasmine.createSpy('navigate')
  }

  beforeEach(async () => {
    const errorSpy = jasmine.createSpyObj('HttpErrorService', ['errorType']);
    const userSpy = jasmine.createSpyObj('UserService', ['get', 'put']);
    const validationSpy = jasmine.createSpyObj('ValidationService', ['validatePassword', 'validateEmail']);
    const previousPageSpy = jasmine.createSpyObj('PreviousPageService', ['previousPage']);
    await TestBed.configureTestingModule({
      declarations: [ EditUsersComponent ],
      imports: [ FormsModule ],
      providers: [
        { provide: UserService, useValue: userSpy },
        { provide: Router, useValue: router },
        { provide: ValidationService, useValue: validationSpy },
        { provide: HttpErrorService, useValue: errorSpy },
        { provide: PreviousPageService, previousPageServiceSpy: previousPageSpy },
        {
          provide: ActivatedRoute,
          useValue: { paramMap: of(convertToParamMap({id: 'testuser'})) }
        }
      ]
    })
    .compileComponents();
    httpErrorServiceSpy = TestBed.inject(HttpErrorService) as jasmine.SpyObj<HttpErrorService>;
    userServiceSpy = TestBed.inject(UserService) as jasmine.SpyObj<UserService>;
    validationServiceSpy = TestBed.inject(ValidationService) as jasmine.SpyObj<ValidationService>;
    previousPageServiceSpy = TestBed.inject(PreviousPageService) as jasmine.SpyObj<PreviousPageService>;
  });

  beforeEach(() => {
    const user = {
      id: 'testuser',
      firstName: 'Test',
      lastName: 'User',
      email:'testuser@mail.com',
      password: ''
    }
    const userAspromise = new Promise<User>((resolve) => {resolve(user)});
    userServiceSpy.get.and.returnValue(userAspromise);
    httpErrorServiceSpy.errorType = '';
    fixture = TestBed.createComponent(EditUsersComponent);
    compiled = fixture.debugElement.nativeElement;
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
  it('user information is rendered on page', waitForAsync(() => {
    fixture.whenStable().then(() => {
      fixture.detectChanges();
      expect(compiled.querySelector('#firstName').value).toContain('Test');
      expect(compiled.querySelector('#lastName').value).toContain('User');
      expect(compiled.querySelector('#email').value).toContain('testuser@mail.com');
    })
  }));
  it('"invalid email" message appears if email does not respect the standard.', waitForAsync(() => {
    validationServiceSpy.validateEmail.and.returnValue(false);
    fixture.whenStable().then(() => {
      fixture.detectChanges();
      expect(compiled.querySelector('#invalid-email-message').textContent).toContain('The given email does not respect the standard. (ex.: \'name@gmail.com\')');
    })
  }));
  it('"invalid password" message appears if password is not complex enough', waitForAsync(() => {
    validationServiceSpy.validatePassword.and.returnValue(false);
    fixture.whenStable().then(() => {
      fixture.detectChanges();
      expect(compiled.querySelector('#invalid-password-message').textContent)
      .toContain('Password must contain at least: 1 lowercase letter, 1 uppercase letter, 1 digit, 1 symbol, 8 characters.');
    })
  }));
  it('"email already taken" message appears when given email is taken by other user.', waitForAsync(() => {
    httpErrorServiceSpy.errorType = 'email_already_taken';
    httpErrorServiceSpy.message = 'The given email is already taken by other user.';
    fixture.whenStable().then(() => {
      fixture.detectChanges();
      expect(compiled.querySelector('#error-message').textContent)
      .toContain('The given email is already taken by other user.');
    })
  }));
  it('"server not responding" message appears when the server is offline.', waitForAsync(() => {
    httpErrorServiceSpy.errorType = 'server_not_responding';
    httpErrorServiceSpy.message = 'The server is not responding.';
    fixture.whenStable().then(() => {
      fixture.detectChanges();
      expect(compiled.querySelector('#error-message').textContent)
      .toContain('The server is not responding.');
    })
  }));
  it('the post is edited when the "Save" button is pushed.', waitForAsync(() => {
    fixture.whenStable().then(() => {
      fixture.detectChanges();
      let button = compiled.querySelector('#save-button');
      button.click();
      expect(userServiceSpy.put.calls.count()).toBe(1);
    });
  }));
  it('redirects when the "Close" button is pushed', waitForAsync(() => {
    fixture.whenStable().then(() => {
      fixture.detectChanges();
      let button = compiled.querySelector('#back-button');
      button.click();
      expect(router.navigate).toHaveBeenCalled();
    });
  }));
});
