import { Component, OnInit, ViewChild, ElementRef, Input, AfterViewInit, OnChanges, SimpleChanges } from '@angular/core';
import { DataTableDirective } from 'angular-datatables';
import { Subject, from, forkJoin } from 'rxjs';
import { first, map, mergeMap } from 'rxjs/operators';
import { Declarations } from 'src/app/declarations.module';
import { HttpClient } from '@angular/common/http';
import { CarrierService } from 'src/app/carrier/service/carrier.service';
import { DropdownCustomItem, DropdownItem } from '../../statelist.service';
import { FuelSurchargeService } from '../../fuelsurcharge/services/fuelsurcharge.service';
import { RegionService } from 'src/app/company-addresses/region/service/region.service';
import { FuelGroupType, TableType } from 'src/app/app.enum';
import { ExternalMappingsService } from 'src/app/self-service-alias/service/externalmappings.service';
import { CreateFuelGroupComponent } from '../create-fuel-group.component';
import { FuelGroupGridViewModel, FuelGroupViewModel } from 'src/app/self-service-alias/models/FuelGroupGridViewModel';

@Component({
    selector: 'app-fuel-group-mapping',
    templateUrl: './fuel-group-mapping.component.html',
    styleUrls: ['./fuel-group-mapping.component.css']
})
export class FuelGroupMappingComponent implements OnInit, AfterViewInit {
    
    title = 'Create';
    @ViewChild(DataTableDirective) datatableElement: DataTableDirective;
    @ViewChild('openSidePannel') public openSidePannel: ElementRef;
    @ViewChild('btnCloseBulkUploadPopup') CloseBulkUploadPopup: ElementRef;
    @ViewChild(CreateFuelGroupComponent) public createFuelGroupComponent: CreateFuelGroupComponent;
    fuelGroupGridList: FuelGroupGridViewModel[] = [];
    fuelGroupEdit: FuelGroupViewModel;

    public dtOptions: any = {};
    public dtTrigger: Subject<any> = new Subject();
    public IsLoading: boolean;
    public IsValidForm: boolean = true;
    public popoverTitle: string = 'Archive Confirmation';
    public popoverMessage: string = 'Do you want to archive?';
    public cancelClicked: boolean = false;
    public confirmClicked: boolean = false;
    public confirmButtonText: string = 'Yes';
    public cancelButtonText: string = 'No';
    public IsShowBulkUploadPopup: boolean = false;
    public SelectedFiles: File[] = [];
    public MaxFileUploadSize: number = 1048576; // 1MB

    public GroupId: number = -1;

    constructor(private httpService: HttpClient, private carrierService: CarrierService,
        private externalMappingsService: ExternalMappingsService,
        private fuelSurchargeService: FuelSurchargeService,
        private RegionService: RegionService    ) { }

    ngOnInit() {
        let exportInvitedColumns = { columns: ':visible' };
        this.dtOptions = {
            dom: '<"html5buttons"B>lTfgitp',
            aaSorting: [],
            orderable: false,
            buttons: [
                { extend: 'colvis' },
                { extend: 'copy', exportOptions: exportInvitedColumns },
                { extend: 'csv', title: 'Fuel Group Mapping', exportOptions: exportInvitedColumns },
                { extend: 'pdf', title: 'Fuel Group Mapping', orientation: 'landscape', exportOptions: exportInvitedColumns },
                { extend: 'print', exportOptions: exportInvitedColumns }
            ],
            pagingType: 'first_last_numbers',
            pageLength: 10,
            lengthMenu: [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]]
        };

