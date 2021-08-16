import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { RegisterComponent } from './register.component';
import { FormsModule } from '@angular/forms';
import { UserService } from 'src/app/services/user.service';
import { ValidationService } from 'src/app/services/validation.service';
import { HttpErrorService } from 'src/app/services/http-error.service';
import {Router} from '@angular/router';
import {Location} from '@angular/common';

describe('RegisterComponent', () => {
  let component: RegisterComponent;
  let fixture: ComponentFixture<RegisterComponent>;
  let compiled: any;
  let firstNameInput: any;
  let lastNameInput: any;
  let emailInput: any;
  let passwordInput: any;
  let httpErrorServiceSpy: jasmine.SpyObj<HttpErrorService>;
  let userServiceSpy: jasmine.SpyObj<UserService>;
  let validationServiceSpy: jasmine.SpyObj<ValidationService>;

  beforeEach(async () => {
    const errorSpy = jasmine.createSpyObj('HttpErrorService', ['errorType']);
    const userSpy = jasmine.createSpyObj('UserService', ['post']);
    const validationSpy = jasmine.createSpyObj('ValidationService', ['validatePassword', 'validateEmail']);
    await TestBed.configureTestingModule({
      declarations: [ RegisterComponent ],
      imports: [
        RouterTestingModule,
         FormsModule
      ],
      providers: [
        { provide: UserService, useValue: userSpy },
        { provide: ValidationService, useValue: validationSpy },
        { provide: HttpErrorService, useValue: errorSpy },
      ]
    })
    .compileComponents();
    httpErrorServiceSpy = TestBed.inject(HttpErrorService) as jasmine.SpyObj<HttpErrorService>;
    userServiceSpy = TestBed.inject(UserService) as jasmine.SpyObj<UserService>;
    validationServiceSpy = TestBed.inject(ValidationService) as jasmine.SpyObj<ValidationService>;
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(RegisterComponent);
    component = fixture.componentInstance;
    compiled = fixture.debugElement.nativeElement;
    firstNameInput = compiled.querySelector('#firstName');
    lastNameInput = compiled.querySelector('#lastName');
    emailInput = compiled.querySelector('#email');
    passwordInput = compiled.querySelector('#password');
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
  it('the post is created when the "Sign Up" button is pushed.', waitForAsync(() => {
    httpErrorServiceSpy.errorType = 'server_not_responding';
    firstNameInput.value = 'Test';
    firstNameInput.dispatchEvent(new Event('first name input'));
    lastNameInput.value = 'User';
    lastNameInput.dispatchEvent(new Event('last name input input'));
    emailInput.value = 'testuser@mail.com';
    emailInput.dispatchEvent(new Event('email input'));
    passwordInput.value = 'Testuser1-';
    passwordInput.dispatchEvent(new Event('password input'));
    fixture.detectChanges();
    let button = compiled.querySelector('#signup-button');
    button.click();
    fixture.whenStable().then(() => {
      fixture.detectChanges();
      expect(userServiceSpy.post.calls.count()).toBe(1);
    });
  }));
  it('first name invalid message is shown when first name input is empty.', () => {
    lastNameInput.value = 'User';
    lastNameInput.dispatchEvent(new Event('last name input input'));
    emailInput.value = 'testuser@mail.com';
    emailInput.dispatchEvent(new Event('email input'));
    passwordInput.value = 'Testuser1-';
    passwordInput.dispatchEvent(new Event('password input'));
    fixture.detectChanges();
    expect(compiled.querySelector('#invalid-firstName-message').textContent)
      .toContain('First Name is required.');
  });
  it('last name invalid message is shown when last name input is empty.', () => {
    firstNameInput.value = 'Test';
    firstNameInput.dispatchEvent(new Event('first name input'));
    emailInput.value = 'testuser@mail.com';
    emailInput.dispatchEvent(new Event('email input'));
    passwordInput.value = 'Testuser1-';
    passwordInput.dispatchEvent(new Event('password input'));
    fixture.detectChanges();
    expect(compiled.querySelector('#invalid-lastName-message').textContent)
      .toContain('Last Name is required.');
  });
  it('email invalid message is shown when email does not respect the standard.', () => {
    validationServiceSpy.validateEmail.and.returnValue(false);
    fixture.detectChanges();
    expect(compiled.querySelector('#invalid-email-message').textContent)
      .toContain('The given email does not respect the standard. (ex.: \'name@gmail.com\')');
  });
  it('invalid password message is shown when the password is not complex enough.', () => {
    validationServiceSpy.validatePassword.and.returnValue(false);
    fixture.detectChanges();
    expect(compiled.querySelector('#invalid-password-message').textContent)
      .toContain('Password must contain at least: 1 lowercase letter, 1 uppercase letter, 1 digit, 1 symbol, 8 characters.');
  });
  it('error message is shown when the email is already taken by other user.', () => {
    httpErrorServiceSpy.errorType = 'email_already_taken';
    httpErrorServiceSpy.message = 'The given email is already taken by other user.';
    fixture.detectChanges();
    expect(compiled.querySelector('#error-message').textContent)
      .toContain('The given email is already taken by other user.');
  });
  it('error message is shown when the server is not responding.', () => {
    httpErrorServiceSpy.errorType = 'server_not_responding';
    httpErrorServiceSpy.message = 'The server is not responding.';
    fixture.detectChanges();
    expect(compiled.querySelector('#error-message').textContent)
      .toContain('The server is not responding.');
  });
  it('"Already a member?" button redirects to login', () =>{
    let location: Location;
    let router: Router;
    location = TestBed.inject(Location);
      router = TestBed.inject(Router);
      fixture.detectChanges();
      spyOn(router, 'navigateByUrl');
      let button = compiled.querySelector('#login-redirect-btn');
      button.click();
      expect(router.navigateByUrl).
        toHaveBeenCalledWith(router.createUrlTree(['/login']),
          { skipLocationChange: false, replaceUrl: false, state: undefined });
  });
});
