export class RegionScheduleViewModel {
    Id?: string;
    RegionId?: string;
    RouteId?: string;
    StartDate?: Date;
    EndDate?: Date;
    IsActive: boolean;
    Repeat?: number;
    RepeatDayList?: any[] = [];
    Type?: number;
    RegionShiftDetail?: ShiftSchedule[] = [];
}

export class ShiftSchedule {
    ShiftId?: string;
    ColumnIndex?: number;
}

export class RegionScheduleMappingViewModel {
    Id?: String;
    RegionId?: String;
    RouteId?: String;
    RegionName?: String;
    RouteName?: String;
    IsUnplanedSchedule?: boolean;
    StartDate?: Date;
    EndDate?: Date;
    Description?: String;
    RepeatDayList?: any[] = [];
    ShiftDetail?: ShiftDetailViewModel[] = []
}

export class ShiftDetailViewModel {
    ShiftId?: string;
    ShiftName: string;
    ColumnIndex: string;
    ColumnName: string;
}