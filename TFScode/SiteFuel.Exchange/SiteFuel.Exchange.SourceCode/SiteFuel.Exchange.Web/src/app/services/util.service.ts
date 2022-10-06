import { Injectable } from '@angular/core';
import { FormBuilder, Validators, FormGroup, FormArray } from '@angular/forms';
import { TrailerViewModel, TrailerShiftModel, TripModel, DeliveryRequestViewModel, DropAddressModel, DemandModel, LoadInfo, CompartmentModel, CompartmentInfo, DriverAdditionalDetailModel, BlendedRequest } from '../carrier/models/DispatchSchedulerModels';
import { DropdownItem } from 'src/app/statelist.service';
import * as moment from 'moment';
import { sortArrayTwice, sortByKeyAsc } from 'src/app/my.functions';
import { NumberConstants, RegExConstants } from 'src/app/app.constants';

@Injectable({
    providedIn: 'root'
})
export class UtilService {

    constructor(private readonly fb: FormBuilder) { }

    public getTrailerForm(trailer: TrailerViewModel, selectedShifts: []): FormGroup {
        return this.fb.group({
            Id: this.fb.control(trailer.Id),
            TrailerId: this.fb.control(trailer.TrailerId), // Rows in the shift 
            StartTime: this.fb.control(trailer.StartTime),
            EndTime: this.fb.control(trailer.EndTime),
            Compartments: this.fb.control(trailer.Compartments),
            TrailerType: this.fb.control(trailer.TrailerType),
            Shifts: this.fb.array([]), //this.getShiftsFormArray(trailer.Shifts || []),
            SelectedShifts: this.fb.control(selectedShifts),
            IsCollapsed: this.fb.control(trailer.IsCollapsed)
        });
    }

    public getShiftsFormArray(shifts: TrailerShiftModel[]): FormArray {
        var _sArray = this.fb.array([]);
        shifts.forEach(x => {
            _sArray.push(this.getShiftFormGroup(x));
        });
        return _sArray;
    }

    public getShiftFormGroup(x: TrailerShiftModel): FormGroup {
        let _sForm = this.fb.group({
            ShiftId: this.fb.control(x.ShiftId),
            StartTime: this.fb.control(x.StartTime),
            EndTime: this.fb.control(x.EndTime),
            SlotPeriod: this.fb.control(x.SlotPeriod),
            Trips: this.getTripsFormArray(x.Trips)
        });
        return _sForm;
    }

    public getTripsFormArray(trips: TripModel[]): FormArray {
        var _tArray = this.fb.array([]);
        trips.forEach(x => {
            _tArray.push(this.getTripFormGroup(x));
        });
        return _tArray;
    }

    public getTripFormGroup(x: TripModel): FormGroup {
        var _tForm = this.fb.group({
            TripId: this.fb.control(x.TripId),
            GroupId: this.fb.control(x.GroupId),
            DeliveryRequests: this.getDeliveryRequestFormArray(x.DeliveryRequests, x.IsCommonPickup, x.IsDispatcherDragDropSequence, 0),
            StartDate: this.fb.control(x.StartDate, Validators.required),
            StartTime: this.fb.control(x.StartTime, Validators.required),
            EndTime: this.fb.control(x.EndTime, Validators.required),
            LoadCode: this.fb.control(x.LoadCode),
            RouteInfo: this.fb.control(x.RouteInfo),
            SupplierSource: this.fb.control(x.SupplierSource),
            Carrier: this.fb.control(x.Carrier),
            TripStatus: this.fb.control(x.TripStatus),
            TripPrevStatus: this.fb.control(x.TripPrevStatus),
            DeliveryGroupStatus: this.fb.control(x.DeliveryGroupStatus),
            DeliveryGroupPrevStatus: this.fb.control(x.DeliveryGroupPrevStatus),
            PickupLocationType: this.fb.control(x.PickupLocationType),
            IsCommonPickup: this.fb.control(x.IsCommonPickup),
            Terminal: this.getTerminalForm(x.Terminal, x.IsCommonPickup && x.PickupLocationType != 2),
            BulkPlant: this.getBulkPlantForm(x.BulkPlant, x.IsCommonPickup && x.PickupLocationType == 2),
            ShiftIndex: this.fb.control(x.ShiftIndex),
            DriverRowIndex: this.fb.control(x.DriverRowIndex),
            DriverColIndex: this.fb.control(x.DriverColIndex),
            TrailerRowIndex: this.fb.control(x.TrailerRowIndex),
            TrailerColIndex: this.fb.control(x.TrailerColIndex),
            IsEditable: this.fb.control(x.IsEditable),
            TimeStamp: this.fb.control(x.TimeStamp),
            ShiftId: this.fb.control(x.ShiftId),
            ShiftStartTime: this.fb.control(x.ShiftStartTime),
            ShiftEndTime: this.fb.control(x.ShiftEndTime),
            SlotPeriod: this.fb.control(x.SlotPeriod),
            DriverScheduleMappingId: this.fb.control(x.DriverScheduleMappingId),
            BadgeNo1: this.fb.control(x.BadgeNo1),
            BadgeNo2: this.fb.control(x.BadgeNo2),
            BadgeNo3: this.fb.control(x.BadgeNo3),
            IsCommonBadge: this.fb.control(x.IsCommonBadge),
            isRecurringSchedule: this.fb.control(x.isRecurringSchedule),
            RecurringScheduleId: this.fb.control(x.RecurringScheduleId),
            ScheduleQuantityType: this.fb.control(x.ScheduleQuantityType),
            ScheduleQuantityTypeText: this.fb.control(x.ScheduleQuantityTypeText),
            IsTrailerExists: this.fb.control(x.IsTrailerExists),
            IsPreLoadInfo: this.fb.control(x.IsPreLoadInfo),
            IsPreloadDisable: x.DeliveryRequests.filter(x => x.IsPreloadDisable == true).length > 0 ? this.fb.control(true) : this.fb.control(false),
            IsDriverScheduleExists: (x.IsDriverScheduleExists),
            IsDispatcherDragDropSequence: (x.IsDispatcherDragDropSequence),
            IsDispatcherDragDropSequenceModified: (x.IsDispatcherDragDropSequenceModified),
        });
        return _tForm;
    }

    public getDriversFormArray(drivers: DriverAdditionalDetailModel[]): FormArray {
        var _dArray = this.fb.array([]);
        drivers.forEach(x => {
            _dArray.push(this.fb.group({
                Id: this.fb.control(x.Id),
                Shifts: this.fb.control(x.Shifts),
                Name: this.fb.control(x.Name),
                IsFilldCompatible: this.fb.control(x.IsFilldCompatible)
            }));
        });
        return _dArray;
    }

    public getTrailersFormArray(Trailers: TrailerViewModel[]): FormArray {
        var _tArray = this.fb.array([]);
        Trailers.forEach(x => {
            _tArray.push(this.fb.group({
                Id: this.fb.control(x.Id || ''),
                TrailerId: this.fb.control(x.TrailerId || ''),
                StartTime: this.fb.control(x.StartTime || ''),
                EndTime: this.fb.control(x.EndTime || ''),
                Compartments: this.fb.control(x.Compartments || ''),
                TrailerType: this.fb.control(x.TrailerType)
            }));
        });
        return _tArray;
    }

