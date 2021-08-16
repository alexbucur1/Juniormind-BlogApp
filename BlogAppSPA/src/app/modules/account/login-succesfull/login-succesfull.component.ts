import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../../services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login-succesfull',
  templateUrl: './login-succesfull.component.html',
  styleUrls: ['./login-succesfull.component.css']
})
export class LoginSuccesfullComponent implements OnInit {

  constructor(
    private authService: AuthService,
    private router: Router) { }

  async ngOnInit() {
    await this.authService.signin();
    this.router.navigate([this.authService.userClient.callBackUrl]);
  }

}
