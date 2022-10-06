import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common'
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { SidebarModule } from 'ng-sidebar';


@NgModule({
    declarations: [],
    imports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule.withConfig({ warnOnNgModelWithFormControl: 'never' }),
        HttpClientModule,
        SidebarModule.forRoot()
    ],
    exports: [

        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        HttpClientModule,
        SidebarModule,
    ]
})

export class GlobalModule { }
