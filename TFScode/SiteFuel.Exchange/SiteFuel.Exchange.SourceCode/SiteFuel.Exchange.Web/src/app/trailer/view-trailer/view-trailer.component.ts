import { Component, OnInit, OnDestroy, ViewChild } from '@angular/core';
import { Subject } from 'rxjs';
import { Declarations } from '../../declarations.module';
import { CarrierService } from 'src/app/carrier/service/carrier.service';
import { TruckDetailsModel } from 'src/app/carrier/model';
import { CreateTrailerComponent } from 'src/app/shared-components/create-trailer/create-trailer.component';
import { LicenceRequirement, TrailerType, TruckStatus } from 'src/app/app.enum';

@Component({
    selector: 'app-view-trailer',
    templateUrl: './view-trailer.component.html',
    styleUrls: ['./view-trailer.component.css']
})

export class ViewTrailerComponent implements OnInit, OnDestroy {
    //public IsCreateTruck: boolean;
    public Trucks: TruckDetailsModel[];
    public ModalText: string;
    public TruckStatus: typeof TruckStatus = TruckStatus;
    public TrailerType: typeof TrailerType = TrailerType;
    public LicenceRequirements: typeof LicenceRequirement = LicenceRequirement;

    public popoverTitle: string = 'Are you sure?';
    public confirmButtonText: string = 'Yes';
    public cancelButtonText: string = 'No';

    dtOptions: any = {};
    dtTrigger: Subject<any> = new Subject();
    constructor(private carrierService: CarrierService) { }
    @ViewChild(CreateTrailerComponent) TrailerComponent: CreateTrailerComponent;

    ngOnInit() {
        this.ModalText = 'Create Trailer';
        var exportColumns = { columns: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10] };
        this.dtOptions = {
            dom: '<"html5buttons"B>lTfgitp',
            buttons: [
                { extend: 'colvis' },
                { extend: 'copy', exportOptions: exportColumns },
                { extend: 'csv', title: 'Trailer Details', exportOptions: exportColumns },
                { extend: 'pdf', title: 'Trailer Details', orientation: 'landscape', exportOptions: exportColumns },
                { extend: 'print', exportOptions: exportColumns }
            ],
            pagingType: 'first_last_numbers',
            pageLength: 10,
            ordering: false,
            lengthMenu: [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]]
        };
        this.getAllTrucks();
    }

    ngOnDestroy(): void {
        this.dtTrigger.unsubscribe();
    }
    editTruck(truck) {
        if (this.TrailerComponent != undefined) {
            this.TrailerComponent.loadTruckDetail(truck);
        }
    }
    deleteTruck(truck) {
        this.carrierService.postDeleteTruck(truck).subscribe(data => {
            if (data.StatusCode == 0) {
                Declarations.msgsuccess(data.StatusMessage, undefined, undefined);
                this.loadTruckDetails();
            }
        });
    }
    createTrailer(header: string) {
        this.ModalText = header;
        //this.IsCreateTruck = true;
    }

    getAllTrucks() {
        this.carrierService.getAllTrucks().subscribe(data => {
            this.Trucks = data;
            this.dtTrigger.next();
        });
    }
    clearPanelData() {
        if (this.TrailerComponent != undefined) {
            this.TrailerComponent.clearTrailerForm();
        }
    }
    loadTruckDetails() {
        this.getAllTrucks();
        $("#truck-datatable").DataTable().clear().destroy();
    }
}