    public getDeliveryRequestFormArray(delReqs: DeliveryRequestViewModel[], isCommonPickup: boolean, IsDispatcherDragDropSequence: boolean = false, isShiftTrailerChange: number = 0, sourceTrip: any = null): FormArray {
        delReqs = sortArrayTwice(delReqs, 'JobId', 'ProductSequence');
        if (IsDispatcherDragDropSequence) {
            delReqs = sortByKeyAsc(delReqs, 'DispatcherDragDropSequence');
        }
        let drs = [] as DeliveryRequestViewModel[];
        delReqs.forEach(x => {
            if (!x.IsBlendedRequest) {
                drs.push(x);
            }
            else if (!drs.some(g => g.BlendedGroupId == x.BlendedGroupId)) {
                var blendDrs = delReqs.filter(b => b.BlendedGroupId == x.BlendedGroupId);
                blendDrs.filter(b => !b.IsAdditive && b.IsBlendedDrParent).forEach(p => { drs.push(p); });
                blendDrs.filter(b => !b.IsAdditive && !b.IsBlendedDrParent).forEach(p => { drs.push(p); });
                blendDrs.filter(b => b.IsAdditive && b.IsBlendedDrParent).forEach(p => { drs.push(p); });
                blendDrs.filter(b => b.IsAdditive && !b.IsBlendedDrParent).forEach(p => { drs.push(p); });
            }
        });
        var _drArray = this.fb.array([], Validators.required);
        drs.forEach(x => {
            var _form = this.getDeliveryRequestForm(x, isCommonPickup, isShiftTrailerChange, sourceTrip);
            _drArray.push(_form);
        });
        return _drArray;
    }
    public getDispatcherDeliveryRequestFormArray(delReqs: DeliveryRequestViewModel[], isCommonPickup: boolean, isShiftTrailerChange: number = 0, sourceTrip: any = null): FormArray {

        let drs = [] as DeliveryRequestViewModel[];
        delReqs.forEach(x => {
            if (!x.IsBlendedRequest) {
                drs.push(x);
            }
            else if (!drs.some(g => g.BlendedGroupId == x.BlendedGroupId)) {
                var blendDrs = delReqs.filter(b => b.BlendedGroupId == x.BlendedGroupId);
                blendDrs.filter(b => !b.IsAdditive && b.IsBlendedDrParent).forEach(p => { drs.push(p); });
                blendDrs.filter(b => !b.IsAdditive && !b.IsBlendedDrParent).forEach(p => { drs.push(p); });
                blendDrs.filter(b => b.IsAdditive).forEach(p => { drs.push(p); });
            }
        });
        var _drArray = this.fb.array([], Validators.required);
        drs.forEach(x => {
            var _form = this.getDispatcherDeliveryRequestForm(x, isCommonPickup, isShiftTrailerChange, sourceTrip);
            _drArray.push(_form);
        });
        return _drArray;
    }
    GetAllLoadsDR(trips: FormArray) {
        var _deliveryRequests = [];
        if (trips) {
            for (var i = 0; i < trips.length; i++) {
                var trip = trips.controls[i] as FormGroup;
                var deliveryRequests = trip.controls["DeliveryRequests"].value;
                if (deliveryRequests) {
                    for (var j = 0; j < deliveryRequests.length; j++) {
                        var deliveryRequest = deliveryRequests[j];
                        if (deliveryRequest) {
                            _deliveryRequests.push(deliveryRequest);
                        }
                    }
                }
            }
        }
        return _deliveryRequests;
    }

