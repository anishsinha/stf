import { ColumnStatusEnum } from "src/app/app.enum";
import { DropDownItem } from "src/app/buyer-wally-board/Models/BuyerWallyBoard";
import { DropdownItem } from "src/app/statelist.service";
import { DeliveryRequestViewModel, TrailerModel } from "../../models/DispatchSchedulerModels";

import { DSBSaveModel} from '../../models/DispatchSchedulerModels';
export class DSBLoadQueueModel {
    Id: string;
    ScheduleBuilderId: string;
    RegionId: string;
    Date: string;
    ShiftId: string;
    ShiftIndex: number;
    DriverRowIndex: number;
}

export class DsbLoadQueueSuccess {
    Id: string;
    RegionId: string;
    ShiftId: string;
    ShiftIndex: number;
    DriverRowIndex: number;
}


export class DrError {
    DrIndex: number;
    Errors: string[];

    constructor() {
        this.DrIndex = 0;
        this.Errors = [];
    }
}


export class TripError {
    TripIndex: number;
    Errors: string[];
    //DrErrors: DrError[];

    constructor() {
        this.TripIndex = 0;
        this.Errors = [];
        //this.DrErrors = [];
    }
}
export class LoadQueueColumnValidations {
    DrCount: number;
    TrailerJobIncompatibleDrs: number;
    ShiftIndex: number;
    ShiftId: string;
    ScheduleIndex: number;
    Errors: string[];
    TripErrors: TripError[];
    ColumnStatus: ColumnStatusEnum;
    Customers: string[];
    Locations: string[];

    constructor() {
        this.DrCount = 0;
        this.ShiftIndex = 0;
        this.ScheduleIndex = 0;
        this.Errors = [];
        this.TripErrors = [];
        this.ColumnStatus = 0;
        this.Customers = [];
        this.Locations = [];
    }
}

export class TrailersDeliveryRequestViewModel {
    trailers: any[];//TrailerModel
    deliveryRequests: DeliveryRequestViewModel[];
    shiftIndex: number;
    scheduleIndex: number;

    constructor() {
        this.trailers = [];
        this.deliveryRequests = [];
    }
}

export class TrailerJobNonCompatibleDrs {
    drCount: number;
    shiftIndex: number;
    scheduleIndex: number;
}

export class LoadQueueStatusViewModel {
    StatusCode: number;
    LoadQueueSuccessInfo: DsbLoadQueueStatusViewModel[];
    LoadQueueErrorInfo: DsbLoadQueueStatusViewModel[];
}

export class DsbLoadQueueStatusViewModel {
    ShiftIndex: number;
    DriverColIndex: number;
    TfxDriverName: string;
    TfxUserId: number;
    TfxCompanyId: number;
    Messages: LoadQueueStatus[];
}

export class LoadQueueStatus {
    StatusMessage: string;
}


export class DSBLoadQueueNotificationResponse {
    public ScheduleBuilderId: string;
    public RegionId: string;
    public Date: string;
    public ShiftId: string;
    public ShiftIndex: number;
    public DriverColIndex: number;
    public Status: number;
    public DSBSaveModel: DSBSaveModel = new DSBSaveModel();

}
export class DSBLoadQueueNotificationModel {
    public ScheduleBuilderId: string;
    public RegionId: string;
    public Date: string;
    public ShiftId: string;
    public ShiftIndex: number;
    public DriverColIndex: number;
}
