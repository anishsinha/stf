import { Injectable } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { Subject, BehaviorSubject, Subscription } from 'rxjs';

@Injectable({
    providedIn: 'root'
})
export class DataService {

    public ShowDeliveryGroupSubject: Subject<boolean>;
    public ShowOpenedDeliveryGroupSubject: Subject<boolean>;
    public EditDeliveryGroupSubject: Subject<any>;
    public DraftDeliveryGroupSubject: Subject<any>;
    public SaveModifiedLoadsSubject: Subject<any>;
    public PublishDeliveryGroupSubject: Subject<any>;
    public CancelDeliveryGroupSubject: Subject<any>;
    public DeleteDeliveryGroupSubject: Subject<any>;
    public AcceptRejectDRSubject: Subject<any>;

    public RestoreDeletedRequestSubject: Subject<any>;
    public DrUpdatedSubject: Subject<any>;
    public RemoveDroppedRequestSubject: Subject<any>;
    public UnsavedChangesSubject: Subject<any>;
    public SavedChangesSubject: Subject<any>;
    public EmptyUnsavedChangesSubject: Subject<any>;
    public TrailerShiftsSubject: BehaviorSubject<any>;
    public IsLoadingButtonSubject: BehaviorSubject<boolean>;
    public FormChangeSubscription: Subscription[] = [];
    public DeleteDRRequestSubject: Subject<any>;
    public DisableDSBControlsSubject: BehaviorSubject<boolean>;
    public EditDriverTrailerSubject: Subject<any>;
    public UpdateDriverTrailerSubject: Subject<any>;
    public SaveDriverAssignmentSubject: Subject<any>;

    public AllShiftsSubject: BehaviorSubject<any>;
    public AllTrailersSubject: BehaviorSubject<any>;
    public AllDeliveryRequestsSubject: BehaviorSubject<any>;

    public PublishEntireRowSubject: Subject<any>;
    public GroupChangeDetectSubject: Subject<any>;
    public ScheduleChangeDetectSubject: Subject<any>;
    public DragDropItemSubject: Subject<any>;

    public CreateLoadChangeSubject: Subject<any>;
    public CreateLoadCancelSubject: BehaviorSubject<any>;
    public CreateLoadSuccessSubject: Subject<any>;

    public CreatePreloadSubject: Subject<any>;
    public UpdatePostloadSubject: Subject<any>;
    public DeletePostloadSubject: Subject<any>;
    public DeliveryPreloadOption: Subject<any>;
    public ResetDrByRoutesSubject: Subject<any>;

