import { Component, OnInit ,Input, OnChanges, SimpleChanges} from '@angular/core';
import { MarineVesselsModel, MarinePortModel } from '../models';
import { Subject } from 'rxjs';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MarinePortsandvesselsService } from '../marine-portsandvessels.service';
import { ConfirmationDialogService } from 'src/app/shared-components/confirmation-dialog/confirmation-dialog.service';
import { Declarations } from 'src/app/declarations.module';

@Component({
  selector: 'app-marinevessels',
  templateUrl: './marinevessels.component.html',
  styleUrls: ['./marinevessels.component.css']
})
export class MarinevesselsComponent implements OnInit, OnChanges{
    @Input() SelectedCountryId: any;

    public ModalText: string;

    public MarineVesselsData: MarineVesselsModel[] = [];
    public MarineVessel: MarineVesselsModel = new MarineVesselsModel();
    dtOptions: any = {};
    dtTrigger: Subject<any> = new Subject();

    public popoverTitle: string = 'Are you sure?';
    public confirmButtonText: string = 'Yes';
    public cancelButtonText: string = 'No';

    vesselCreateForm: FormGroup;


    public IsLoading: boolean = false;

    constructor(private marineService: MarinePortsandvesselsService, private fb: FormBuilder,
        private confirmationdialogueservice: ConfirmationDialogService) { }
    ngOnInit(): void {
        this.ModalText = 'Create Vessel';
        var exportColumns = { columns: [0, 1, 2] };
        this.dtOptions = {
            dom: '<"html5buttons"B>lTfgitp',
            buttons: [
                { extend: 'colvis' },
                { extend: 'copy', exportOptions: exportColumns },
                { extend: 'csv', title: 'Vessel Details', exportOptions: exportColumns },
                { extend: 'pdf', title: 'Vessel Details', orientation: 'landscape', exportOptions: exportColumns },
                { extend: 'print', exportOptions: exportColumns }
            ],
            pagingType: 'first_last_numbers',
            pageLength: 10,
            ordering: true,
            lengthMenu: [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]]
        };

        this.MarineVessel.CountryId = this.SelectedCountryId;

        this.initializeVesselCreationForm(this.MarineVessel);
        this.getMarineVesselsData();
    }
    ngOnChanges(changes: SimpleChanges): void {
        if (changes.SelectedCountryId && changes.SelectedCountryId.currentValue) {
            // get call for grid data
           // this.getMarineVesselsData();            
        }
    }
    createVessel(header: string) {
        this.ModalText = header;
        this.vesselCreateForm.get('CountryId').setValue(this.SelectedCountryId);
    }

    getMarineVesselsData() {
        let countryId = this.SelectedCountryId;
        this.IsLoading = true;
        this.marineService.getMarineVessels(countryId).subscribe((data) => {
            if (data) {
                jQuery("#vessels-datatable").DataTable().clear().destroy();
                console.log(data);
                this.MarineVesselsData = data;
                this.dtTrigger.next();
                this.IsLoading = false;
            }

        });
    }
    initializeVesselCreationForm(vessel: MarineVesselsModel): FormGroup {
        this.vesselCreateForm = this.fb.group({
            Id: this.fb.control(vessel.Id),
            Name: this.fb.control(vessel.Name, [Validators.required]),
            IMONumber: this.fb.control(vessel.IMONumber),
            Flag: this.fb.control(vessel.Flag),
            CountryId: this.fb.control(vessel.CountryId)
            //Customers: this.fb.control(vessel.Customers),            
        })
        return this.vesselCreateForm;
    }
    editVessel(vessel: MarineVesselsModel) {
        if (vessel && vessel.Id > 0) {
            this.vesselCreateForm.get('Name').setValue(vessel.Name);
            this.vesselCreateForm.get('Id').setValue(vessel.Id);
            this.vesselCreateForm.get('CountryId').setValue(vessel.CountryId);
            this.vesselCreateForm.get('IMONumber').setValue(vessel.IMONumber);
            this.vesselCreateForm.get('Flag').setValue(vessel.Flag);
            //this.vesselCreateForm.get('Customers').setValue(vessel.Customers);          
        }
    }
    deleteVessel(vessel) {
        if (vessel) {
            this.marineService.deleteVessel(vessel.Id).subscribe(data => {
                if (data != null && data.StatusCode == 0) {
                    Declarations.msgsuccess(data.StatusMessage, undefined, undefined);
                    this.getMarineVesselsData();
                }
                else {
                    Declarations.msgerror(data == null ? 'Failed' : data.StatusMessage, undefined, undefined);
                }
            });
        }
    }
    
    clearPanelData() {
        this.vesselCreateForm.reset();
        this.vesselCreateForm.get('CountryId').setValue(this.SelectedCountryId);
    }
   
    
    ngOnDestroy(): void {
        this.dtTrigger.unsubscribe();
    }

    isInvalid(name: string): boolean {
        var result = this.vesselCreateForm.get(name).invalid
            &&
            (
            this.vesselCreateForm.get(name).dirty
                ||
            this.vesselCreateForm.get(name).touched
            )
        return result;
    }

    isRequired(name: string): boolean {
        return this.vesselCreateForm.get(name).errors.required;
    }

    onSubmit() {
        this.vesselCreateForm.markAllAsTouched();
        if (this.vesselCreateForm.valid) {
            this.IsLoading = true;
            // serverside api to save port
            let port = this.vesselCreateForm.value;
            this.marineService.saveMarineVessel(port).subscribe(data => {
                if (data != null && data.StatusCode == 0) {
                    Declarations.msgsuccess(data.StatusMessage, undefined, undefined);
                    this.IsLoading = false;
                    this.vesselCreateForm.reset();
                    this.clearPanelData();
                    let dismissSlider = document.getElementById('btnCancel') as HTMLElement;
                    dismissSlider.click();
                    this.getMarineVesselsData();
                } else {
                    this.IsLoading = false;
                    Declarations.msgerror(data == null ? 'Failed' : data.StatusMessage, undefined, undefined);
                }
            });
        }
        else {
            return;
        }
    }
}
