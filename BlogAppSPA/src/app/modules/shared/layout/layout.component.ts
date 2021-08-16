import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';
import { Router, Event, NavigationEnd } from '@angular/router';
import { HttpErrorService } from 'src/app/services/http-error.service';

@Component({
  selector: 'app-layout',
  templateUrl: './layout.component.html',
  styleUrls: ['./layout.component.css'],
})
export class LayoutComponent implements OnInit {
  isAuthPage = false;
  constructor(public authService: AuthService, private router: Router,public errorService: HttpErrorService) { }

  ngOnInit(){
    this.errorService.clearErrorStateOnRouteChange();
    this.router.events.subscribe((event: Event) => {
      if (event instanceof NavigationEnd) {
        let path = this.router.url;
        this.isAuthPage = path.includes('login') || path.includes('register');
      }
    });
  }

  showErrorPage(){
    return this.errorService.errorType == 'showPage_server_not_responding' ||
    this.errorService.errorType == 'forbidden_page';
  }

  hideHeaderAndFooter(){
    return this.isAuthPage || this.showErrorPage();
  }
}