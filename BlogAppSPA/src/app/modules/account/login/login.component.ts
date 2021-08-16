import { Component, OnInit } from '@angular/core';
import { PreviousPageService } from '../../../../app/services/previous-page.service';


@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  constructor(public previousPageService: PreviousPageService,) {  }

   ngOnInit(){
     document.location.href=`http://localhost:3001/login?callBackUrl=${this.previousPageService.previousPage}`;
  }}
