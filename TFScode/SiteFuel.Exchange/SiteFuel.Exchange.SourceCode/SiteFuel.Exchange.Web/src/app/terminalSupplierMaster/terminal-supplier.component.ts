import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-terminal-supplier',
  templateUrl: './terminal-supplier.component.html',
  styleUrls: ['./terminal-supplier.component.css']
})
export class TerminalSupplierComponent implements OnInit {
  isShowCode=true;
  isShowDesc=false;
  selectedCountry=1;
  constructor() { }

  ngOnInit() {
  }

  public changeTab(id:any): void {
     if(id==2){
      this.isShowDesc=true;
      this.isShowCode=false;
     }else{
      this.isShowDesc=false;
      this.isShowCode=true; 
     }
   
 
}
}
