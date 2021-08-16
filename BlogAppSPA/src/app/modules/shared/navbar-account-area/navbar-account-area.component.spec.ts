import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AuthService } from 'src/app/services/auth.service';
import { NavbarAccountAreaComponent } from './navbar-account-area.component';
import { RouterTestingModule } from '@angular/router/testing';
import {Router} from '@angular/router';
import {Location} from '@angular/common';

describe('NavbarAccountAreaComponent', () => {
  let component: NavbarAccountAreaComponent;
  let fixture: ComponentFixture<NavbarAccountAreaComponent>;
  let authServiceSpy: jasmine.SpyObj<AuthService>;
  let compiled: any;
  let router: Router;
  let location: Location;

  beforeEach(async () => {
    const authSpy = jasmine.createSpyObj('AuthService', ['logout']);
    await TestBed.configureTestingModule({
      declarations: [NavbarAccountAreaComponent],
      imports: [RouterTestingModule],
      providers: [
        { provide: AuthService, useValue: authSpy },
      ]
    })
      .compileComponents();
      authServiceSpy = TestBed.inject(AuthService) as jasmine.SpyObj<AuthService>;
      router = TestBed.inject(Router);
      location = TestBed.inject(Location);
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(NavbarAccountAreaComponent);
    component = fixture.componentInstance;
    compiled = fixture.debugElement.nativeElement;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('Render account area properly when user is not logged in.', () => {
    fixture.detectChanges();
    expect(compiled.querySelector('#login-btn').value).toBe('Login');
    expect(compiled.querySelector('#user-widget')).toBeNull();
    expect(compiled.querySelector('#create-btn')).toBeNull();
  });

  it('Render account area properly when user is logged in.', () => {
    component.userId = 'bucur';
    component.userFullName = 'bucur alex';
    fixture.detectChanges();
    expect(compiled.querySelector('#login-btn')).toBeNull();
    expect(compiled.querySelector('#user-widget').textContent).toContain('bucur alex');
    expect(compiled.querySelector('#create-btn').value).toContain('Create New Post');
});

it('"Login" button redirects to login page.', () => {
  fixture.detectChanges();
  spyOn(router, 'navigateByUrl');
  let loginBtn = compiled.querySelector('#login-btn');
  loginBtn.click();
  fixture.detectChanges();
  expect(router.navigateByUrl).
    toHaveBeenCalledWith(router.createUrlTree(['/login']),
      { skipLocationChange: false, replaceUrl: false, state: undefined });
});

it('"Create New Post" button redirects to login page.', () => {
  component.userId = 'bucur';
  component.userFullName = 'bucur alex';
  fixture.detectChanges();
  spyOn(router, 'navigateByUrl');
  let createBtn = compiled.querySelector('#create-btn');
  createBtn.click();
  fixture.detectChanges();
  expect(router.navigateByUrl).
    toHaveBeenCalledWith(router.createUrlTree(['/create']),
      { skipLocationChange: false, replaceUrl: false, state: undefined });
});

it('"Settings" button redirects to user edit page.', () => {
  component.userId = 'bucur';
  component.userFullName = 'bucur alex';
  fixture.detectChanges();
  spyOn(router, 'navigateByUrl');
  let accountDropdown = compiled.querySelector('#user-widget');
  accountDropdown.click();
  let settingsBtn = compiled.querySelector('#settings-btn');
  settingsBtn.click();
  fixture.detectChanges();
  expect(router.navigateByUrl).
    toHaveBeenCalledWith(router.createUrlTree(['/users/edit/bucur']),
      { skipLocationChange: false, replaceUrl: false, state: undefined });
});

it('"Logout" button logs the user out.', () => {
  component.userId = 'bucur';
  component.userFullName = 'bucur alex';
  fixture.detectChanges();
  spyOn(router, 'navigateByUrl');
  let accountDropdown = compiled.querySelector('#user-widget');
  accountDropdown.click();
  let settingsBtn = compiled.querySelector('#logout-btn');
  settingsBtn.click();
  fixture.detectChanges();
  expect(authServiceSpy.logout.calls.count()).toEqual(1);
});
});