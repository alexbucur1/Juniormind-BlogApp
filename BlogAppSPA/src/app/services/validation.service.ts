import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ValidationService {

  constructor() { }

  public validatePassword(password: string): boolean{
    let strongPassword = new RegExp('(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[^A-Za-z0-9])(?=.{8,})')
    return strongPassword.test(password);
  }

  public validateEmail(email: string){
    return email.includes('@') && email.includes('.com');
  }
}
