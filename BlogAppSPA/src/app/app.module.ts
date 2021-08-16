import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule } from '@angular/router';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LayoutComponent } from './modules/shared/layout/layout.component';
import { SharedModule } from './modules/shared/shared.module';
import { UsersModule } from './modules/users/users.module';
import { AccountModule } from './modules/account/account.module';
import { HttpErrorInterceptor } from "./services/http-interceptor.service";
import { HTTP_INTERCEPTORS } from "@angular/common/http";
import { HttpErrorService } from './services/http-error.service';

@NgModule({
  declarations: [
    AppComponent,
  ],
  imports: [
    HttpClientModule,
    BrowserModule,
    AppRoutingModule,
    SharedModule,
    UsersModule,
    AccountModule,
    RouterModule.forRoot([
      {
        path: '',
        component: LayoutComponent,
        children: [
          {
            path: '',
            loadChildren: () => import('./modules/posts/posts.module').then((m) => m.PostsModule),
          },
        ],
      },
    ]),
    NgbModule,
  ],
  providers: [ { provide: HTTP_INTERCEPTORS, useClass: HttpErrorInterceptor, multi: true }, HttpErrorService],
  bootstrap: [AppComponent],
})
export class AppModule { }
