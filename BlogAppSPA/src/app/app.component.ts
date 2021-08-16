import { Component, OnInit} from '@angular/core';
import { PreviousPageService } from './services/previous-page.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent implements OnInit{
  title = 'BlogAppSPA';
  constructor(public previousPageService: PreviousPageService){
  }

  ngOnInit(){
    this.previousPageService.start();
  }
}