    public getDeliveryRequestForm(delReq: DeliveryRequestViewModel, isCommonPickup: boolean, isShiftTrailerChange: number = 0, sourceTrip: any = null): FormGroup {
        var isTerminalPickup = false;
        var isBulkplantPickup = false;
        //dragged from left panel
        if (!sourceTrip) {
            isTerminalPickup = !isCommonPickup && delReq.PickupLocationType != 2;
            isBulkplantPickup = !isCommonPickup && delReq.PickupLocationType == 2;
        }
        else { //dragged from a load
            var tripFromPickupLocationType, pickupLocation;
            if (sourceTrip.controls['IsCommonPickup'].value == true) {
                tripFromPickupLocationType = sourceTrip.controls['PickupLocationType'].value;
                if (tripFromPickupLocationType == 2) {
                    isBulkplantPickup = true;
                    pickupLocation = sourceTrip.controls['BulkPlant'].value;
                    delReq.BulkPlant = pickupLocation;
                    delReq.PickupLocationType = 2;
                }
                else {
                    isTerminalPickup = true;
                    pickupLocation = sourceTrip.controls['Terminal'].value;
                    delReq.Terminal = pickupLocation;
                    delReq.PickupLocationType = 1;
                }
            }
            else {
                isBulkplantPickup = delReq.PickupLocationType == 2;
                isTerminalPickup = delReq.PickupLocationType != 2;
            }
        }
        let quantityValidators = [];
        if (delReq.ScheduleQuantityType == 1 || delReq.ScheduleQuantityType == 0) {
            quantityValidators = [Validators.required, Validators.min(0.00001), Validators.pattern(RegExConstants.DecimalNumber)];
            if (delReq.TankMaxFill > 0 && !delReq.IsMaxFillAllowed) {
                quantityValidators.push(Validators.max(delReq.TankMaxFill));
            }
        }
        let orderIdValidators = [];
        if (!delReq.IsTBD) {
            orderIdValidators.push(Validators.required);
        }
        var _drForm = this.fb.group({
            Id: this.fb.control(''),
            JobId: this.fb.control(''),
            JobAddress: this.fb.control(''),
            JobCity: this.fb.control(''),
            JobName: this.fb.control(''),
            ProductType: this.fb.control(''),
            ProductTypeId: this.fb.control(delReq.ProductTypeId),
            SiteId: this.fb.control(''),
            RequiredQuantity: this.fb.control('', quantityValidators),
            Priority: this.fb.control(''),
            TankId: this.fb.control(''),
            TankMaxFill: this.fb.control(0),
            IsMaxFillAllowed: this.fb.control(delReq.IsMaxFillAllowed),
            CreditApprovalFilePath: this.fb.control(delReq.CreditApprovalFilePath),
            //IsReAssignToCarrier: this.fb.control(delReq.IsReAssignToCarrier),
            StorageId: this.fb.control(''),
            AssignedToCompanyId: this.fb.control(null),
            CreatedByCompanyId: this.fb.control(null),
            SupplierCompanyId: this.fb.control(null),
            Status: this.fb.control(0),
            PreviousStatus: this.fb.control(0),
            ScheduleStatus: this.fb.control(0),
            SchedulePreviousStatus: this.fb.control(0),
            OrderId: this.fb.control('', orderIdValidators),
            CreatedByRegionId: this.fb.control(''),
            AssignedToRegionId: this.fb.control(''),
            PickupLocationType: this.fb.control(0),
            Sap_OrderNo: this.fb.control(delReq.Sap_OrderNo),
            UniqueOrderNo: this.fb.control(delReq.UniqueOrderNo),
            Terminal: this.getTerminalForm(delReq.Terminal, isTerminalPickup),
            BulkPlant: this.getBulkPlantForm(delReq.BulkPlant, isBulkplantPickup),
            UoM: this.fb.control(''),
            ParentId: this.fb.control(null),
            DeliveryGroupId: this.fb.control(null),
            DeliveryScheduleId: this.fb.control(null),
            TrackableScheduleId: this.fb.control(null),
            CustomerCompany: this.fb.control(''),
            TrackScheduleEnrouteStatus: this.fb.control(null),
            ProductSequence: this.fb.control(delReq.ProductSequence),
            TrackScheduleStatus: this.fb.control(null),
            TrackScheduleStatusName: this.fb.control(null),
            StatusClassId: this.fb.control(null),
            IsNotCompatible: this.fb.control(false),
            IsBlinkDR: this.fb.control(false),
            IsAutoCreatedDR: this.fb.control(false),
            BadgeNo1: this.fb.control(''),
            BadgeNo2: this.fb.control(''),
            BadgeNo3: this.fb.control(''),
            IsCommonBadge: this.fb.control(true),
            SourceTripId: this.fb.control(null),
            DispactherNote: this.fb.control(''),
            Notes: this.fb.control(delReq.Notes),
            isRecurringSchedule: this.fb.control(delReq.isRecurringSchedule),
            RecurringScheduleId: this.fb.control(delReq.RecurringScheduleId),
            ScheduleQuantityType: this.fb.control(delReq.ScheduleQuantityType),
            ScheduleQuantityTypeText: this.fb.control(delReq.ScheduleQuantityTypeText),
            RouteInfo: this.fb.control(null),
            CarrierStatus: this.fb.control(0),
            DeliveryRequestFor: this.fb.control(delReq.DeliveryRequestFor),
            Compartments: isShiftTrailerChange == 0 ? this.getCompartments(delReq.Compartments) : this.fb.array([]),
            IsFilldInvoke: this.fb.control(false),
            TruckLoadType: this.fb.control(''),
            IsRetainFuelLoaded: this.fb.control(false),
            DelReqSource: this.fb.control(delReq.DelReqSource),
            IsPreloadDisable: this.fb.control(delReq.IsPreloadDisable),
            IsSpiltDRIconVisible: this.fb.control(delReq.IsSpiltDRIconVisible),
            GroupParentDRId: this.fb.control(delReq.GroupParentDRId),
            GroupChildDRs: this.fb.control(delReq.GroupChildDRs),
            DeliveryWindow: this.fb.control(delReq.DeliveryWindow),
            IsAcceptNightDeliveries: this.fb.control(delReq.IsAcceptNightDeliveries),
            TrailerTypes: this.fb.control(delReq.TrailerTypes),
            LoadQueueAttributes: this.fb.control(delReq.LoadQueueAttributes),
            DRQueueAttributes: this.fb.control(delReq.DRQueueAttributes),
            HoursToCoverDistance: this.fb.control(delReq.HoursToCoverDistance),
            CustomerBrandId: this.fb.control(delReq.CustomerBrandId),
            IsJobFilter: this.fb.control(true),
            IsSelectedForAssignment: this.fb.control(false),
            IsTBD: this.fb.control(delReq.IsTBD),
            TBDGroupId: this.fb.control(delReq.TBDGroupId),
            FuelTypeId: this.fb.control(delReq.FuelTypeId),
            FuelType: this.fb.control(delReq.FuelType),
            TBDLocations: this.fb.control(delReq.TBDLocations),
            DeliveryDateStartTime: this.fb.control(delReq.DeliveryDateStartTime),
            Vessel: this.fb.control(delReq.Vessel),
            Berth: this.fb.control(delReq.Berth),
            IsMarine: this.fb.control(delReq.IsMarine),
            //BLENDED DR
            IsBlendedRequest: this.fb.control(delReq.IsBlendedRequest || false),
            IsCommonPickupForBlend: this.fb.control(delReq.IsCommonPickupForBlend || false),
            BlendedGroupId: this.fb.control(delReq.BlendedGroupId || null),
            BlendParentProductTypeId: this.fb.control(delReq.BlendParentProductTypeId),
            IsBlendedDrParent: this.fb.control(delReq.IsBlendedDrParent || false),
            TotalBlendedQuantity: this.fb.control(delReq.TotalBlendedQuantity),
            BlendDrScheduleStatus: this.fb.control(delReq.BlendDrScheduleStatus),
            BlendedProductName: this.fb.control(delReq.BlendedProductName || null),
            IsAdditive: this.fb.control(delReq.IsAdditive),
            AdditiveProductName: this.fb.control(delReq.AdditiveProductName || null),
            SelectedDate: this.fb.control(delReq.SelectedDate),
            IsFutureDR: this.fb.control(delReq.IsFutureDR),
            IsCalendarView: this.fb.control(delReq.IsCalendarView),
            IsDispatcherDragDrop: this.fb.control(delReq.IsDispatcherDragDrop),
            DispatcherDragDropSequence: this.fb.control(delReq.DispatcherDragDropSequence),
            DeliveryLevelPO: this.fb.control(delReq.DeliveryLevelPO),
            IndicativePrice: this.fb.control(delReq.IndicativePrice),
            ScheduleStartTime: this.fb.control(delReq.ScheduleStartTime),
            ScheduleEndTime: this.fb.control(delReq.ScheduleEndTime),
        });
        if (delReq != null && delReq != undefined) {
            if (delReq.PreLoadedFor) {
                _drForm.addControl('PreLoadedFor', this.fb.control(delReq.PreLoadedFor));
            }
            if (delReq.PostLoadedFor) {
                _drForm.addControl('PostLoadedFor', this.fb.control(delReq.PostLoadedFor));
            }
            if (delReq.PreLoadInfo) {
                _drForm.addControl('PreLoadInfo', this.fb.group({
                    ShiftId: delReq.PreLoadInfo.ShiftId,
                    ScheduleIndex: delReq.PreLoadInfo.ScheduleIndex,
                    TripIndex: delReq.PreLoadInfo.TripIndex,
                    DrId: delReq.PreLoadInfo.DrId
                }));
            }
            if (delReq.PostLoadInfo) {
                _drForm.addControl('PostLoadInfo', this.fb.group({
                    ShiftId: delReq.PostLoadInfo.ShiftId,
                    ScheduleIndex: delReq.PostLoadInfo.ScheduleIndex,
                    TripIndex: delReq.PostLoadInfo.TripIndex,
                    DrId: delReq.PostLoadInfo.DrId
                }));
            }
            if (delReq.BulkPlant == null) {
                delReq.BulkPlant = new DropAddressModel();
            }
            if (delReq.Terminal == null) {
                delReq.Terminal = new DropdownItem();
            }
            _drForm.patchValue(delReq);
        }
        return _drForm;
    }
    public getDispatcherDeliveryRequestForm(delReq: DeliveryRequestViewModel, isCommonPickup: boolean, isShiftTrailerChange: number = 0, sourceTrip: any = null): FormGroup {
        var isTerminalPickup = false;
        var isBulkplantPickup = false;
        //dragged from left panel
        if (!sourceTrip) {
            isTerminalPickup = !isCommonPickup && delReq.PickupLocationType != 2;
            isBulkplantPickup = !isCommonPickup && delReq.PickupLocationType == 2;
        }
        else { //dragged from a load
            var tripFromPickupLocationType, pickupLocation;
            if (sourceTrip.controls['IsCommonPickup'].value == true) {
                tripFromPickupLocationType = sourceTrip.controls['PickupLocationType'].value;
                if (tripFromPickupLocationType == 2) {
                    isBulkplantPickup = true;
                    pickupLocation = sourceTrip.controls['BulkPlant'].value;
                    delReq.BulkPlant = pickupLocation;
                    delReq.PickupLocationType = 2;
                }
                else {
                    isTerminalPickup = true;
                    pickupLocation = sourceTrip.controls['Terminal'].value;
                    delReq.Terminal = pickupLocation;
                    delReq.PickupLocationType = 1;
                }
            }
            else {
                isBulkplantPickup = delReq.PickupLocationType == 2;
                isTerminalPickup = delReq.PickupLocationType != 2;
            }
        }
        let quantityValidators = [];
        if (delReq.ScheduleQuantityType == 1 || delReq.ScheduleQuantityType == 0) {
            quantityValidators = [Validators.required, Validators.min(0.00001), Validators.pattern(RegExConstants.DecimalNumber)];
            if (delReq.TankMaxFill > 0 && !delReq.IsMaxFillAllowed) {
                quantityValidators.push(Validators.max(delReq.TankMaxFill));
            }
        }
        let orderIdValidators = [];
        if (!delReq.IsTBD) {
            orderIdValidators.push(Validators.required);
        }
        var _drForm = this.fb.group({
            Id: this.fb.control(delReq.Id),
            JobId: this.fb.control(delReq.JobId),
            JobAddress: this.fb.control(delReq.JobAddress),
            JobCity: this.fb.control(delReq.JobCity),
            JobName: this.fb.control(delReq.JobName),
            ProductType: this.fb.control(delReq.ProductType),
            ProductTypeId: this.fb.control(delReq.ProductTypeId),
            SiteId: this.fb.control(delReq.SiteId),
            RequiredQuantity: this.fb.control('', quantityValidators),
            Priority: this.fb.control(delReq.Priority),
            TankId: this.fb.control(delReq.TankId),
            TankMaxFill: this.fb.control(delReq.TankMaxFill),
            IsMaxFillAllowed: this.fb.control(delReq.IsMaxFillAllowed),
            //IsReAssignToCarrier: this.fb.control(delReq.IsReAssignToCarrier),
            StorageId: this.fb.control(delReq.StorageId),
            AssignedToCompanyId: this.fb.control(delReq.AssignedToCompanyId),
            CreatedByCompanyId: this.fb.control(delReq.CreatedByCompanyId),
            SupplierCompanyId: this.fb.control(delReq.SupplierCompanyId),
            Status: this.fb.control(delReq.Status),
            PreviousStatus: this.fb.control(delReq.PreviousStatus),
            ScheduleStatus: this.fb.control(delReq.ScheduleStatus),
            SchedulePreviousStatus: this.fb.control(delReq.SchedulePreviousStatus),
            OrderId: this.fb.control(delReq.OrderId, orderIdValidators),
            CreatedByRegionId: this.fb.control(delReq.CreatedByRegionId),
            AssignedToRegionId: this.fb.control(delReq.AssignedToRegionId),
            PickupLocationType: this.fb.control(delReq.PickupLocationType),
            Terminal: this.getTerminalForm(delReq.Terminal, isTerminalPickup),
            BulkPlant: this.getBulkPlantForm(delReq.BulkPlant, isBulkplantPickup),
            UoM: this.fb.control(delReq.UoM),
            ParentId: this.fb.control(delReq.ParentId),
            DeliveryGroupId: this.fb.control(delReq.DeliveryGroupId),
            DeliveryScheduleId: this.fb.control(delReq.DeliveryScheduleId),
            TrackableScheduleId: this.fb.control(delReq.TrackableScheduleId),
            CustomerCompany: this.fb.control(delReq.CustomerCompany),
            TrackScheduleEnrouteStatus: this.fb.control(delReq.TrackScheduleEnrouteStatus),
            ProductSequence: this.fb.control(delReq.ProductSequence),
            TrackScheduleStatus: this.fb.control(delReq.TrackScheduleStatus),
            TrackScheduleStatusName: this.fb.control(delReq.TrackScheduleStatusName),
            StatusClassId: this.fb.control(delReq.StatusClassId),
            IsNotCompatible: this.fb.control(delReq.IsNotCompatible),
            IsBlinkDR: this.fb.control(false),
            IsAutoCreatedDR: this.fb.control(delReq.IsAutoCreatedDR),
            BadgeNo1: this.fb.control(delReq.BadgeNo1),
            BadgeNo2: this.fb.control(delReq.BadgeNo2),
            BadgeNo3: this.fb.control(delReq.BadgeNo3),
            IsCommonBadge: this.fb.control(delReq.IsCommonBadge),
            SourceTripId: this.fb.control(delReq.SourceTripId),
            DispactherNote: this.fb.control(delReq.DispactherNote),
            Notes: this.fb.control(delReq.Notes),
            isRecurringSchedule: this.fb.control(delReq.isRecurringSchedule),
            RecurringScheduleId: this.fb.control(delReq.RecurringScheduleId),
            ScheduleQuantityType: this.fb.control(delReq.ScheduleQuantityType),
            ScheduleQuantityTypeText: this.fb.control(delReq.ScheduleQuantityTypeText),
            RouteInfo: this.fb.control(null),
            CarrierStatus: this.fb.control(delReq.CarrierStatus),
            DeliveryRequestFor: this.fb.control(delReq.DeliveryRequestFor),
            Compartments: isShiftTrailerChange == 0 ? this.getCompartments(delReq.Compartments) : this.fb.array([]),
            IsFilldInvoke: this.fb.control(false),
            TruckLoadType: this.fb.control(''),
            IsRetainFuelLoaded: this.fb.control(false),
            DelReqSource: this.fb.control(delReq.DelReqSource),
            IsPreloadDisable: this.fb.control(delReq.IsPreloadDisable),
            IsSpiltDRIconVisible: this.fb.control(delReq.IsSpiltDRIconVisible),
            GroupParentDRId: this.fb.control(delReq.GroupParentDRId),
            GroupChildDRs: this.fb.control(delReq.GroupChildDRs),
            DeliveryWindow: this.fb.control(delReq.DeliveryWindow),
            IsAcceptNightDeliveries: this.fb.control(delReq.IsAcceptNightDeliveries),
            TrailerTypes: this.fb.control(delReq.TrailerTypes),
            LoadQueueAttributes: this.fb.control(delReq.LoadQueueAttributes),
            DRQueueAttributes: this.fb.control(delReq.DRQueueAttributes),
            HoursToCoverDistance: this.fb.control(delReq.HoursToCoverDistance),
            CustomerBrandId: this.fb.control(delReq.CustomerBrandId),
            IsJobFilter: this.fb.control(true),
            IsSelectedForAssignment: this.fb.control(false),
            IsTBD: this.fb.control(delReq.IsTBD),
            TBDGroupId: this.fb.control(delReq.TBDGroupId),
            FuelTypeId: this.fb.control(delReq.FuelTypeId),
            FuelType: this.fb.control(delReq.FuelType),
            TBDLocations: this.fb.control(delReq.TBDLocations),
            DeliveryDateStartTime: this.fb.control(delReq.DeliveryDateStartTime),
            Vessel: this.fb.control(delReq.Vessel),
            Berth: this.fb.control(delReq.Berth),
            IsMarine: this.fb.control(delReq.IsMarine),
            //BLENDED DR
            IsBlendedRequest: this.fb.control(delReq.IsBlendedRequest || false),
            IsCommonPickupForBlend: this.fb.control(delReq.IsCommonPickupForBlend || false),
            BlendedGroupId: this.fb.control(delReq.BlendedGroupId || null),
            BlendParentProductTypeId: this.fb.control(delReq.BlendParentProductTypeId),
            IsBlendedDrParent: this.fb.control(delReq.IsBlendedDrParent || false),
            TotalBlendedQuantity: this.fb.control(delReq.TotalBlendedQuantity),
            BlendDrScheduleStatus: this.fb.control(delReq.BlendDrScheduleStatus),
            BlendedProductName: this.fb.control(delReq.BlendedProductName || null),
            IsAdditive: this.fb.control(delReq.IsAdditive),
            AdditiveProductName: this.fb.control(delReq.AdditiveProductName || null),
            SelectedDate: this.fb.control(delReq.SelectedDate),
            IsFutureDR: this.fb.control(delReq.IsFutureDR),
            IsCalendarView: this.fb.control(delReq.IsCalendarView),
            IsDispatcherDragDrop: this.fb.control(delReq.IsDispatcherDragDrop),
            DispatcherDragDropSequence: this.fb.control(delReq.DispatcherDragDropSequence),
            DeliveryLevelPO: this.fb.control(delReq.DeliveryLevelPO),
            IndicativePrice: this.fb.control(delReq.IndicativePrice),
            ScheduleStartTime: this.fb.control(delReq.ScheduleStartTime),
            ScheduleEndTime: this.fb.control(delReq.ScheduleEndTime),
        });
        if (delReq != null && delReq != undefined) {
            if (delReq.PreLoadedFor) {
                _drForm.addControl('PreLoadedFor', this.fb.control(delReq.PreLoadedFor));
            }
            if (delReq.PostLoadedFor) {
                _drForm.addControl('PostLoadedFor', this.fb.control(delReq.PostLoadedFor));
            }
            if (delReq.PreLoadInfo) {
                _drForm.addControl('PreLoadInfo', this.fb.group({
                    ShiftId: delReq.PreLoadInfo.ShiftId,
                    ScheduleIndex: delReq.PreLoadInfo.ScheduleIndex,
                    TripIndex: delReq.PreLoadInfo.TripIndex,
                    DrId: delReq.PreLoadInfo.DrId
                }));
            }
            if (delReq.PostLoadInfo) {
                _drForm.addControl('PostLoadInfo', this.fb.group({
                    ShiftId: delReq.PostLoadInfo.ShiftId,
                    ScheduleIndex: delReq.PostLoadInfo.ScheduleIndex,
                    TripIndex: delReq.PostLoadInfo.TripIndex,
                    DrId: delReq.PostLoadInfo.DrId
                }));
            }
            if (delReq.BulkPlant == null) {
                delReq.BulkPlant = new DropAddressModel();
            }
            if (delReq.Terminal == null) {
                delReq.Terminal = new DropdownItem();
            }
            _drForm.patchValue(delReq);
        }
        return _drForm;
    }
    getDeliveryRequestFormNew(delReq: DeliveryRequestViewModel): FormGroup {
        let _drForm = this.fb.group({
            Id: this.fb.control(delReq.Id),
            JobId: this.fb.control(delReq.JobId),
            JobAddress: this.fb.control(delReq.JobAddress),
            JobCity: this.fb.control(delReq.JobCity),
            JobName: this.fb.control(delReq.JobName),
            ProductType: this.fb.control(delReq.ProductType),
            ProductTypeId: this.fb.control(delReq.ProductTypeId),
            SiteId: this.fb.control(delReq.SiteId),
            RequiredQuantity: this.fb.control(delReq.RequiredQuantity),
            Priority: this.fb.control(delReq.Priority),
            AssignedToCompanyId: this.fb.control(delReq.AssignedToCompanyId),
            CreatedByCompanyId: this.fb.control(delReq.CreatedByCompanyId),
            SupplierCompanyId: this.fb.control(delReq.SupplierCompanyId),
            Status: this.fb.control(delReq.Status),
            CreatedByRegionId: this.fb.control(delReq.CreatedByRegionId),
            AssignedToRegionId: this.fb.control(delReq.AssignedToRegionId),
            UoM: this.fb.control(delReq.UoM),
            OrderId: this.fb.control(delReq.OrderId),
            ParentId: this.fb.control(delReq.ParentId),
            CustomerCompany: this.fb.control(delReq.CustomerCompany),
            TankId: this.fb.control(delReq.TankId),
            StorageId: this.fb.control(delReq.StorageId),
            TankMaxFill: this.fb.control(delReq.TankMaxFill),
            IsMaxFillAllowed: this.fb.control(delReq.IsMaxFillAllowed),
            IsAutoCreatedDR: this.fb.control(delReq.IsAutoCreatedDR),
            BadgeNo1: this.fb.control(delReq.BadgeNo1),
            BadgeNo2: this.fb.control(delReq.BadgeNo2),
            BadgeNo3: this.fb.control(delReq.BadgeNo3),
            IsCommonBadge: this.fb.control(delReq.IsCommonBadge),
            Notes: this.fb.control(delReq.Notes),
            DispactherNote: this.fb.control(delReq.DispactherNote),
            isRecurringSchedule: this.fb.control(delReq.isRecurringSchedule),
            RecurringScheduleId: this.fb.control(delReq.RecurringScheduleId),
            ScheduleQuantityType: this.fb.control(delReq.ScheduleQuantityType),
            RecurringScheduleInfo: this.fb.control(delReq.RecurringScheduleInfo),
            ScheduleQuantityTypeText: this.fb.control(delReq.ScheduleQuantityTypeText),
            RouteInfo: this.fb.control(delReq.RouteInfo),
            CarrierStatus: this.fb.control(delReq.CarrierStatus),
            DeliveryRequestFor: this.fb.control(delReq.DeliveryRequestFor),
            DeliveryWindow: this.fb.control(delReq.DeliveryWindow),
            DelReqSource: this.fb.control(delReq.DelReqSource),
            IsSpiltDRIconVisible: this.fb.control(delReq.IsSpiltDRIconVisible),
            GroupParentDRId: this.fb.control(delReq.GroupParentDRId),
            ProductSequence: this.fb.control(delReq.ProductSequence),
            Terminal: this.getTerminalFormNew(delReq.Terminal),
            Bulkplant: this.getBulkPlantNew(delReq.BulkPlant),
            IsBrokered: this.fb.control(delReq.IsBrokered),
            IsAcceptNightDeliveries: this.fb.control(delReq.IsAcceptNightDeliveries),
            TrailerTypes: this.fb.control(delReq.TrailerTypes),
            HoursToCoverDistance: this.fb.control(delReq.HoursToCoverDistance),
            LoadQueueAttributes: this.fb.control(delReq.LoadQueueAttributes),
            DRQueueAttributes: this.fb.control(delReq.DRQueueAttributes),
            CustomerBrandId: this.fb.control(delReq.CustomerBrandId),
            IsSelectedForAssignment: this.fb.control(false),
            IsTBD: this.fb.control(delReq.IsTBD),
            TBDGroupId: this.fb.control(delReq.TBDGroupId),
            FuelTypeId: this.fb.control(delReq.FuelTypeId),
            FuelType: this.fb.control(delReq.FuelType),
            OrderType: this.fb.control(delReq.OrderType),
            DeliveryDateStartTime: this.fb.control(delReq.DeliveryDateStartTime),
            Vessel: this.fb.control(delReq.Vessel),
            Berth: this.fb.control(delReq.Berth),
            IsMarine: this.fb.control(delReq.IsMarine),
            CreditApprovalFilePath: this.fb.control(delReq.CreditApprovalFilePath),
            //BLENDED DR
            IsAdditive: this.fb.control(delReq.IsAdditive || false),
            IsBlendedRequest: this.fb.control(delReq.IsBlendedRequest || false),
            IsCommonPickupForBlend: this.fb.control(delReq.IsCommonPickupForBlend || false),
            BlendedGroupId: this.fb.control(delReq.BlendedGroupId || null),
            BlendParentProductTypeId: this.fb.control(delReq.BlendParentProductTypeId),
            BlendDrScheduleStatus: this.fb.control(delReq.BlendDrScheduleStatus),
            BlendedProductName: this.fb.control(delReq.BlendedProductName || null),
            AdditiveProductName: this.fb.control(delReq.AdditiveProductName || null),
            IsBlendedDrParent: this.fb.control(delReq.IsBlendedDrParent || false),
            TotalBlendedQuantity: this.fb.control(delReq.TotalBlendedQuantity),
            SelectedDate: this.fb.control(delReq.SelectedDate),
            IsFutureDR: this.fb.control(delReq.IsFutureDR),
            IsCalendarView: this.fb.control(delReq.IsCalendarView),
            IsDispatcherDragDrop: this.fb.control(delReq.IsDispatcherDragDrop),
            DispatcherDragDropSequence: this.fb.control(delReq.DispatcherDragDropSequence),
            DeliveryLevelPO: this.fb.control(delReq.DeliveryLevelPO),
            ScheduleStartTime: this.fb.control(delReq.ScheduleStartTime),
            ScheduleEndTime : this.fb.control(delReq.ScheduleEndTime),
            IndicativePrice: this.fb.control(delReq.IndicativePrice),
        });
        return _drForm;
    }

