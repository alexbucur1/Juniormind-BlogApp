import { Component, OnInit, Input } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-navbar-account-area',
  templateUrl: './navbar-account-area.component.html',
  styleUrls: ['./navbar-account-area.component.css'],
})
export class NavbarAccountAreaComponent implements OnInit {
  @Input() userId = '';
  @Input() userFullName = '';
  constructor(public authService: AuthService) { }

  ngOnInit(): void {
  }

  logout(){
    this.authService.logout();
  }
}
