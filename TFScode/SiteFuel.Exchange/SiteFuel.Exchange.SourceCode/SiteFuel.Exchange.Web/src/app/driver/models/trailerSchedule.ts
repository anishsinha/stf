
export class TrailerSchedule {

    Id: string;
    RegionId: string;
    TrailerId: string;
    TrailerShiftDetail: TrailerShiftDetail[] = [];
    StartDate: Date;
    EndDate: Date;
    RepeatDayList: any[] = [];
    Type: number;
    IsActive: boolean;
}

export class TrailerShiftDetail {
    ShiftId: string;
    ColumnId: number;
}