    private getCompartments(compartments: CompartmentInfo[]): FormArray {
        let _cArray = this.fb.array([]);
        compartments.forEach(x => {
            _cArray.push(this.getCompartmentsFormGroupForDR(x));
        });
        return _cArray;
    }

    public getCompTerminalForm(terminal: DropdownItem): FormGroup {
        var _tform = this.fb.group({
            Id: this.fb.control(''),
            Name: this.fb.control('')
        });
        if (terminal) {
            _tform.patchValue(terminal);
        }
        return _tform;
    }

    public getTerminalForm(terminal: DropdownItem, isTerminalPickup: boolean, isDemandForm: boolean = false): FormGroup {
        var _tform = this.fb.group({
            Id: this.fb.control(''),
            // Name: this.fb.control('', (isTerminalPickup && !isDemandForm ? Validators.required : null))
            Name: this.fb.control('')
        });
        // SelectedTerminal : this.fb.control('')
        if (isTerminalPickup && terminal) {
            _tform.patchValue(terminal);
        }
        return _tform;
    }

    public getDriverForm(driver: any): FormGroup {
        return this.fb.group({
            Id: this.fb.control(driver.Id),
            Name: this.fb.control(driver.Name),
            Shifts: this.fb.control(driver.Shifts),
            IsFilldCompatible: this.fb.control(driver.IsFilldCompatible)
        });
    }

