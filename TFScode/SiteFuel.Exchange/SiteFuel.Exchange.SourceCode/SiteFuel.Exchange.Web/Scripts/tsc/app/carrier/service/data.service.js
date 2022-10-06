import { __decorate } from "tslib";
import { Injectable } from '@angular/core';
import { Subject, BehaviorSubject } from 'rxjs';
let DataService = class DataService {
    constructor() {
        this.FormChangeSubscription = [];
        this.drQueueChangesForFilter = new Subject();
        this.DsbQueueChangesForNotification = new Subject();
        this.DriverColumnChangeDetect = new Subject();
        this.OpenOnTheFlyLocationFormSubject = new Subject();
        this.getOnTheFlyLocationDetailsSubject = new Subject();
        this.OnTheFlyDetectSubject = new Subject();
        this.LoadLocationSequenceSubject = new Subject();
        this.DropTerminalSelectedSubject = new Subject();
        this.RemoveFeesSubject = new Subject();
        this.ShowDeliveryGroupSubject = new Subject();
        this.ShowOpenedDeliveryGroupSubject = new Subject();
        this.EditDeliveryGroupSubject = new Subject();
        this.DraftDeliveryGroupSubject = new Subject();
        this.SaveModifiedLoadsSubject = new Subject();
        this.PublishDeliveryGroupSubject = new Subject();
        this.CancelDeliveryGroupSubject = new Subject();
        this.RestoreDeletedRequestSubject = new Subject();
        this.DrUpdatedSubject = new Subject();
        this.RemoveDroppedRequestSubject = new Subject();
        this.DeleteDeliveryGroupSubject = new Subject();
        this.AcceptRejectDRSubject = new Subject();
        this.UnsavedChangesSubject = new Subject();
        this.SavedChangesSubject = new Subject();
        this.EmptyUnsavedChangesSubject = new Subject();
        this.TrailerShiftsSubject = new BehaviorSubject(null);
        this.IsLoadingButtonSubject = new BehaviorSubject(false);
        this.DeleteDRRequestSubject = new Subject();
        this.DisableDSBControlsSubject = new BehaviorSubject(false);
        this.EditDriverTrailerSubject = new Subject();
        this.UpdateDriverTrailerSubject = new Subject();
        this.SaveDriverAssignmentSubject = new Subject();
        this.AllShiftsSubject = new BehaviorSubject([]);
        this.AllTrailersSubject = new BehaviorSubject([]);
        this.AllDeliveryRequestsSubject = new BehaviorSubject([]);
        this.PublishEntireRowSubject = new Subject();
        this.GroupChangeDetectSubject = new Subject();
        this.ScheduleChangeDetectSubject = new Subject();
        this.DragDropItemSubject = new Subject();
        this.CreateLoadChangeSubject = new Subject();
        this.CreateLoadCancelSubject = new BehaviorSubject({});
        this.CreateLoadSuccessSubject = new Subject();
        this.CreatePreloadSubject = new Subject();
        this.UpdatePostloadSubject = new Subject();
        this.DeletePostloadSubject = new Subject();
        this.DeliveryPreloadOption = new Subject();
        this.ResetDrByRoutesSubject = new Subject();
        this.EditCompartmentAssigmentSubject = new Subject();
        this.SaveEditCompartmentAssigmentSubject = new Subject();
        this.OpenDsbOttoBuilderSubject = new Subject();
        this.OpenDsbOttoNotificationSubject = new Subject();
        this.DsbOttoNotificationCountSubject = new Subject();
        this.SplitDrsInfoSubject = new Subject();
        this.TransferDSInfoSubject = new Subject();
        this.TrailerDSInfoSubject = new Subject();
        this.UnAssignTrailerFromShift = new Subject();
        this.RouteDetailsSubject = new Subject();
        this.DeleteRouteDetailsSubject = new Subject();
        this.HideDeliveryGroupSubject = new Subject();
        this.GridViewSearchGroupSubject = new Subject();
        this.RemoveTrailerGroupSubject = new Subject();
        this.ShiftVisibility = new Subject();
        this.RouteResetGroupSubject = new Subject();
        this.RouteDeleteDeliveryGroupSubject = new Subject();
        this.DsbQueueChangesForNotification = new Subject();
        this.OptionalPickupSubject = new Subject();
        this.IsScheduleBuilderSubject = new BehaviorSubject(false);
        this.CancelEntireRowSubject = new Subject();
        this.CancelDSDeliveryGroupSubject = new Subject();
        this.CancelDSDeliveryGroupNormalGroupDSSubject = new Subject();
        this.CancelDSLocationSubject = new Subject();
        this.DeliveryScheduleRemoveSubject = new Subject();
        this.DispatcherLoadDragDropSubject = new Subject();
        this.DispatcherLoadDragDropMapSubject = new Subject();
        this.RefreshDsbOttoBuilderSubject = new Subject();
    }
    setShowDeliveryGroupSubject(data) {
        this.ShowDeliveryGroupSubject.next(data);
    }
    setShowOpenedDeliveryGroupSubject(data) {
        this.ShowOpenedDeliveryGroupSubject.next(data);
    }
    setEditDeliveryGroupSubject(data) {
        this.EditDeliveryGroupSubject.next(data);
    }
    setDraftDeliveryGroupSubject(data) {
        this.DraftDeliveryGroupSubject.next(data);
    }
    setSaveModifiedLoadsSubject(data) {
        this.SaveModifiedLoadsSubject.next(data);
    }
    setPublishDeliveryGroupSubject(data) {
        this.PublishDeliveryGroupSubject.next(data);
    }
    setCancelDeliveryGroupSubject(data) {
        this.CancelDeliveryGroupSubject.next(data);
    }
    setDeleteDeliveryGroupSubject(data) {
        this.DeleteDeliveryGroupSubject.next(data);
    }
    setAcceptRejectDRSubject(data) {
        this.AcceptRejectDRSubject.next(data);
    }
    setRestoreDeletedRequestSubject(data) {
        this.RestoreDeletedRequestSubject.next(data);
    }
    setDrUpdatedSubject(data) {
        this.DrUpdatedSubject.next(data);
    }
    setRemoveDroppedRequestSubject(data) {
        this.RemoveDroppedRequestSubject.next(data);
    }
    setUnsavedChangesSubject(data) {
        this.UnsavedChangesSubject.next(data);
    }
    setSavedChangesSubject(data) {
        this.SavedChangesSubject.next(data);
    }
    setUnsavedChangesAsEmptySubject() {
        this.EmptyUnsavedChangesSubject.next();
    }
    setTrailerShiftsSubject(data) {
        this.TrailerShiftsSubject.next(data);
    }
    setDeleteDRRequestSubject(data) {
        this.DeleteDRRequestSubject.next(data);
    }
    setDisableDSBControls(data) {
        this.DisableDSBControlsSubject.next(data);
    }
    setEditDriverTrailerSubject(data) {
        this.EditDriverTrailerSubject.next(data);
    }
    setUpdateDriverTrailerSubject(data) {
        this.UpdateDriverTrailerSubject.next(data);
    }
    setAllShiftsSubject(data) {
        this.AllShiftsSubject.next(data);
    }
    setAllTrailersSubject(data) {
        this.AllTrailersSubject.next(data);
    }
    setAllDeliveryRequestsSubject(data) {
        data = this.removeDuplicates(data);
        this.AllDeliveryRequestsSubject.next(data);
    }
    removeDuplicates(data) {
        data = data.filter((item, index, array) => index === array.findIndex((find) => find.Id === item.Id));
        return data;
    }
    setSaveDriverAssignmentSubject(data) {
        this.SaveDriverAssignmentSubject.next(data);
    }
    setPublishEntireRowSubject(data) {
        this.PublishEntireRowSubject.next(data);
    }
    setGroupChangeDetectSubject(data) {
        this.GroupChangeDetectSubject.next(data);
    }
    setScheduleChangeDetectSubject(data) {
        this.ScheduleChangeDetectSubject.next(data);
    }
    setDragDropItemSubject(data) {
        this.DragDropItemSubject.next(data);
    }
    setCreateLoadChangeSubject(data) {
        this.CreateLoadChangeSubject.next(data);
    }
    setCreateLoadCancelSubject(data) {
        this.CreateLoadCancelSubject.next(data);
    }
    setCreateLoadSuccessSubject(data) {
        this.CreateLoadSuccessSubject.next(data);
    }
    setCreatePreloadSubject(data) {
        this.CreatePreloadSubject.next(data);
    }
    setUpdatePostloadSubject(data) {
        this.UpdatePostloadSubject.next(data);
    }
    setDeletePostloadSubject(data) {
        this.DeletePostloadSubject.next(data);
    }
    setDeliveryPreloadOption(data) {
        this.DeliveryPreloadOption.next(data);
    }
    setResetDrByRoutesSubject(data) {
        this.ResetDrByRoutesSubject.next(data);
    }
    setEditCompartmentAssigmentSubject(data) {
        this.EditCompartmentAssigmentSubject.next(data);
    }
    setSaveEditCompartmentAssigmentSubject(data) {
        this.SaveEditCompartmentAssigmentSubject.next(data);
    }
    setOpenDsbOttoBuilderSubject(isOpen) {
        this.OpenDsbOttoBuilderSubject.next(isOpen);
    }
    setOpenDsbOttoNotificationSubject(isOpen) {
        this.OpenDsbOttoNotificationSubject.next(isOpen);
    }
    setDsbOttoNotificationCountSubject(isOpen) {
        this.DsbOttoNotificationCountSubject.next(isOpen);
    }
    setSplitDRsInfoSubject(inputData) {
        this.SplitDrsInfoSubject.next(inputData);
    }
    setTransferDSSubject(data) {
        this.TransferDSInfoSubject.next(data);
    }
    setTrailerInformationSubject(data) {
        this.TrailerDSInfoSubject.next(data);
    }
    unassignTrailerInformationSubject(data) {
        this.UnAssignTrailerFromShift.next(data);
    }
    setRouteDetailsSubject(data) {
        this.RouteDetailsSubject.next(data);
    }
    setDeleteRouteDetailsSubject(data) {
        this.DeleteRouteDetailsSubject.next(data);
    }
    setHideDeliveryGroupSubject(data) {
        this.HideDeliveryGroupSubject.next(data);
    }
    setGridViewSearch(data) {
        this.GridViewSearchGroupSubject.next(data);
    }
    setRemoveTrailer(data) {
        this.RemoveTrailerGroupSubject.next(data);
    }
    setShiftVisibility(data) {
        this.ShiftVisibility.next(data);
    }
    setRouteResetGroupSubject(data) {
        this.RouteResetGroupSubject.next(data);
    }
    setRouteDeleteDeliveryGroupSubject(data) {
        this.RouteDeleteDeliveryGroupSubject.next(data);
    }
    setDrQueueChangesForFilter(data) {
        this.drQueueChangesForFilter.next(data);
    }
    setDsbQueueChangesForNotification(data) {
        this.DsbQueueChangesForNotification.next(data);
    }
    setDriverColumnChangeDetect(data) {
        this.DriverColumnChangeDetect.next(data);
    }
    setOptionalPickupInfo(data) {
        this.OptionalPickupSubject.next(data);
    }
    setScheduleLoaderSubject(data) {
        this.IsScheduleBuilderSubject.next(data);
    }
    setOpenOnTheFlyLocationFormSubject(data) {
        this.OpenOnTheFlyLocationFormSubject.next(data);
    }
    setGetOnTheFlyLocationDetailsSubject(data) {
        this.getOnTheFlyLocationDetailsSubject.next(data);
    }
    setOnTheFlyDetectSubject(data) {
        this.OnTheFlyDetectSubject.next(data);
    }
    setCancelEntireRowSubject(data) {
        this.CancelEntireRowSubject.next(data);
    }
    setCancelDSDeliveryGroupSubject(data) {
        this.CancelDSDeliveryGroupSubject.next(data);
    }
    setCancelDSGroupNormalSubDSSubject(data) {
        this.CancelDSDeliveryGroupNormalGroupDSSubject.next(data);
    }
    setCancelDSLocationDSSubject(data) {
        this.CancelDSLocationSubject.next(data);
    }
    setDeliveryScheduleRemoveSubject(data) {
        this.DeliveryScheduleRemoveSubject.next(data);
    }
    setDispatcherLoadDragDropSubject(data) {
        this.DispatcherLoadDragDropSubject.next(data);
    }
    setDispatcherLoadDragDropMapSubject(data) {
        this.DispatcherLoadDragDropMapSubject.next(data);
    }
    setLoadLocationSequenceSubject(data) {
        this.LoadLocationSequenceSubject.next(data);
    }
    refreshDsbOttoBuilderSubject(isRefresh) {
        this.RefreshDsbOttoBuilderSubject.next(isRefresh);
    }
    setDropTerminalSelectedSubject(data) {
        this.DropTerminalSelectedSubject.next(data);
    }
    removeFeesOnCreateNewSubject() {
        this.RemoveFeesSubject.next();
    }
};
DataService = __decorate([
    Injectable({
        providedIn: 'root'
    })
], DataService);
export { DataService };
//# sourceMappingURL=data.service.js.map