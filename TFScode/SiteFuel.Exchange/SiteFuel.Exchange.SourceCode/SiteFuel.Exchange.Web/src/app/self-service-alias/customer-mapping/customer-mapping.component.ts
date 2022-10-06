import { Component, OnInit, ViewChild, Input, EventEmitter } from '@angular/core';
import { Subject } from 'rxjs';
import { CustomerDetailsViewModel } from 'src/app/carrier/models/CustomerDetailsViewModel';
import { CarrierService } from 'src/app/carrier/service/carrier.service';

@Component({
  selector: 'app-customer-mapping',
  templateUrl: './customer-mapping.component.html',
  styleUrls: ['./customer-mapping.component.css']
})
export class CustomerMappingComponent implements OnInit {
   
    isShow = false;
    isShowLoader = false;
    dtOptions: any = {};
    dtTrigger: Subject<any> = new Subject();
    CustomerDataToBeSend: CustomerDetailsViewModel; 
    public isShowCountryDDL: boolean = false;
    public CarrierCustomers: CustomerDetailsViewModel[] = [];
    public HeaderText: string;
    public IsLoading: boolean = false;
    
    constructor(private carrierService: CarrierService) {
       
        };

    ngOnInit() {
        this.initializeCarrierCustomers();
        this.getAllCustomerData();
        this.isShowCountryDDL = false;
  }
    getDriverDetails() {
        
        $("#customer-grid-datatable").DataTable().clear().destroy();
    }

    getAllCustomerData() {
        this.isShowLoader = true;
        this.carrierService.getAllCustomerData().subscribe(data => {
            this.CarrierCustomers = data;
            this.isShowLoader = false;
            $("#customer-grid-datatable").DataTable().clear().destroy();
            this.dtTrigger.next();
        });
    }


    setPanelHeader(headerText: string) {
        this.HeaderText = headerText;
    }


    editDriver(Customer: any)
    {
        this.CustomerDataToBeSend = JSON.parse(JSON.stringify(Customer));
    }

    clearForm() {
        $("#customer-grid-datatable").DataTable().clear().destroy();
        this.getAllCustomerData();
    }

    initializeCarrierCustomers() {
        let exportInvitedColumns = { columns: ':visible' };
        this.dtOptions = {
            dom: '<"html5buttons"B>lTfgitp',
            buttons: [
                { extend: 'colvis' },
                { extend: 'copy', exportOptions: exportInvitedColumns },
                { extend: 'csv', title: 'Customer Details', exportOptions: exportInvitedColumns },
                { extend: 'pdf', title: 'Customer Details', orientation: 'landscape', exportOptions: exportInvitedColumns },
                { extend: 'print', exportOptions: exportInvitedColumns }
            ],
            pagingType: 'first_last_numbers',
            pageLength: 10,
            lengthMenu: [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]]
        };
    }

}