    public getAssignedTrailerForm(x: TrailerViewModel): FormGroup {
        return this.fb.group({
            Id: this.fb.control(x.Id),
            TrailerId: this.fb.control(x.TrailerId),
            StartTime: this.fb.control(x.StartTime),
            EndTime: this.fb.control(x.EndTime),
            Compartments: this.fb.control(x.Compartments),
            TrailerType: this.fb.control(x.TrailerType),
            IsFilldCompatible: this.fb.control(x.IsFilldCompatible)
        });
    }

    public getDriverTrailerForm() {
        var _dtForm = this.fb.group({
            Driver: this.fb.control(null, [Validators.required]),
            Trailers: this.fb.control([]),
            Index1: this.fb.control(0),
            Index2: this.fb.control(0),
            StartTime: this.fb.control(''),
            EndTime: this.fb.control(''),
            Compartments: this.fb.control(''),
            TrailerType: this.fb.control(''),
            IsFilldCompatible: this.fb.control(false),
            IsIncludeAllRegionDriver: this.fb.control(false)
        });
        return _dtForm;
    }

    public getCompBulkPlantForm(bulkPlant: DropAddressModel) {
        var _bform = this.fb.group({
            Address: this.fb.control(''),
            City: this.fb.control(''),
            State: this.fb.group({ Id: this.fb.control(''), Code: this.fb.control('') }),
            Country: this.fb.group({ Id: this.fb.control(''), Code: this.fb.control('') }),
            CountryGroup: this.fb.group({ Id: this.fb.control(''), Code: this.fb.control('') }),
            ZipCode: this.fb.control(''),
            CountyName: this.fb.control(''),
            Latitude: this.fb.control(''),
            Longitude: this.fb.control(''),
            SiteName: this.fb.control(''),
            SiteId: this.fb.control('')
        });
        if (bulkPlant) {
            _bform.patchValue(bulkPlant);
        }
        return _bform;
    }

