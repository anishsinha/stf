import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormControl, FormArray } from '@angular/forms';
import { RouteInformationModel, JobWithSequence, RouteTfxJobsList } from '../../models/location';
import { RouteInfoService } from '../../service/route-info.service';
import { IDropdownSettings } from 'ng-multiselect-dropdown';
import { DropdownItem } from 'src/app/statelist.service';
import { Declarations } from 'src/app/declarations.module';
import { PanelStatus } from 'src/app/app.enum';

@Component({
  selector: 'app-route-info',
  templateUrl: './route-info.component.html',
  styleUrls: ['./route-info.component.css']
})
export class RouteInfoComponent implements OnInit {

  public RouteForm: FormGroup;
  public RouteName: string;
  public RouteList: RouteInformationModel[] = [];
  public RouteLocationList: RouteTfxJobsList[] = [];

  public JobList: DropdownItem[] = [];
  public SelectedRegionId: string = '';
  public currentUserCompanyId: number = 0;
  public multiDropdownSettings: IDropdownSettings;
  public PanelStatus = PanelStatus.ShowRouteList;
  public IsLoading = false;

  constructor(
    private fb: FormBuilder,
    private routeInfoService: RouteInfoService) { }

  ngOnInit() {
    this.RouteForm = this.initForm();
    this.multiDropdownSettings = {
      singleSelection: true,
      idField: 'Id',
      textField: 'Name',
      itemsShowLimit: 1,
      allowSearchFilter: true
    };
  }
  validateLocations(): boolean {
    let tfxJobs = this.RouteForm.get('TfxJobs').value as any[];
    let seen = new Set();
    return tfxJobs.some(function(currentObject) {
        return seen.size === seen.add(currentObject.JobDetails[0].Id).size;
    });
  }
  onSubmit() {

    if (!this.validateLocations()) {
      let formValue = this.RouteForm.value;
      let data: RouteInformationModel = {
        Id: formValue.Id,
        Name: formValue.Name,
        TfxJobs: formValue.TfxJobs.map((job, index) => ({ SequenceNo: (index + 1), Id: job.JobDetails[0].Id, Name: job.JobDetails[0].Name })),
        RegionId: formValue.RegionId,
        TfxCompanyId: formValue.TfxCompanyId,
        CreatedBy: null,
        ShiftInfoDetails: null
      };

      this.IsLoading = true;
      if (this.RouteForm.get('Id').value) {
        this.routeInfoService.updateRouteInfo(data).subscribe(response => {
          if (response != null && response['StatusCode'] == 0) {
            this.PanelStatus = PanelStatus.ShowRouteList;
            this.getRoutesByRegion();
            Declarations.msgsuccess("Route updated.", undefined, undefined);
          }
          else {
            Declarations.msgerror(response['StatusMessage'], undefined, undefined);
          }
          this.IsLoading = false;
        });
      }
      else {
        this.routeInfoService.createRouteInfo(data).subscribe(response => {
          if (response != null && response['StatusCode'] == 0) {
            this.PanelStatus = PanelStatus.ShowRouteList;
            this.getRoutesByRegion();
            Declarations.msgsuccess(response['StatusMessage'], undefined, undefined);
          }
          else {
            Declarations.msgerror(response['StatusMessage'], undefined, undefined);
          }
          this.IsLoading = false;
        });
      }
    }
    else{
      Declarations.msgerror('Duplicate locations exist.', undefined, undefined);
    }
  }
  setRegionId(companyId: number, regionId: string) {
    this.PanelStatus = PanelStatus.ShowRouteList;
    this.SelectedRegionId = regionId;
    this.currentUserCompanyId = companyId;
    this.getRoutesByRegion();
  }
  editRouteClicked(route: RouteInformationModel) {
    this.getLocationsByRoute(route.Id);
    this.PanelStatus = PanelStatus.ShowForm;
    this.buildForm(route);
  }
  getRouteInfo(route: RouteInformationModel) {
    var routeLocInfo = this.RouteList.find(top => top.Id == route.Id);
    if (routeLocInfo != null) {
      this.routeInfoService.sendRouteInfo(routeLocInfo);
    }
  }
  deleteRouteClicked(route: RouteInformationModel) {
    this.IsLoading = true;
    this.PanelStatus = PanelStatus.ShowRouteList;
    let data = {
      RouteId: route.Id,
      RegionId: route.RegionId
    }
    this.routeInfoService.deleteRouteInfo(data).subscribe(response => {
      if (response != null && response['StatusCode'] == 0) {
        Declarations.msgsuccess(response['StatusMessage'], undefined, undefined);
      }
      else {
        Declarations.msgerror(response['StatusMessage'], undefined, undefined);
      }
      this.IsLoading = false;
      this.getRoutesByRegion();
    });
  }
  newRouteClicked() {
    this.PanelStatus = PanelStatus.ShowForm;
    this.getLocationsByRegion();
    this.RouteForm = this.initForm();
    this.addLocation(null);
  }

