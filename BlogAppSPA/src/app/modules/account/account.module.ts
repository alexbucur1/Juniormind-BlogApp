import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { SharedModule } from '../shared/shared.module';
import { RouterModule } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { LoginSuccesfullComponent } from './login-succesfull/login-succesfull.component';

@NgModule({
  declarations: [
    LoginComponent,
    LoginSuccesfullComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    RouterModule,
    FormsModule,
  ]
})
export class AccountModule { }