    public getBulkPlantForm(bulkPlant: DropAddressModel, isLocationPickup: boolean, isDemandForm: boolean = false) {
        var _bform = this.fb.group({

            Address: this.fb.control(''),
            City: this.fb.control(''),
            State: this.fb.group({ Id: this.fb.control(''), Code: this.fb.control('') }),
            Country: this.fb.group({ Id: this.fb.control(''), Code: this.fb.control('') }),
            CountryGroup: this.fb.group({ Id: this.fb.control(''), Code: this.fb.control('') }),
            ZipCode: this.fb.control(''),
            CountyName: this.fb.control(''),
            Latitude: this.fb.control(''),
            Longitude: this.fb.control(''),
            SiteName: this.fb.control(''),
            SiteId: this.fb.control('')
        });
        if (isLocationPickup && bulkPlant) {
            _bform.patchValue(bulkPlant);
        }
        return _bform;
    }

    public getTrailerTrip(startTime: Date, slotPeriod: number): TripModel {
        if (slotPeriod <= 0) { slotPeriod = 3; }
        let trip = new TripModel();
        trip.StartDate = moment(startTime).format('MM/DD/YYYY');
        trip.StartTime = moment(startTime).format('hh:mm A');
        let endTime = moment(startTime).add(slotPeriod, 'hours').toDate();
        trip.EndTime = moment(endTime).format('hh:mm A');
        trip.IsEditable = true;
        trip.IsPreloadDisable = false;
        return trip;
    }

    public getLoadForm(): FormGroup {
        return this.fb.group({
            JobId: this.fb.control(null),
            RegionId: this.fb.control(null),
            OrderList: this.fb.control([]),
            Demands: this.fb.array([])
        });
    }

    public initCreateLoadFormArray(formArray: FormArray, demandModels: DemandModel[], regionId: string): void {
        formArray.clear();
        demandModels.forEach(x => {
            formArray.push(this.getDemandForm(x, regionId));
        });
    }

