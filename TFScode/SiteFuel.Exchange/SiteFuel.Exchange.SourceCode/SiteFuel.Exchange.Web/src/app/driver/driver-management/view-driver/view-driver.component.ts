import { Component, OnInit, ViewChild } from '@angular/core';
import { Subject } from 'rxjs';
import { CreateDriverComponent } from '../create-driver/create-driver.component';
import { DriverViewModel, DriverShiftModel, ShiftDetailModel } from '../../models/DriverManagementModel';
import { DriverService } from '../../services/driver.service';
import { Declarations } from 'src/app/declarations.module';
import { RegionService } from 'src/app/company-addresses/region/service/region.service';
import * as moment from 'moment';
import { Driver } from '../../../shared-components/sendbird/sendbirdCommon';

@Component({
    selector: 'app-view-driver',
    templateUrl: './view-driver.component.html',
    styleUrls: ['./view-driver.component.css']
})
export class ViewDriverComponent implements OnInit {
    dtOptions: any = {};
    dtTrigger: Subject<any> = new Subject();
    dtOptionsOnboarded: any = {};
    dtTriggerOnboarded: Subject<any> = new Subject();
    public InvitedDrivers: DriverViewModel[] = [];
    public OnboardedDrivers: DriverViewModel[] = [];
    public HeaderText: string;
    public IsLoading: boolean = false;
    public popoverTitle: string = 'Are you sure?';
    public confirmButtonText: string = 'Yes';
    public cancelButtonText: string = 'No';
    public ShiftInfo: DriverShiftModel = new DriverShiftModel();
    public IsShowShiftInfoPopup: boolean = false;
    public setDeleteDriverInfo: DriverViewModel;
    public IsLoadingdriverDelete: boolean = false;
    public IsScheduleExists: boolean = false;
    DriverShiftDetailList: any[] = [];

    constructor(private driverService: DriverService, private regionService: RegionService) {
    }
    @ViewChild(CreateDriverComponent) CreateDriver: CreateDriverComponent;

    ngOnInit() {
        this.HeaderText = 'Create Driver';
        this.initializeInvitedDrivers();
        this.initializeOnboardedDrivers();
        this.getAllDrivers();
    }

