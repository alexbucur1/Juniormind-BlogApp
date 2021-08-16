import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NavbarComponent } from './navbar/navbar.component';
import { AppRoutingModule } from '../../app-routing.module';
import { LayoutComponent } from './layout/layout.component';
import { NavbarLinksComponent } from './navbar-links/navbar-links.component';
import { LoadingSpinnerComponent } from './loading-spinner/loading-spinner.component';
import { NavbarAccountAreaComponent } from './navbar-account-area/navbar-account-area.component';
import { ModalComponent } from './modal/modal.component';
import { PageNotFoundComponent } from './page-not-found/page-not-found.component';
import { ErrorPageComponent } from './error-page/error-page.component';

@NgModule({
  declarations: [
    NavbarComponent,
    LayoutComponent,
    NavbarLinksComponent,
    LoadingSpinnerComponent,
    NavbarAccountAreaComponent,
    ModalComponent,
    PageNotFoundComponent,
    ErrorPageComponent
  ],
  imports: [
    CommonModule,
    AppRoutingModule,
    NgbModule,
  ],
  exports: [
    LayoutComponent,
    LoadingSpinnerComponent,
    ModalComponent,
    ErrorPageComponent
  ],
})
export class SharedModule { }