        this.getFuelGroupMapping();

    }


    filterGrid() {
        $("#fuel-group-datatable").DataTable().clear().destroy();
    }
    ngAfterViewInit(): void {
        this.dtTrigger.next();
    }

    private getFuelGroupMapping() :void {
        this.IsLoading = true;
        this.externalMappingsService.getFuelGroupSummary().subscribe(async (data) => {
            this.IsLoading = false;
            this.fuelGroupGridList = await (data);
            this.datatableRerender();
        });
    }

    private datatableRerender(): void {
        if (this.datatableElement.dtInstance) {
            this.datatableElement.dtInstance.then((dtInstance: DataTables.Api) => {
                dtInstance.destroy();
                this.dtTrigger.next();
            });
        }
    }

    IdInComparer(otherArray) {
        return function (current) {
            return otherArray.filter(function (other) {
                return other == current.Id
            }).length == 1;
        }
    }

    
   
    public editFuelGroupMapping(groupId: number,mode: string): void {
        this.title = mode;
        this.createFuelGroupComponent.isLoadingSubject.next(false);
        this.createFuelGroupComponent.nativeServiceCall = false;
        this.httpService.get(this.externalMappingsService.getFuelGroupDetailsUrl + groupId).pipe(
            map(item => {
                this.IsLoading = true;
                return item as FuelGroupViewModel;
            }),
            mergeMap(item => {
                
                this.fuelGroupEdit = item;
                
                var getFuelTypesUrl = "";
                if (item.FuelGroupType == FuelGroupType.Standard) {
                    getFuelTypesUrl = this.externalMappingsService.getFuelTypesUrl + this.fuelGroupEdit.ProductTypeIds + "&fuelGroupType=" + this.fuelGroupEdit.FuelGroupType + "&editingGroupId=-1" + "&editingcompanyId=-1";
                } else {
                    getFuelTypesUrl = this.externalMappingsService.getFuelTypesUrl + this.fuelGroupEdit.ProductTypeIds + "&fuelGroupType=" + this.fuelGroupEdit.FuelGroupType + "&editingGroupId=" + this.fuelGroupEdit.Id + "&editingcompanyId=" + this.fuelGroupEdit.AssignedCompanyId;
                }
                var getTableTypesUrl = "/FuelSurcharge/GetTableTypes";
                const productTypes = this.httpService.get(this.externalMappingsService.getProductTypeUrl);
                const fuelTypes = this.httpService.get(getFuelTypesUrl);
                const customers = this.httpService.get(this.fuelSurchargeService.getSupplierCustomersUrl);
                const carriers = this.httpService.get(this.RegionService.getCarriersUrl);
                const Tabletypes = this.httpService.get(getTableTypesUrl);
                return forkJoin([fuelTypes, customers, carriers, productTypes, Tabletypes]);
            })).subscribe(result => {
                this.IsLoading = false;
                //this.createFuelGroupComponent.rcForm.reset();
                this.createFuelGroupComponent.isLoadingSubject.next(true);
                this.createFuelGroupComponent.rcForm.controls.Id.setValue(this.fuelGroupEdit.Id);
                this.createFuelGroupComponent.ViewOrEdit = mode;
                this.createFuelGroupComponent.rcForm.controls.GroupName.setValue(this.fuelGroupEdit.GroupName);

                if (this.fuelGroupEdit.FuelGroupType == FuelGroupType.Custom) {

                    let tblTypeList = result[4] as DropdownItem[];
                    tblTypeList = tblTypeList.filter(x => x.Id != 1); // no master included.
                    tblTypeList.forEach(res => res.Name = res.Name.replace("Specific", ""));
                    this.createFuelGroupComponent.TableTypeList = tblTypeList;

                    this.createFuelGroupComponent.rcForm.controls['GroupTypeCustom'].get('TableTypes').setValue(this.createFuelGroupComponent.TableTypeList.filter(t => t.Id == this.fuelGroupEdit.TableType));
                    //this.createFuelGroupComponent.onTableTypeSelect({ Id: this.fuelGroupEdit.TableType });

                    this.createFuelGroupComponent.rcForm.controls['GroupTypeCustom'].get('Customers').patchValue([]);
                    this.createFuelGroupComponent.rcForm.controls['GroupTypeCustom'].get('Carriers').patchValue([]);
                    this.createFuelGroupComponent.IsCustomerSelected = false;
                    this.createFuelGroupComponent.IsCarrierSelected = false;

                    this.createFuelGroupComponent.isLoadingSubject.next(true);

                    if (this.fuelGroupEdit.TableType != null && this.fuelGroupEdit.TableType == TableType.Customer) {
                        this.createFuelGroupComponent.IsCustomerSelected = true;
                        this.createFuelGroupComponent.CustomerList = result[1] as DropdownItem[];
                        this.createFuelGroupComponent.rcForm.controls['GroupTypeCustom'].get('Customers').setValue(this.createFuelGroupComponent.CustomerList.filter(x => x.Id == this.fuelGroupEdit.AssignedCompanyId));

                    }
                    else if (this.fuelGroupEdit.TableType != null && this.fuelGroupEdit.TableType == TableType.Carrier) {
                        this.createFuelGroupComponent.IsCarrierSelected = true;
                        this.createFuelGroupComponent.CarrierList = result[2] as DropdownItem[];
                        this.createFuelGroupComponent.rcForm.controls['GroupTypeCustom'].get('Carriers').setValue(this.createFuelGroupComponent.CarrierList.filter(x => x.Id == this.fuelGroupEdit.AssignedCompanyId));

                    }
                }

                this.createFuelGroupComponent.rcForm.controls.FuelGroupType.setValue(this.fuelGroupEdit.FuelGroupType);
                this.createFuelGroupComponent.ProductTypeList = result[3] as DropdownItem[];
                this.createFuelGroupComponent.rcForm.controls['GroupTypeStandard'].get('ProductTypes').setValue(this.createFuelGroupComponent.ProductTypeList.filter(this.IdInComparer(this.fuelGroupEdit.ProductTypeIds)));
                this.createFuelGroupComponent.IsProductTypeSelected = true;
                this.IsLoading = true;
                var ps = result[0] as DropdownCustomItem[];
                if (this.fuelGroupEdit.FuelGroupType == FuelGroupType.Custom) {
                    ps.filter(this.IdInComparer(this.fuelGroupEdit.FuelTypeIds));
                } else {
                    ps.filter(this.IdInComparer(this.fuelGroupEdit.FuelTypeIds)).forEach(res => res.isDisabled = false); // not all make isDisable false.
                }
                this.createFuelGroupComponent.rcForm.controls['GroupTypeStandard'].get('FuelTypes').patchValue([]);
                this.createFuelGroupComponent.FuelTypeList = ps;
                let fueltyps = this.createFuelGroupComponent.FuelTypeList.filter(this.IdInComparer(this.fuelGroupEdit.FuelTypeIds));
                this.createFuelGroupComponent.rcForm.controls['GroupTypeStandard'].get('FuelTypes').setValue(fueltyps);
                this.createFuelGroupComponent.isLoadingSubject.next(true);

                
                this.notEditableForm(mode);
                this.IsLoading = false;
                this.createFuelGroupComponent.isLoadingSubject.next(false);
                this.createFuelGroupComponent.nativeServiceCall = true;
            });
    }
    private notEditableForm(mode: string) {
        this.createFuelGroupComponent.IsEditable = true;
        if (mode == "VIEW") {
            this.createFuelGroupComponent.IsEditable = false;
        }
        //this.createFuelGroupComponent.rcForm.controls.GroupName.disable({ onlySelf: isEdit });
        //this.createFuelGroupComponent.rcForm.controls.FuelGroupType.disable({onlySelf: false });
        //this.createFuelGroupComponent.rcForm.controls['GroupTypeStandard'].get('ProductTypes').disable({ onlySelf: isEdit });
        //this.createFuelGroupComponent.rcForm.controls['GroupTypeStandard'].get('FuelTypes').disable({ onlySelf: isEdit });
        //this.createFuelGroupComponent.rcForm.controls['GroupTypeCustom'].get('TableTypes').disable({ onlySelf: isEdit });
        //this.createFuelGroupComponent.rcForm.controls['GroupTypeCustom'].get('Customers').disable({ onlySelf: isEdit });
        //this.createFuelGroupComponent.rcForm.controls['GroupTypeCustom'].get('Carriers').disable({ onlySelf: isEdit });
        //console.log(mode  + " : " + isEdit);
    }
   
    public addFuelGroup() {
        this.title = 'Create';
        this.createFuelGroupComponent.rcForm.reset();
        this.createFuelGroupComponent.IsEditable = true;
        this.createFuelGroupComponent.IsCarrierSelected = false;
        this.createFuelGroupComponent.IsCustomerSelected = false;
        this.createFuelGroupComponent.rcForm.controls['Id'].patchValue(null);
        this.createFuelGroupComponent.ViewOrEdit = "Create";
        this.createFuelGroupComponent.rcForm.controls.FuelGroupType.setValue(FuelGroupType.Standard);
        this.createFuelGroupComponent.isLoadingSubject.next(false);
    }

    getOutput($event) {
        if ($event) {
            this.openSidePannel.nativeElement.click();
            this.getFuelGroupMapping();
        }
    }

    archiveFuelGroup(item: FuelGroupGridViewModel) {
        this.IsLoading = true;
        
        this.externalMappingsService.archiveFuelGroup(item.Id.toString()).subscribe(async (data) => {
            var result = await (data);
            if (result.StatusCode == 0) {
                Declarations.msgsuccess(item.GroupName +  " fuel group archived successfully.", undefined, undefined);
                this.getFuelGroupMapping();
            } else
                Declarations.msgerror(result.StatusMessage, undefined, undefined);
            this.IsLoading = false;
        });
    }


    selectFiles(files: File[]) {
        if (files != null && files != undefined && files.length > 0)
            this.SelectedFiles = files;
    }

 

    isValidFile(file: File) {
        var isValid = true;
        var extension = this.getExtension(file.name);
        if (extension == undefined || extension == null || extension == '' || extension.toLowerCase() != 'csv') {
            Declarations.msgerror('Invalid file, only csv files are allowed', undefined, undefined);
            isValid = false;
            return isValid;
        }

        if (file.size > this.MaxFileUploadSize) {
            Declarations.msgerror('Invalid file size, file size should not be greater than 1 MB', undefined, undefined);
            isValid = false;
            return isValid;
        }

        return isValid;
    }

    downloadCsvTemplate() {
        this.IsLoading = true;
        var timestamp = new Date().getTime();
        this.carrierService.downloadTerminalItemCodeMappingTemplate(timestamp).subscribe(blob => {
            const a = document.createElement('a')
            const objectUrl = URL.createObjectURL(blob)
            a.href = objectUrl
            a.download = 'FuelGroupMapping_Template.csv';
            a.click();
            URL.revokeObjectURL(objectUrl);

            this.IsLoading = false;
        });
    }

    getExtension(fileName) {
        // extract file name from full path ...
        var basename = fileName.split(/[\\/]/).pop();

        // (supports `\\` and `/` separators)
        var pos = basename.lastIndexOf(".");       // get last position of `.`

        if (basename === "" || pos < 1)            // if file name is empty or ...
            return "";                             //  `.` not found (-1) or comes first (0)

        return basename.slice(pos + 1);            // extract extension ignoring `.`
    }
}