    public EditCompartmentAssigmentSubject: Subject<any>;
    public SaveEditCompartmentAssigmentSubject: Subject<any>;
    public OpenDsbOttoBuilderSubject: Subject<any>;
    public OpenDsbOttoNotificationSubject: Subject<any>;
    public DsbOttoNotificationCountSubject: Subject<any>;
    public SplitDrsInfoSubject: Subject<any>;
    public TransferDSInfoSubject: Subject<any>;
    public TrailerDSInfoSubject: Subject<any>;
    public UnAssignTrailerFromShift: Subject<any>;
    public RouteDetailsSubject: Subject<any>;
    public DeleteRouteDetailsSubject: Subject<any>;
    public HideDeliveryGroupSubject: Subject<any>;
    public GridViewSearchGroupSubject: Subject<any>;
    public RemoveTrailerGroupSubject: Subject<any>;
    public ShiftVisibility: Subject<any>;
    public RouteResetGroupSubject: Subject<any>;
    public RouteDeleteDeliveryGroupSubject: Subject<any>;
    public drQueueChangesForFilter: Subject<boolean> = new Subject<boolean>();
    public DsbQueueChangesForNotification: Subject<boolean> = new Subject<boolean>();
    public DriverColumnChangeDetect: Subject<boolean> = new Subject<boolean>();
    public OptionalPickupSubject: Subject<any>;
    public IsScheduleBuilderSubject: BehaviorSubject<boolean>;
    public OpenOnTheFlyLocationFormSubject: Subject<any> = new Subject<any>();
    public getOnTheFlyLocationDetailsSubject: Subject<any> = new Subject<any>();
    public OnTheFlyDetectSubject: Subject<any> = new Subject<any>();
    public CancelEntireRowSubject: Subject<any>;
    public CancelDSDeliveryGroupSubject: Subject<any>;
    public CancelDSDeliveryGroupNormalGroupDSSubject: Subject<any>;
    public CancelDSLocationSubject: Subject<any>;
    public DeliveryScheduleRemoveSubject: Subject<any>;
    public DispatcherLoadDragDropSubject: Subject<any>;
    public DispatcherLoadDragDropMapSubject: Subject<any>;
    public LoadLocationSequenceSubject: Subject<FormGroup> = new Subject<FormGroup>();
    public RefreshDsbOttoBuilderSubject: Subject<any>;
    public DropTerminalSelectedSubject: Subject<number> = new Subject<number>();
    public RemoveFeesSubject: Subject<boolean> = new Subject<boolean>();
    constructor() {
        this.ShowDeliveryGroupSubject = new Subject<boolean>();
        this.ShowOpenedDeliveryGroupSubject = new Subject<boolean>();
        this.EditDeliveryGroupSubject = new Subject<any>();
        this.DraftDeliveryGroupSubject = new Subject<any>();
        this.SaveModifiedLoadsSubject = new Subject<any>();
        this.PublishDeliveryGroupSubject = new Subject<any>();
        this.CancelDeliveryGroupSubject = new Subject<any>();
        this.RestoreDeletedRequestSubject = new Subject<any>();
        this.DrUpdatedSubject = new Subject<any>();
        this.RemoveDroppedRequestSubject = new Subject<any>();
        this.DeleteDeliveryGroupSubject = new Subject<any>();
        this.AcceptRejectDRSubject = new Subject<any>();
        this.UnsavedChangesSubject = new Subject<any>();
        this.SavedChangesSubject = new Subject<any>();
        this.EmptyUnsavedChangesSubject = new Subject<any>();
        this.TrailerShiftsSubject = new BehaviorSubject<any>(null);
        this.IsLoadingButtonSubject = new BehaviorSubject<any>(false);
        this.DeleteDRRequestSubject = new Subject<any>();
        this.DisableDSBControlsSubject = new BehaviorSubject<any>(false);
        this.EditDriverTrailerSubject = new Subject<any>();
        this.UpdateDriverTrailerSubject = new Subject<any>();
        this.SaveDriverAssignmentSubject = new Subject<any>();
        this.AllShiftsSubject = new BehaviorSubject<any>([]);
        this.AllTrailersSubject = new BehaviorSubject<any>([]);
        this.AllDeliveryRequestsSubject = new BehaviorSubject<any>([]);
        this.PublishEntireRowSubject = new Subject<any>();
        this.GroupChangeDetectSubject = new Subject<any>();
        this.ScheduleChangeDetectSubject = new Subject<any>();
        this.DragDropItemSubject = new Subject<any>();
        this.CreateLoadChangeSubject = new Subject<any>();
        this.CreateLoadCancelSubject = new BehaviorSubject<any>({});
        this.CreateLoadSuccessSubject = new Subject<any>();
        this.CreatePreloadSubject = new Subject<any>();
        this.UpdatePostloadSubject = new Subject<any>();
        this.DeletePostloadSubject = new Subject<any>();
        this.DeliveryPreloadOption = new Subject<any>();
        this.ResetDrByRoutesSubject = new Subject<any>();
        this.EditCompartmentAssigmentSubject = new Subject<any>();
        this.SaveEditCompartmentAssigmentSubject = new Subject<any>();
        this.OpenDsbOttoBuilderSubject = new Subject<any>();
        this.OpenDsbOttoNotificationSubject = new Subject<any>();
        this.DsbOttoNotificationCountSubject = new Subject<any>();
        this.SplitDrsInfoSubject = new Subject<any>();
        this.TransferDSInfoSubject = new Subject<any>();
        this.TrailerDSInfoSubject = new Subject<any>();
        this.UnAssignTrailerFromShift = new Subject<any>();
        this.RouteDetailsSubject = new Subject<any>();
        this.DeleteRouteDetailsSubject = new Subject<any>();
        this.HideDeliveryGroupSubject = new Subject<any>();
        this.GridViewSearchGroupSubject = new Subject<any>();
        this.RemoveTrailerGroupSubject = new Subject<any>();
        this.ShiftVisibility = new Subject<any>();
        this.RouteResetGroupSubject = new Subject<any>();
        this.RouteDeleteDeliveryGroupSubject = new Subject<any>();
        this.DsbQueueChangesForNotification = new Subject<boolean>();
        this.OptionalPickupSubject = new Subject<any>();
        this.IsScheduleBuilderSubject = new BehaviorSubject<any>(false);
        this.CancelEntireRowSubject = new Subject<any>();
        this.CancelDSDeliveryGroupSubject = new Subject<any>();
        this.CancelDSDeliveryGroupNormalGroupDSSubject = new Subject<any>();
        this.CancelDSLocationSubject = new Subject<any>();
        this.DeliveryScheduleRemoveSubject = new Subject<any>();
        this.DispatcherLoadDragDropSubject = new Subject<any>();
        this.DispatcherLoadDragDropMapSubject = new Subject<any>();
        this.RefreshDsbOttoBuilderSubject = new Subject<any>();
    }

