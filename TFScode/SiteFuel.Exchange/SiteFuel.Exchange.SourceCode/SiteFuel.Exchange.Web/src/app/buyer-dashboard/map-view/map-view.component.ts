import { Component, Input, OnInit, ViewEncapsulation } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-map-view',
  templateUrl: './map-view.component.html',
  styleUrls: ['./map-view.component.scss'],
  encapsulation: ViewEncapsulation.None,
})
export class MapViewComponent implements OnInit {
  @Input() SelectedCountryId: any
  public viewType = 1;
  constructor(private router:Router) { }

  ngOnInit() {
  }
  public changeViewType(type: any): void {
    localStorage.setItem('viewType', <string>type);
    this.viewType = type;
  }

  public navigate(): void {
    this.router.navigate([]).then(result => {  window.open('/Buyer/Job/BuyerWallyBoard?viewTypeFromDashboard='+ this.viewType, '_blank'); });
  }
}
