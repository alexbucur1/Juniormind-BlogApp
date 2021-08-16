import { Injectable } from "@angular/core";
import { Router, Event, NavigationEnd } from '@angular/router';
import { AuthService } from "./auth.service";

@Injectable()
export class HttpErrorService {
  message = '';
  errorType = '';
  private method = '';
  private errorMessage = '';
  private status = -1;

  constructor(private router: Router, private authService: AuthService) {}

  async manageError(message: string, status: number, method: string){
    this.errorMessage = message;
    this.status = status;
    this.method = method;

    this.message = this.generateMessage();
  }

  clearErrorState(){
        this.message = '';
        this.method = '';
        this.errorType = '';
        this.errorMessage = '';
        this.status = -1;
      }

      clearErrorStateOnRouteChange(){
        this.router.events.subscribe((event: Event) => {
          if (event instanceof NavigationEnd) {
            this.clearErrorState();
          }
        });
      }

  private generateMessage(): string{
    if (this.status == 401){
      this.authService.logout();
      this.router.navigate(['/login']);
      this.errorType = "unauthorized";
      return "You have to log in first.";
  }

    if (this.status == 403 || this.errorMessage.includes('AccessDenied')){
      this.errorType = this.method == "GET" ?
      "forbidden_page" : "";
      return "You are forbidden from accessing this page.";
    }

    if (this.status == 0){
      this.errorType = this.method == "GET" && !this.errorMessage.includes('comments') ?
      "showPage_server_not_responding" :
      "server_not_responding"
      return "The server is not responding.";
    }

    return this.generateBadRequestMessage();
  }

  private generateBadRequestMessage(){
    if (this.errorMessage.includes("users")){
      return this.generateUsersMessage()
    }

    if (this.errorMessage.includes("account")){
      return this.generateAccountMessage();
    }

  return "";
  }

  private generateUsersMessage(){
    this.errorType = "email_already_taken";
    return "The given email is already taken by other user.";
  }

  private generateAccountMessage(){
    this.errorType = "wrong_email_or_password"
    return "The given password or email is wrong.";
  }
}