    public setShowDeliveryGroupSubject(data: boolean): void {
        this.ShowDeliveryGroupSubject.next(data);
    }

    public setShowOpenedDeliveryGroupSubject(data: boolean) {
        this.ShowOpenedDeliveryGroupSubject.next(data);
    }

    public setEditDeliveryGroupSubject(data: any): void {
        this.EditDeliveryGroupSubject.next(data);
    }

    public setDraftDeliveryGroupSubject(data: any): void {
        this.DraftDeliveryGroupSubject.next(data);
    }

    public setSaveModifiedLoadsSubject(data: any): void {
        this.SaveModifiedLoadsSubject.next(data);
    }

    public setPublishDeliveryGroupSubject(data: any): void {
        this.PublishDeliveryGroupSubject.next(data);
    }

    public setCancelDeliveryGroupSubject(data: any): void {
        this.CancelDeliveryGroupSubject.next(data);
    }

    public setDeleteDeliveryGroupSubject(data: any): void {
        this.DeleteDeliveryGroupSubject.next(data);
    }

    public setAcceptRejectDRSubject(data: any): void {
        this.AcceptRejectDRSubject.next(data);
    }
     
    public setRestoreDeletedRequestSubject(data: any): void {
        this.RestoreDeletedRequestSubject.next(data);
    }

    public setDrUpdatedSubject(data: any): void {
        this.DrUpdatedSubject.next(data);
    }

    public setRemoveDroppedRequestSubject(data: any): void {
        this.RemoveDroppedRequestSubject.next(data);
    }

    public setUnsavedChangesSubject(data: any): void {
        this.UnsavedChangesSubject.next(data);
    }

    public setSavedChangesSubject(data: any): void {
        this.SavedChangesSubject.next(data);
    }

    public setUnsavedChangesAsEmptySubject(): void {
        this.EmptyUnsavedChangesSubject.next();
    }

    public setTrailerShiftsSubject(data: any): void {
        this.TrailerShiftsSubject.next(data);
    }

    public setDeleteDRRequestSubject(data: any): void {
        this.DeleteDRRequestSubject.next(data);
    }
    public setDisableDSBControls(data: any): void {
        this.DisableDSBControlsSubject.next(data);
    }

    public setEditDriverTrailerSubject(data: any): void {
        this.EditDriverTrailerSubject.next(data);
    }

    public setUpdateDriverTrailerSubject(data: any): void {
        this.UpdateDriverTrailerSubject.next(data);
    }

    public setAllShiftsSubject(data: any): void {
        this.AllShiftsSubject.next(data);
    }

    public setAllTrailersSubject(data: any): void {
        this.AllTrailersSubject.next(data);
    }

    public setAllDeliveryRequestsSubject(data: any): void {
        data = this.removeDuplicates(data);
        this.AllDeliveryRequestsSubject.next(data);
    }

    private removeDuplicates(data: any): any {
        data = data.filter((item, index, array) =>
            index === array.findIndex((find) =>
                find.Id === item.Id
            )
        );
        return data;
    }

    public setSaveDriverAssignmentSubject(data: any): void {
        this.SaveDriverAssignmentSubject.next(data);
    }

    public setPublishEntireRowSubject(data: any): void {
        this.PublishEntireRowSubject.next(data);
    }

    public setGroupChangeDetectSubject(data: any): void {
        this.GroupChangeDetectSubject.next(data);
    }

    public setScheduleChangeDetectSubject(data: any): void {
        this.ScheduleChangeDetectSubject.next(data);
    }

    public setDragDropItemSubject(data: any): void {
        this.DragDropItemSubject.next(data);
    }

    public setCreateLoadChangeSubject(data: any): void {
        this.CreateLoadChangeSubject.next(data);
    }

    public setCreateLoadCancelSubject(data: any): void {
        this.CreateLoadCancelSubject.next(data);
    }

    public setCreateLoadSuccessSubject(data: any): void {
        this.CreateLoadSuccessSubject.next(data);
    }

    public setCreatePreloadSubject(data: any): void {
        this.CreatePreloadSubject.next(data);
    }

    public setUpdatePostloadSubject(data: any): void {
        this.UpdatePostloadSubject.next(data);
    }

    public setDeletePostloadSubject(data: any): void {
        this.DeletePostloadSubject.next(data);
    }

    public setDeliveryPreloadOption(data: any): void {
        this.DeliveryPreloadOption.next(data);
    }

    public setResetDrByRoutesSubject(data: any): void {
        this.ResetDrByRoutesSubject.next(data);
    }

