import { Component, OnInit} from '@angular/core';
import { RegionService } from '../../company-addresses/region/service/region.service';

@Component({
  selector: 'app-driver',
  templateUrl: './driver.component.html',
  styleUrls: ['./driver.component.css']
})
export class DriverComponent implements OnInit {
    
    isDriverShow = true;
    isProfileShow=false;
    constructor(public regionService: RegionService) { }

  ngOnInit() {
  }

    public changeTab(tabClick:string): void {
      this.isDriverShow = false;
      this. isProfileShow=false;
       if(tabClick==="DriverShow")
       {
        this.isDriverShow = true;
       }
       if(tabClick=="ProfileShow")
       {
        this.isProfileShow = true;
       }
    }
}
