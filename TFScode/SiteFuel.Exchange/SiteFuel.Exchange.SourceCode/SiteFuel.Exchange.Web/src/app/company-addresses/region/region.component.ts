import { Component, OnInit } from '@angular/core';

@Component({
    selector: 'region-component',
    templateUrl: './region.component.html',
    styleUrls: ['./region.component.css']
})
export class RegionComponent implements OnInit {
    viewType = 1;
    constructor() { }
  
    ngOnInit() {
    }
    public changeViewType(val): void {
        this.viewType = val;
    }
  
  }
