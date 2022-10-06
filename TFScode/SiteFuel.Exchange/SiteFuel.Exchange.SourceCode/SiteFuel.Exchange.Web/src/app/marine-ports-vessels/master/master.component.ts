import { Component, OnInit } from '@angular/core';
import { MarinePortsandvesselsService } from '../marine-portsandvessels.service';
import { DropdownItem } from 'src/app/statelist.service';
import { Country } from 'src/app/self-service-alias/models/location';
declare var currentCompanyId: any;
declare var defaultCountryId :any;
@Component({
  selector: 'app-master',
  templateUrl: './master.component.html',
  styleUrls: ['./master.component.css']
})
export class MasterComponent implements OnInit {
    viewType = 1;
    //country selection related variables
    CountryList: DropdownItem[] = [];
    public SelectedCountryId: number;

    IsLoading = false;
    constructor(private marineService: MarinePortsandvesselsService) { }

   ngOnInit(): void {
        this.getCountries();
        this.SelectedCountryId = Country.USA;
   }
    public changeViewType(value) {
        this.viewType = value;
    }
    private getCountries() {
        this.marineService.GetAllCountries().subscribe((data)=>{
         this.CountryList = data;
        });
    }

    public onCountryChange(event) {
      this.SelectedCountryId = event.target.value;
    }

}