    initializeInvitedDrivers() {
        let exportInvitedColumns = { columns: ':visible' };
        this.dtOptions = {
            dom: '<"html5buttons"B>lTfgitp',
            buttons: [
                { extend: 'colvis' },
                { extend: 'copy', exportOptions: exportInvitedColumns },
                { extend: 'csv', title: 'Driver Details', exportOptions: exportInvitedColumns },
                { extend: 'pdf', title: 'Driver Details', orientation: 'landscape', exportOptions: exportInvitedColumns },
                { extend: 'print', exportOptions: exportInvitedColumns }
            ],
            pagingType: 'first_last_numbers',
            pageLength: 10,
            lengthMenu: [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]]
        };
    }

    initializeOnboardedDrivers() {
        let exportOnboardedColumns = { columns: ':visible' };
        this.dtOptionsOnboarded = {
            dom: '<"html5buttons"B>lTfgitp',
            buttons: [
                { extend: 'colvis' },
                { extend: 'copy', exportOptions: exportOnboardedColumns },
                { extend: 'csv', title: 'Driver Details', exportOptions: exportOnboardedColumns },
                { extend: 'pdf', title: 'Driver Details', orientation: 'landscape', exportOptions: exportOnboardedColumns },
                { extend: 'print', exportOptions: exportOnboardedColumns }
            ],
            pagingType: 'first_last_numbers',
            pageLength: 10,
            lengthMenu: [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]]
        };
    }

    getAllDrivers() {
        this.IsLoading = true;
        this.driverService.getAllDrivers().subscribe(data => {
            this.IsLoading = false;
            this.InvitedDrivers = data.InvitedDrivers as DriverViewModel[];
            this.OnboardedDrivers = data.OnboardedDrivers as DriverViewModel[];
            this.dtTrigger.next();
            this.dtTriggerOnboarded.next();
        });
    }

    editDriver(driver: DriverViewModel, isOnboarded: boolean) {
        if (this.CreateDriver != undefined) {
            this.CreateDriver.IsOnboarded = isOnboarded;
            this.CreateDriver.loadDriverDetail(driver);
        }
    }

    deleteDriver() {
        if (this.setDeleteDriverInfo != null) {
            this.IsLoadingdriverDelete = true;
            this.IsLoading = true;
            this.driverService.postDeleteDriver(this.setDeleteDriverInfo).subscribe(data => {
                this.IsLoading = false;
                this.IsLoadingdriverDelete = false;
                this.getDriverDetails();
                if (data.StatusCode == 0) {
                    Declarations.msgsuccess(data.StatusMessage, undefined, undefined);
                }
                else if (data.StatusCode == 2) {
                    Declarations.msgwarning(data.StatusMessage, undefined, undefined);
                }
                else {
                    Declarations.msgerror(data == null ? 'Failed' : data.StatusMessage, undefined, undefined);
                }
                $("#btnDriverCancel").click();
            });
        }
    }

    changeDriverStatus(driver: any) {
        var isActive = driver.IsActive;
        var userId = driver.UserId;
        this.IsLoading = true;
        this.driverService.changeDriverStatus(userId, isActive).subscribe(data => {
            this.IsLoading = false;
            if (data.StatusCode == 0) {
                Declarations.msgsuccess(data.StatusMessage, undefined, undefined);
            }
            else if (data.StatusCode == 2) {
                Declarations.msgwarning(data.StatusMessage, undefined, undefined);
            }
            else {
                Declarations.msgerror(data == null ? 'Failed' : data.StatusMessage, undefined, undefined);
            }
        });
    }

    async showDriverShifts(driver: DriverViewModel) {
        var driverIds = driver.DriverId.toString();
        this.IsShowShiftInfoPopup = true;
        this.IsLoading = true;
        this.regionService.getShiftByDrivers(driverIds, 0).subscribe(data => {
            this.IsLoading = false;
            if (data != null && data.Result) {
                this.ShiftInfo.DriverName = driver.FirstName + " " + driver.LastName;
                this.DriverShiftDetailList = data.Result;
                this.setShiftInfo();
            }
            else {
                this.ShiftInfo = new DriverShiftModel();
            }
        });
    }

    closeDriverShiftsPopup() {
        this.IsShowShiftInfoPopup = false;
        this.ShiftInfo = new DriverShiftModel();
    }

    clearPanelControls() {
        if (this.CreateDriver != undefined) {
            this.CreateDriver.clearForm();
        }
    }

    setPanelHeader(headerText: string) {
        this.HeaderText = headerText;
    }

    getDriverDetails() {
        this.getAllDrivers();
        $("#invited-driver-grid-datatable").DataTable().clear().destroy();
        $("#onboarded-driver-grid-datatable").DataTable().clear().destroy();
    }

    async setShiftInfo() {
        this.DriverShiftDetailList.forEach(async (driver) => {
            for await (let shift of driver.ScheduleList) {
                var shiftDetail = new ShiftDetailModel();
                shiftDetail.FromDate = moment(shift.StartDate).format('MM/DD/YYYY');
                shiftDetail.ToDate = moment(shift.EndDate).format('MM/DD/YYYY');
                shiftDetail.ShiftFrom = shift.ShiftDetail.StartTime;
                shiftDetail.ShiftTo = shift.ShiftDetail.EndTime;
                this.ShiftInfo.Shifts.push(shiftDetail);
            }
        });
    }
    setdeleteDriver(driverInfo: DriverViewModel) {
        this.IsScheduleExists = false;
        this.setDeleteDriverInfo = driverInfo;
        if (driverInfo.IsScheduleExists) {
            this.IsScheduleExists = true;
        }
    }
}
