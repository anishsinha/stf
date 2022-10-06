import { Component, OnInit, OnDestroy, ViewChild } from '@angular/core';
import { TractorService } from '../service/tractor.service';
import { TruckDetailsModel,TractorDetailsModel, TrailerFuelRetainModel } from '../model';
import { Subject } from 'rxjs';
import { CreateTractorComponent } from '../create-tractor/create-tractor.component';
import { Declarations } from 'src/app/declarations.module';
import { CarrierService } from 'src/app/carrier/service/carrier.service';
import { CreateTrailerComponent } from 'src/app/shared-components/create-trailer/create-trailer.component';
import { LicenceRequirement, TractorStatus, TrailerType, TruckStatus } from 'src/app/app.enum';

@Component({
    selector: 'app-view-tractor',
    templateUrl: './view-tractor.component.html',
    styleUrls: ['./view-tractor.component.css']
})

export class ViewTractorComponent implements OnInit, OnDestroy {
    //public IsCreateTruck: boolean;
    public Tractors: TractorDetailsModel[];
    public Trucks: TruckDetailsModel[];
    public ModalText: string;
    public ModalTextTrailer: string;
    public TruckStatus: typeof TruckStatus = TruckStatus;
    public TractorStatus: typeof TractorStatus = TractorStatus;
    public TrailerType: typeof TrailerType = TrailerType;
    public LicenceRequirements: typeof LicenceRequirement = LicenceRequirement;
    public popoverTitle: string = 'Are you sure?';
    public confirmButtonText: string = 'Yes';
    public cancelButtonText: string = 'No';
    public IsLoading: boolean = false;
    dtOptions: any = {};
    dtOptionsTrailer: any = {};
    dtTrigger: Subject<any> = new Subject();
    dtTriggerTrailer: Subject<any> = new Subject();
    public CompartmentDetails: TrailerFuelRetainModel[];
    constructor(private tractorService: TractorService, private carrierService: CarrierService) { }
    @ViewChild(CreateTractorComponent) TractorComponent: CreateTractorComponent;
    @ViewChild(CreateTrailerComponent) TrailerComponent: CreateTrailerComponent;

    ngOnInit() {
        this.intializeTrailer();
        this.intializeTractors();
    }

    intializeTrailer() {
        this.IsLoading = true;
        this.ModalTextTrailer = 'Create Trailer'
        let exportColumnsTrailer = { columns: ':visible' };
        this.dtOptionsTrailer = {
            dom: '<"html5buttons"B>lTfgitp',
            buttons: [
                { extend: 'colvis' },
                { extend: 'copy', exportOptions: exportColumnsTrailer },
                { extend: 'csv', title: 'Trailer Details', exportOptions: exportColumnsTrailer },
                { extend: 'pdf', title: 'Trailer Details', orientation: 'landscape', exportOptions: exportColumnsTrailer },
                { extend: 'print', exportOptions: exportColumnsTrailer }
            ],
            pagingType: 'first_last_numbers',
            pageLength: 10,
            lengthMenu: [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]],
            "order": [],
        };
        this.getAllTrucks();
        this.dtTriggerTrailer.next();
    }
    intializeTractors() {
        this.IsLoading = true;
        this.ModalText = 'Create Tractor';
        let exportColumns = { columns: ':visible' };
        this.dtOptions = {
            dom: '<"html5buttons"B>lTfgitp',
            buttons: [
                { extend: 'colvis' },
                { extend: 'copy', exportOptions: exportColumns },
                { extend: 'csv', title: 'Tractor Details', exportOptions: exportColumns },
                { extend: 'pdf', title: 'Tractor Details', orientation: 'landscape', exportOptions: exportColumns },
                { extend: 'print', exportOptions: exportColumns }
            ],
            pagingType: 'first_last_numbers',
            pageLength: 10,
            lengthMenu: [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]],
        };
        this.getAllTractor();
    }
    ngOnDestroy(): void {
        this.dtTrigger.unsubscribe();
        this.dtTriggerTrailer.unsubscribe();
    }

    editTractor(tractor) {
        if (this.TractorComponent != undefined) {
            this.TractorComponent.loadTractorDetail(tractor);
        }
    }
    editTruck(truck) {
        if (this.TrailerComponent != undefined) {
            this.TrailerComponent.loadTruckDetail(truck);
        }
    }
    deleteTractor(tractor) {
        this.tractorService.postDeleteTractor(tractor).subscribe(data => {
            if (data.StatusCode == 0) {
                Declarations.msgsuccess(data.StatusMessage, undefined, undefined);
                this.loadTractorDetails();
            }
        });
    }
    deleteTruck(truck) {
        this.carrierService.postDeleteTruck(truck).subscribe(data => {
            if (data.StatusCode == 0) {
                Declarations.msgsuccess(data.StatusMessage, undefined, undefined);
                this.loadTruckDetails();
            }
        });
    }
    createTractor(header: string) {
        this.ModalText = header;
        //this.IsCreateTruck = true;
    }
    createTrailer(header: string) {
        this.ModalTextTrailer = header;
        //this.IsCreateTruck = true;
    }
    getAllTractor() {
        this.tractorService.getAllTractors().subscribe(data => {
            this.IsLoading = false;
            this.Tractors = data;
            this.dtTrigger.next();

        });
    }
    getAllTrucks() {
        this.carrierService.getAllTrucks().subscribe(data => {
            this.IsLoading = false;
            this.Trucks = data;
            this.dtTriggerTrailer.next();
        });
    }
    clearPanelData() {
        if (this.TractorComponent != undefined) {
            this.TractorComponent.clearTractorForm();
        }
    }
    clearPanelDataTrailer() {
        if (this.TrailerComponent != undefined) {
            this.TrailerComponent.clearTrailerForm();
        }
    }
    loadTractorDetails() {
        this.getAllTractor();
        $("#tractor-datatable").DataTable().clear().destroy();
    }
    loadTruckDetails() {
        this.getAllTrucks();
        $("#truck-datatable").DataTable().clear().destroy();
    }
    OpenTrailerDetails(Id: string) {
        this.CompartmentDetails = [];
        var trucksDetail = this.Trucks.find(top => top.Id == Id);
        if (trucksDetail != null) {
            this.CompartmentDetails = trucksDetail.TrailerFuelRetains;
        }
        $("#btnconfirm-trailerInfo").click();
    }
}
