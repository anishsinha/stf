import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { SidebarComponent } from './sidebar/sidebar.component';
import { GlobalModule } from './modules/global.module';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { MarinePortsVesselsModule } from './marine-ports-vessels/marine-ports-vessels.module';


@NgModule({
    declarations: [
        AppComponent,
        SidebarComponent,
    ],
    imports: [
        BrowserModule,
        AppRoutingModule,
        BrowserAnimationsModule,
        //modules for lazy loading
        GlobalModule,
        NgbModule,
        MarinePortsVesselsModule
    ],

    providers: [],
    bootstrap: [AppComponent]
})
export class AppModule { }

