import { Component, OnInit } from '@angular/core';
import { UserService } from 'src/app/services/user.service';
import { ValidationService } from 'src/app/services/validation.service';
import { User } from 'src/app/models/user.model';
import { PreviousPageService } from 'src/app/services/previous-page.service';
import { HttpErrorService } from 'src/app/services/http-error.service';
import { ActivatedRoute } from '@angular/router';
import { Router } from '@angular/router';

@Component({
  selector: 'app-edit-users',
  templateUrl: './edit-users.component.html',
  styleUrls: ['./edit-users.component.css']
})
export class EditUsersComponent implements OnInit {
  isLoading = false;
  user: User
  id: any = '';

  constructor(private userService: UserService,
     private router: Router,
     public validationService: ValidationService,
     public errorService: HttpErrorService,
     private route: ActivatedRoute,
     private previousPageService: PreviousPageService) {
    this.user = {
      id: '',
      firstName: '',
      lastName: '',
      email:'',
      password: ''
    }
   }

  ngOnInit(): void {
  this.isLoading = true;
  this.route.paramMap.subscribe((params) => {
    this.id = params.get('id');
    this.get(this.id);
  });
  }

  async get(id: string): Promise<void>{
    this.user = await this.userService.get(id);
    this.isLoading = false;
  }

  async onSubmit(){
    await this.userService.put(this.user);
    if (!this.editFailed()){
      this.goBack();
    }
  }

  goBack(){
    this.router.navigate([this.previousPageService.previousPage]);
  }

  validatePassword(password: string): boolean{
  return this.validationService.validatePassword(password);
  }

  editFailed(){
    return this.errorService.errorType == "email_already_taken" ||
    this.errorService.errorType == 'server_not_responding';
  }

}