    public setEditCompartmentAssigmentSubject(data: any): void {
        this.EditCompartmentAssigmentSubject.next(data);
    }

    public setSaveEditCompartmentAssigmentSubject(data: any): void {
        this.SaveEditCompartmentAssigmentSubject.next(data);
    }

    public setOpenDsbOttoBuilderSubject(isOpen: boolean): void {
        this.OpenDsbOttoBuilderSubject.next(isOpen);
    }
    public setOpenDsbOttoNotificationSubject(isOpen: boolean): void {
        this.OpenDsbOttoNotificationSubject.next(isOpen);
    }
    public setDsbOttoNotificationCountSubject(isOpen: number): void {
        this.DsbOttoNotificationCountSubject.next(isOpen);
    }
    public setSplitDRsInfoSubject(inputData: any): void {
        this.SplitDrsInfoSubject.next(inputData);
    }
    public setTransferDSSubject(data: any): void {
        this.TransferDSInfoSubject.next(data);
    }
    public setTrailerInformationSubject(data: any): void {
        this.TrailerDSInfoSubject.next(data);
    }
    public unassignTrailerInformationSubject(data: any): void {
        this.UnAssignTrailerFromShift.next(data);
    }
    public setRouteDetailsSubject(data: any): void {
        this.RouteDetailsSubject.next(data);
    }
    public setDeleteRouteDetailsSubject(data: any): void {
        this.DeleteRouteDetailsSubject.next(data);
    }
    public setHideDeliveryGroupSubject(data: any): void {
        this.HideDeliveryGroupSubject.next(data);
    }
    public setGridViewSearch(data: any): void {
        this.GridViewSearchGroupSubject.next(data);
    }
    public setRemoveTrailer(data: any): void {
        this.RemoveTrailerGroupSubject.next(data);
    }
    public setShiftVisibility(data: any): void {
        this.ShiftVisibility.next(data);
    }
    public setRouteResetGroupSubject(data: any): void {
        this.RouteResetGroupSubject.next(data);
    }
    public setRouteDeleteDeliveryGroupSubject(data: any): void {
        this.RouteDeleteDeliveryGroupSubject.next(data);
    }
    public setDrQueueChangesForFilter(data: boolean): void {
        this.drQueueChangesForFilter.next(data);
    }
    public setDsbQueueChangesForNotification(data: boolean): void {
        this.DsbQueueChangesForNotification.next(data);
    }
    public setDriverColumnChangeDetect(data: boolean): void {
        this.DriverColumnChangeDetect.next(data);
    }
    public setOptionalPickupInfo(data: any): void {
        this.OptionalPickupSubject.next(data);
    }
    public setScheduleLoaderSubject(data: any): void {
        this.IsScheduleBuilderSubject.next(data);
    }
    public setOpenOnTheFlyLocationFormSubject(data: any): void {
        this.OpenOnTheFlyLocationFormSubject.next(data);
    }
    public setGetOnTheFlyLocationDetailsSubject(data: any): void {
        this.getOnTheFlyLocationDetailsSubject.next(data);
    }
    public setOnTheFlyDetectSubject(data: boolean): void {
        this.OnTheFlyDetectSubject.next(data);
    }
    public setCancelEntireRowSubject(data: any): void {
        this.CancelEntireRowSubject.next(data);
    }
    public setCancelDSDeliveryGroupSubject(data: any): void {
        this.CancelDSDeliveryGroupSubject.next(data);
    }
    public setCancelDSGroupNormalSubDSSubject(data: any): void {
        this.CancelDSDeliveryGroupNormalGroupDSSubject.next(data);
    }
    public setCancelDSLocationDSSubject(data: any): void {
        this.CancelDSLocationSubject.next(data);
    }
    public setDeliveryScheduleRemoveSubject(data: any): void {
        this.DeliveryScheduleRemoveSubject.next(data);
    }
    public setDispatcherLoadDragDropSubject(data: any): void {
        this.DispatcherLoadDragDropSubject.next(data);
    }
    public setDispatcherLoadDragDropMapSubject(data: any): void {
        this.DispatcherLoadDragDropMapSubject.next(data);
    }
    public setLoadLocationSequenceSubject(data: FormGroup): void {
        this.LoadLocationSequenceSubject.next(data);
    }
    public refreshDsbOttoBuilderSubject(isRefresh: boolean): void {
        this.RefreshDsbOttoBuilderSubject.next(isRefresh);
    }
    public setDropTerminalSelectedSubject(data: number): void {
        this.DropTerminalSelectedSubject.next(data);
    }
    public removeFeesOnCreateNewSubject(): void {
        this.RemoveFeesSubject.next();
    }
}