    private getDemandForm(demandModel: DemandModel, regionId: string): FormGroup {
        let quantityValidators = [];
        if (demandModel.ScheduleQuantityType == 1 || demandModel.ScheduleQuantityType == 0) {
            quantityValidators = [Validators.required, Validators.min(0.00001), Validators.pattern(RegExConstants.DecimalNumber)];
            //if (demandModel.TankMaxFill > 0) {
            //    quantityValidators.push(Validators.max(demandModel.TankMaxFill));
            //}
        }
        return this.fb.group({
            Id: this.fb.control(demandModel.Id),
            SiteId: this.fb.control(demandModel.SiteId),
            JobId: this.fb.control(demandModel.JobId),
            JobName: this.fb.control(demandModel.JobName),
            OrderId: this.fb.control(demandModel.OrderId),
            TankId: this.fb.control(demandModel.TankId),
            StorageId: this.fb.control(demandModel.StorageId),
            RequiredQuantity: this.fb.control(demandModel.RequiredQuantity || '', quantityValidators),
            Priority: this.fb.control(demandModel.Priority),
            CreatedByRegionId: this.fb.control(regionId),
            CustomerCompany: this.fb.control(demandModel.BuyerCompanyName),
            //CurrentThreshold: this.fb.control(demandModel.cu),
            TankMaxFill: this.fb.control(demandModel.TankMaxFill),
            IsMaxFillAllowed: this.fb.control(demandModel.IsMaxFillAllowed),
            //IsReAssignToCarrier: this.fb.control(demandModel.IsReAssignToCarrier),
            TankMinFill: this.fb.control(demandModel.TankMinFill),
            TankCapacity: this.fb.control(demandModel.TankCapacity),
            TankName: this.fb.control(demandModel.TankName),
            ProductName: this.fb.control(demandModel.ProductName),
            ProductType: this.fb.control(demandModel.ProductType),
            Level: this.fb.control(demandModel.Level),
            NetVolume: this.fb.control(demandModel.NetVolume),
            Ullage: this.fb.control(demandModel.Ullage),
            UoM: this.fb.control(demandModel.UoM),
            IsDRExists: this.fb.control(demandModel.IsDRExists),
            IsDRMissed: this.fb.control(demandModel.IsDRMissed),
            isRecurringSchedule: this.fb.control(demandModel.isRecurringSchedule),
            ScheduleQuantityType: this.fb.control(demandModel.ScheduleQuantityType || 1),
            ScheduleQuantityTypeText: this.fb.control(demandModel.ScheduleQuantityTypeText || 'Quantity'),
            DeliveryReqId: this.fb.control(demandModel.DeliveryReqId),
            CarrierStatus: this.fb.control(demandModel.CarrierStatus),
            DeliveryRequestFor: this.fb.control(demandModel.DeliveryRequestFor),
            IsEndSupplier: this.fb.control(demandModel.IsEndSupplier),
            IsDispatchRetained: this.fb.control(demandModel.IsDispatchRetained),
            Notes: this.fb.control(demandModel.Notes),
            IsRetainFuelLoaded: this.fb.control(false),
            GroupParentDRId: this.fb.control(demandModel.GroupParentDRId),
            ProductSequence: this.fb.control(demandModel.ProductSequence),
            BadgeNo1: this.fb.control(demandModel.BadgeNo1 ? demandModel.BadgeNo1 : null),
            BadgeNo2: this.fb.control(demandModel.BadgeNo2 ? demandModel.BadgeNo2 : null),
            BadgeNo3: this.fb.control(demandModel.BadgeNo3 ? demandModel.BadgeNo3 : null),
            PickupLocationType: this.fb.control(demandModel.PickupLocationType),
            Terminal: this.getTerminalForm(demandModel.Terminal, demandModel.PickupLocationType != 2, true),
            BulkPlant: this.getBulkPlantForm(demandModel.BulkPlant, demandModel.PickupLocationType == 2, true),
            IsAcceptNightDeliveries: this.fb.control(demandModel.IsAcceptNightDeliveries),
            LoadQueueAttributes: this.fb.control(demandModel.LoadQueueAttributes),
            DRQueueAttributes: this.fb.control(demandModel.DRQueueAttributes),
            TrailerTypes: this.fb.control(demandModel.TrailerTypes),
            HoursToCoverDistance: this.fb.control(demandModel.HoursToCoverDistance),
            IsJobFilter: this.fb.control(true),
            ScheduleStartTime: this.fb.control(demandModel.ScheduleStartTime),
            ScheduleEndTime: this.fb.control(demandModel.ScheduleEndTime),
        });
    }

    public getLoadInfoForm(data: LoadInfo): FormGroup {
        return this.fb.group({
            ShiftId: this.fb.control(data.ShiftId),
            ScheduleIndex: this.fb.control(data.ScheduleIndex),
            TripIndex: this.fb.control(data.TripIndex),
            DrId: this.fb.control(data.DrId)
        });
    }

    public getCompartmentViewForm(trips: TripModel[], firstTrailerId: string): FormGroup {
        let tripsArray = this.fb.array([]);
        for (var idx = 0; idx < trips.length; idx++) {
            tripsArray.push(this.getCompartmentViewLoadFormGroup(trips[idx], firstTrailerId));
        }
        var _tForm = this.fb.group({
            Trips: tripsArray
        });
        return _tForm;
    }

    private getCompartmentViewLoadFormGroup(x: TripModel, firstTrailerId: string): FormGroup {
        let commonPickupName = x.IsCommonPickup ? (x.PickupLocationType == 2 ? x.BulkPlant.SiteName : x.Terminal.Name) : null;
        var _tForm = this.fb.group({
            DeliveryRequests: this.getCompartmentViewDRArray(x.DeliveryRequests, x.IsCommonPickup, commonPickupName, firstTrailerId),
            ShiftIndex: this.fb.control(x.ShiftIndex),
            DriverRowIndex: this.fb.control(x.DriverRowIndex),
            DriverColIndex: this.fb.control(x.DriverColIndex),
            IsEditable: this.fb.control(x.IsEditable),
            StartTime: this.fb.control(x.StartTime)
        });
        return _tForm;
    }

    private getCompartmentViewDRArray(delReqs: DeliveryRequestViewModel[], isCommonPickup: boolean, commonPickupName: string, firstTrailerId: string): FormArray {
        var _drArray = this.fb.array([]);
        delReqs.forEach(x => {
            var _form = this.getCompartmentViewDRForm(x, isCommonPickup, commonPickupName, firstTrailerId);
            _drArray.push(_form);
        });
        return _drArray;
    }

    private getCompartmentViewDRForm(delReq: DeliveryRequestViewModel, isCommonPickup: boolean, commonPickupName: string, firstTrailerId: string): FormGroup {
        let pickupName = isCommonPickup ? commonPickupName : (delReq.PickupLocationType == 2 ? delReq.BulkPlant.SiteName : delReq.Terminal.Name);
        let disabledControl = false;
        if (delReq.TrackScheduleEnrouteStatus == 16 || delReq.TrackScheduleStatus == 7 || delReq.TrackScheduleStatus == 8 || delReq.TrackScheduleStatus == 9 || delReq.TrackScheduleEnrouteStatus == 21 || delReq.TrackScheduleStatus == 25) {
            disabledControl = true;
        }
        else if (delReq.PreLoadInfo != null) {
            disabledControl = true;
        }
        var _drForm = this.fb.group({
            Id: this.fb.control(delReq.Id),
            UoM: this.fb.control(delReq.UoM),
            ProductType: this.fb.control(delReq.ProductType),
            PickupName: this.fb.control(pickupName),
            RequiredQuantity: this.fb.control(delReq.RequiredQuantity),
            DrQuantity: this.fb.control(delReq.RequiredQuantity),
            ScheduleQuantityType: this.fb.control(delReq.ScheduleQuantityType),
            ScheduleQuantityTypeText: this.fb.control(delReq.ScheduleQuantityTypeText),
            disabledControl: this.fb.control(disabledControl),
            Compartments: this.getCompartmentsFormArray(delReq, delReq.Compartments, firstTrailerId, disabledControl),
            PickupLocationType: this.fb.control(0),
            Terminal: this.getCompTerminalForm(delReq.Terminal),
            BulkPlant: this.getCompBulkPlantForm(delReq.BulkPlant),
            IsRetainFuelLoaded: this.fb.control(delReq.IsRetainFuelLoaded),
            OrderId: this.fb.control(delReq.OrderId),
            ProductTypeId: this.fb.control(delReq.ProductTypeId),
            FuelTypeId: this.fb.control(delReq.FuelTypeId),
            ScheduleStartTime: this.fb.control(delReq.ScheduleStartTime),
            ScheduleEndTime: this.fb.control(delReq.ScheduleEndTime),
            FuelType: this.fb.control(delReq.FuelType)
        });
        return _drForm;
    }

