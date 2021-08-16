import { Component, OnInit, ViewChild } from '@angular/core';
import { UserService } from 'src/app/services/user.service';
import { ModalComponent } from '../../shared/modal/modal.component';
import { Page } from 'src/app/models/page.model';
import { User } from 'src/app/models/user.model';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.css'],
})
export class UsersComponent implements OnInit {
  users: Page<User>
  isLoading = false;
  sortOrder = '';
  search = '';
  @ViewChild(ModalComponent) modal!: ModalComponent;
  constructor(private userService: UserService) {
    this.users = {
      hasNextPage: false,
      hasPreviousPage: false,
      pageIndex: 1,
      items: [],
      pageSize: 0,
    };
   }

  ngOnInit(): void {
    this.isLoading = true;
    this.getAll();
  }

  async getAll(page: number = 1): Promise<void> {
    this.users =  await this.userService.getAll(this.sortOrder, page, this.search);
    this.isLoading = false;
  }

  async delete(id: string) {
    const response = await this.modal.open();

    if (response) {
      await this.userService.delete(id);
      await this.getAll();
    }
  }

  sortByFirstName(){
    if (this.sortOrder == 'firstName_desc'){
      this.sortOrder = 'firstName';
    }
    else{
      this.sortOrder  ='firstName_desc'
    }
    this.isLoading = true;
    this.getAll();
  }

  sortByLastName(){
    if (this.sortOrder == 'lastName_desc'){
      this.sortOrder = 'lastName';
    }
    else{
      this.sortOrder  ='lastName_desc'
    }
    this.isLoading = true;
    this.getAll();
  }

  sortByEmail(){
    if (this.sortOrder == 'email_desc'){
      this.sortOrder = 'email';
    }
    else{
      this.sortOrder  ='email_desc'
    }
    this.isLoading = true;
    this.getAll();
  }

  searchUser(){
    this.isLoading = true;
    this.getAll();
  }
}
