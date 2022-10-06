import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { InvitationComponent } from './invitation.component';
import { LeftMenuComponent } from './left-menu/left-menu.component'
import { RouterModule, Routes } from '@angular/router';
import { SharedModule } from '../modules/shared.module';
import { DirectiveModule } from '../modules/directive.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { InvitationSubmitComponent } from './invitation-submit/invitation-submit.component';
import { VisibilityChangeModule } from 'src/app/directives/visibility-change.module';

const routeInv: Routes = [
  {
    path: "",
    component: InvitationComponent
  },
  {
    path: "/Index",
    component: InvitationComponent
  },
  {
    path: "/Submit",
    component: InvitationSubmitComponent
  }
];

@NgModule({
  declarations: [
    InvitationComponent,
    LeftMenuComponent,
    InvitationSubmitComponent
  ],

  imports: [
    CommonModule,
    DirectiveModule,
    SharedModule,
    FormsModule,
    ReactiveFormsModule,
    VisibilityChangeModule,
    RouterModule.forChild(routeInv)
  ]
})
export class InvitationModule { }