    private getCompartmentsFormArray(delReq: DeliveryRequestViewModel, compartments: CompartmentModel[], firstTrailerId: string, disabledControl: boolean): FormArray {
        var _drArray = this.fb.array([]);
        if (compartments.length == 0 && !disabledControl) {
            let comp = new CompartmentInfo();
            comp.TrailerId = firstTrailerId;
            compartments.push(comp);
        }
        compartments.forEach(x => {
            _drArray.push(this.getCompartmentsFormGroup(delReq, x, disabledControl));
        });
        return _drArray;
    }

    public getCompartmentsFormGroup(delReq: DeliveryRequestViewModel, comp: CompartmentModel, disabledControl: boolean): FormGroup {
        let quantityValidators = [];
        if (delReq.ScheduleQuantityType <= 1) {
            quantityValidators = [Validators.required, Validators.pattern(RegExConstants.DecimalNumber), Validators.min(0.00000001), Validators.max(delReq.RequiredQuantity)];
        }
        if (disabledControl) {
            return this.fb.group({
                TrailerId: this.fb.control({ value: comp.TrailerId, disabled: disabledControl }),
                CompartmentId: this.fb.control({ value: comp.CompartmentId, disabled: disabledControl }),
                Quantity: this.fb.control(comp.Quantity)
            });
        }
        else {
            return this.fb.group({
                TrailerId: this.fb.control(comp.TrailerId, [Validators.required]),
                CompartmentId: this.fb.control(comp.CompartmentId, [Validators.required]),
                Quantity: this.fb.control(comp.Quantity, quantityValidators)
            });
        }
    }

    public getCompartmentsFormGroupForDR(comp: CompartmentModel): FormGroup {
        return this.fb.group({
            TrailerId: this.fb.control(comp.TrailerId),
            CompartmentId: this.fb.control(comp.CompartmentId),
            Quantity: this.fb.control(comp.Quantity)
        });
    }

    getBlendRequestFormArray(models?: BlendedRequest[]): FormArray {

        var _blendedDrArray = this.fb.array([]);

        if (models && models.length > 0) {
            models.forEach(model => {
                _blendedDrArray.push(this.getBlendRequestFormGroup(model));
            });
        }

        return _blendedDrArray;
    }

    getBlendRequestFormGroup(model: BlendedRequest): FormGroup {

        return this.fb.group({
            Id: this.fb.control(model.Id),
            OrderId: this.fb.control(model.OrderId, [Validators.required]),
            PoNumber: this.fb.control(model.PoNumber),
            ProductType: this.fb.control(model.ProductType),
            ProductTypeId: this.fb.control(model.ProductTypeId),
            RequiredQuantity: this.fb.control(model.RequiredQuantity, [Validators.required, Validators.min(NumberConstants.MinQuantity)]),
            QuantityInPercent: this.fb.control(model.QuantityInPercent, model.IsAdditive ? [] : [Validators.required, Validators.min(NumberConstants.MinPercent), Validators.max(NumberConstants.MaxPercent)]),
            PickupLocationType: this.fb.control(model.PickupLocationType),
            Terminal: this.getTerminalFormNew(model.Terminal),
            BulkPlant: this.getBulkPlantNew(model.BulkPlant),
            IsAdditive: this.fb.control(model.IsAdditive),
            UoM: this.fb.control(model.UoM),
            JobId: this.fb.control(model.JobId),
            CarrierStatus: this.fb.control(model.CarrierStatus),
            IsBlendedRequest: this.fb.control(model.IsBlendedRequest),
            IsCommonPickupForBlend: this.fb.control(model.IsCommonPickupForBlend),
            BlendedGroupId: this.fb.control(model.BlendedGroupId),
            BlendParentProductTypeId: this.fb.control(model.BlendParentProductTypeId),
            BlendedProductName: this.fb.control(model.BlendedProductName),
            AdditiveProductName: this.fb.control(model.AdditiveProductName),
            IsBlendedDrParent: this.fb.control(model.IsBlendedDrParent),
            TotalBlendedQuantity: this.fb.control(model.TotalBlendedQuantity),
            SchedulePreviousStatus: this.fb.control(model.SchedulePreviousStatus),
            ScheduleStatus: this.fb.control(model.ScheduleStatus),
            TrackScheduleStatus: this.fb.control(model.TrackScheduleStatus),
            TrackableScheduleId: this.fb.control(model.TrackableScheduleId),
            SelectedDate: this.fb.control(model.SelectedDate),
            IsFutureDR: this.fb.control(model.IsFutureDR),
            IsCalendarView: this.fb.control(model.IsCalendarView),
            IsDispatcherDragDrop: this.fb.control(model.IsDispatcherDragDrop),
            DispatcherDragDropSequence: this.fb.control(model.DispatcherDragDropSequence),
            ScheduleStartTime: this.fb.control(model.ScheduleStartTime),
            ScheduleEndTime: this.fb.control(model.ScheduleEndTime),
            DeliveryLevelPO: this.fb.control(model.DeliveryLevelPO)
        });
    }

    public getTerminalFormNew(terminal: DropdownItem): FormGroup {
        let _tform = this.fb.group({ Id: this.fb.control(''), Name: this.fb.control('') });
        if (terminal) { _tform.patchValue(terminal); }
        return _tform;
    }

    public getBulkPlantNew(bulkPlant: DropAddressModel) {

        let _bform = this.fb.group({
            Address: this.fb.control(''),
            City: this.fb.control(''),
            State: this.fb.group({ Id: this.fb.control(''), Code: this.fb.control('') }),
            Country: this.fb.group({ Id: this.fb.control(''), Code: this.fb.control('') }),
            CountryGroup: this.fb.group({ Id: this.fb.control(''), Code: this.fb.control('') }),
            ZipCode: this.fb.control(''),
            CountyName: this.fb.control(''),
            Latitude: this.fb.control(''),
            Longitude: this.fb.control(''),
            SiteName: this.fb.control(''),
            SiteId: this.fb.control('')
        });

        if (bulkPlant) {
            if (!bulkPlant.CountryGroup) {
                bulkPlant.CountryGroup = new DropdownItem();
            }
            _bform.patchValue(bulkPlant);
        }
        return _bform;
    }

    isValidOptimizedCapacityTrip(assignedTrailers: TrailerViewModel[], allTrailers: TrailerViewModel[], trip: TripModel) {
        
        let isValidTrip = true;

        if (trip.DeliveryRequests.length > 0) {
            
            let totalOptimizedCapacity = allTrailers.filter(t => assignedTrailers.some(t2 => t2.TrailerId == t.TrailerId)).reduce((a, b) => +a + +b.OptimizedCapacity, 0)
            let tripQuantity = trip.DeliveryRequests.filter(dr => dr.ScheduleQuantityType == 1).reduce((a, b) => +a + +b.RequiredQuantity, 0);
            
            if (tripQuantity != totalOptimizedCapacity) {
                isValidTrip = false;
            }
        }

        return isValidTrip;
    }
}