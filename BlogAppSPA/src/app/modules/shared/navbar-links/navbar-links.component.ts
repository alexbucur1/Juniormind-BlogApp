import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-navbar-links',
  templateUrl: './navbar-links.component.html',
  styleUrls: ['./navbar-links.component.css'],
})
export class NavbarLinksComponent implements OnInit {
  @Input() path!: string;

  @Input() text!: string;

  constructor() { }

  ngOnInit(): void {
  }
}
