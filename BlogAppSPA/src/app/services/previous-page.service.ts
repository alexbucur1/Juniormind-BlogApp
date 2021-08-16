import { Injectable } from '@angular/core';
import { Router, Event, NavigationEnd } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class PreviousPageService {
  currentPage = '/';
  previousPage = '';

  constructor(private router: Router) {}

  start(){
    this.router.events.subscribe((event: Event) => {
      if (event instanceof NavigationEnd) {
        let path = this.router.url;
        this.previousPage = this.currentPage;
        this.currentPage = path;
      }
    });
  }
}