  initForm(): FormGroup {
    return this.fb.group({
      Id: this.fb.control(''),
      Name: this.fb.control('', Validators.required),
      RegionId: this.fb.control(this.SelectedRegionId, Validators.required),
      TfxCompanyId: this.fb.control(this.currentUserCompanyId),
      ShiftInfoDetails: this.fb.control(''),
      TfxJobs: this.fb.array([], Validators.required)
    });
  }

  buildForm(route: any) {
    let jobs: JobWithSequence[] = [];
    if (route && route.TfxJobs.length > 0) {
      jobs = route.TfxJobs.map(job => ({
        SequenceNo: job.SequenceNo,
        JobDetails: { Id: job.Id.toString(), Name: job.Name }
      }));
    }

    this.RouteForm = this.initForm();
    let name = this.RouteForm.get('Name') as FormControl;
    name.patchValue(route.Name);
    let id = this.RouteForm.get('Id') as FormControl;
    id.patchValue(route.Id);
    let shift = this.RouteForm.get('ShiftInfoDetails') as FormControl;
    shift.patchValue(route.ShiftInfoDetails);
    let tfxJobs = this.RouteForm.get('TfxJobs') as FormArray;
    jobs.forEach(job => {
      tfxJobs.push(this.getRouteJob(job));
    });
  }
  addLocation(data: JobWithSequence) {
    let tfxJobs = this.RouteForm.get('TfxJobs') as FormArray;
    tfxJobs.push(this.getRouteJob(data));
  }
  getRouteJob(data: JobWithSequence): FormGroup {
    return this.fb.group({
      SequenceNo: new FormControl(data && data.SequenceNo ? data.SequenceNo : null),
      JobDetails: new FormControl(data && data.JobDetails ? [data.JobDetails] : null, Validators.required),
    })
  }
  removeLocation(i: number) {
    let tfxJobs = this.RouteForm.get('TfxJobs') as FormArray;
    tfxJobs.removeAt(i);
    if (tfxJobs.length === 0) {
      this.addLocation(null);
    }
  }
  getRoutesByRegion(): void {
    this.IsLoading = true;
    this.RouteList = [];
    this.routeInfoService.getRoutesByRegion(this.SelectedRegionId).subscribe(response => {
      if (response != null && response['ResponseData'] && response['ResponseData'].length > 0) {
        this.RouteList = response['ResponseData'];
      }
      this.IsLoading = false;
    });
  }
  getLocationsByRegion(): void {
    this.IsLoading = true;
    this.JobList = [];
    this.routeInfoService.getLocationsByRegion(this.SelectedRegionId).subscribe(response => {
      if (response != null && response['ResponseData'] && response['ResponseData'].length > 0) {
        this.JobList = response['ResponseData'];
      }
      this.IsLoading = false;
    });
  }
  getLocationsByRoute(routeId: string): void {
    this.IsLoading = true;
    this.JobList = [];
    this.routeInfoService.getLocationsByRoute(routeId, this.SelectedRegionId).subscribe(response => {
      if (response != null && response['ResponseData'] && response['ResponseData'].length > 0) {
        this.JobList = response['ResponseData'];
      }
      this.IsLoading = false;
    });
  }
}

