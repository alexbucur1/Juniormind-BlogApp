import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import { Page } from 'src/app/models/page.model';
import { User } from 'src/app/models/user.model';
import { UserService } from 'src/app/services/user.service';
import { UsersComponent } from './users.component';
import { FormsModule } from '@angular/forms';
import { RouterTestingModule } from '@angular/router/testing';
import {Router} from '@angular/router';
import {Location} from '@angular/common';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { By } from '@angular/platform-browser';
import { ModalComponent } from '../../shared/modal/modal.component';
import { MockComponent } from 'ng-mocks';

describe('UsersComponent', () => {
  let component: UsersComponent;
  let fixture: ComponentFixture<UsersComponent>;
  let userServiceSpy: jasmine.SpyObj<UserService>;
  let compiled: any;
  let router: Router;
  let location: Location;

  beforeEach(async () => {
    const userSpy = jasmine.createSpyObj('UserService', ['getAll', 'delete']);
    await TestBed.configureTestingModule({
      declarations: [UsersComponent, MockComponent(ModalComponent)],
      imports: [FormsModule, RouterTestingModule, NgbModule],
      providers:[
        { provide: UserService, useValue: userSpy },
      ]
    })
      .compileComponents();
      userServiceSpy = TestBed.inject(UserService) as jasmine.SpyObj<UserService>;
      router = TestBed.inject(Router);
      location = TestBed.inject(Location);
  });

  beforeEach(() => {
    const users = [
      {
        id: '1',
        firstName: 'user1',
        lastName: 'user1',
        email:'user1@mail.com',
        password: 'User111-'
      },
      {
        id: '2',
        firstName: 'user2',
        lastName: 'user2',
        email:'user2@mail.com',
        password: 'User222-'
      },
      {
        id: '3',
        firstName: 'user3',
        lastName: 'user3',
        email:'user3@mail.com',
        password: 'User333-'
      }
    ];

    const page = {
      hasNextPage: true,
      hasPreviousPage: true,
      pageIndex: 2,
      items: users,
      pageSize: 5,
    };

    const pageAsPromise = new Promise<Page<User>>((resolve) => {
      resolve(page);
    });
    userServiceSpy.getAll.and.returnValue(pageAsPromise);
    fixture = TestBed.createComponent(UsersComponent);
    component = fixture.componentInstance;
    compiled = fixture.debugElement.nativeElement;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
  it('users are rendered on page.', waitForAsync(() => {
    fixture.whenStable().then(() => {
      fixture.detectChanges();
      expect(compiled.querySelector('#firstName-1').textContent).toContain('user1');
      expect(compiled.querySelector('#firstName-2').textContent).toContain('user2');
      expect(compiled.querySelector('#firstName-3').textContent).toContain('user3');
      expect(compiled.querySelector('#lastName-1').textContent).toContain('user1');
      expect(compiled.querySelector('#lastName-2').textContent).toContain('user2');
      expect(compiled.querySelector('#lastName-3').textContent).toContain('user3');
      expect(compiled.querySelector('#email-1').textContent).toContain('user1@mail.com');
      expect(compiled.querySelector('#email-2').textContent).toContain('user2@mail.com');
      expect(compiled.querySelector('#email-3').textContent).toContain('user3@mail.com');
    });
  }));
  it('"options" button toggles Edit And Delete Links', waitForAsync(() => {
    fixture.whenStable().then(() => {
      fixture.detectChanges();
      let button = compiled.querySelector('#options-1');
      button.click();
      expect(compiled.querySelector('#edit-user-1').textContent).
        toContain("Edit");
        expect(compiled.querySelector('#delete-user-1').textContent).
        toContain("Delete");
    });
  }));
  it('"Edit" link should redirect to specified user edit page.', waitForAsync(() => {
    fixture.whenStable().then(() => {
      fixture.detectChanges();
      spyOn(router, 'navigateByUrl');
      let optionsBtn = compiled.querySelector('#options-1');
      optionsBtn.click();
      fixture.detectChanges();
      let editBtn = compiled.querySelector('#edit-user-1');
      editBtn.click();
      expect(router.navigateByUrl).
        toHaveBeenCalledWith(router.createUrlTree(['/users/edit/1']),
          { skipLocationChange: false, replaceUrl: false, state: undefined });
    });
  }));
  it('"Delete" button should open modal component.', waitForAsync(() => {
    fixture.whenStable().then(() => {
      fixture.detectChanges();
      spyOn(router, 'navigateByUrl');
      let optionsBtn = compiled.querySelector('#options-1');
      optionsBtn.click();
      fixture.detectChanges();
      let editBtn = compiled.querySelector('#delete-user-1');
      editBtn.click();
      fixture.detectChanges();
      expect(fixture.debugElement
        .queryAll(By.directive(ModalComponent))
        .map(el => el.componentInstance)[0].type).toBe("delete");
    });
  }));
  it('GetAll should be called when "Previous" button is pushed.', waitForAsync(() => {
    fixture.whenStable().then(() => {
      fixture.detectChanges();
      let button = compiled.querySelector('#previous-btn');
      button.click();
      fixture.whenStable().then(() => {
        fixture.detectChanges();
        expect(userServiceSpy.getAll.calls.count()).toEqual(2);
       })
    });
  }));
  it('GetAll should be called when "Next" button is pushed.', waitForAsync(() => {
    fixture.whenStable().then(() => {
      fixture.detectChanges();
      let button = compiled.querySelector('#next-btn');
      button.click();
      fixture.whenStable().then(() => {
        fixture.detectChanges();
        expect(userServiceSpy.getAll.calls.count()).toEqual(2);
       })
    });
  }));
});
