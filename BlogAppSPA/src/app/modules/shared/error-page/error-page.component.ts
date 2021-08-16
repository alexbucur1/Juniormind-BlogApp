import { Component, OnInit } from '@angular/core';
import { HttpErrorService } from 'src/app/services/http-error.service';
import { Location } from '@angular/common';
import { PreviousPageService } from 'src/app/services/previous-page.service';

@Component({
  selector: 'app-error-page',
  templateUrl: './error-page.component.html',
  styleUrls: ['./error-page.component.css']
})
export class ErrorPageComponent implements OnInit {
  title = '';
  currentPage = '';
  previousPage = '';
  constructor(public errorService: HttpErrorService,
     private location: Location,
      public previousPageService: PreviousPageService) { }

  ngOnInit(): void {
    this.currentPage = this.previousPageService.currentPage;
    this.previousPage = this.previousPageService.previousPage
    this.title = this.errorService.errorType == 'forbidden_page' ? 'Forbidden.' : 'Server Error.';
  }

  refresh(){
    this.errorService.clearErrorState();
    this.location.go(this.currentPage);
  }
}
