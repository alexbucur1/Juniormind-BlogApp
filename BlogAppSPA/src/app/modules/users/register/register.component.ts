import { Component, OnInit } from '@angular/core';
import { UserService } from 'src/app/services/user.service';
import { User } from 'src/app/models/user.model';
import { ValidationService } from 'src/app/services/validation.service';
import { HttpErrorService } from 'src/app/services/http-error.service';
import { environment } from 'src/environments/environment';
import { Router } from '@angular/router';


@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  user: User;

  constructor(private userService: UserService,
       public validationService: ValidationService,
       public errorService: HttpErrorService,
       private router: Router) {
    this.user = {
      id: '',
      firstName: '',
      lastName: '',
      email:'',
      password: ''
    };
   }

  ngOnInit(): void {
  }

  async onSubmit(){
    await this.userService.post(this.user);
    if (!this.registerFailed()){
      this.router.navigate(['/login'])
    }
  }

  registerFailed(){
    return this.errorService.errorType == 'server_not_responding' ||
    this.errorService.errorType == 'email_already_taken';
  }
}
