export class DriverScheduleMapping {
    Id?: string;
    DriverId?: number;    
    DriverName?: string;    
    ScheduleList?: DriverSchedule[]=[];
}

export class  DriverSchedule {

    Id?: string;
    ShiftId? : string;
    StartDate?: Date;
    EndDate?: Date;
    RepeatDayList?: any[] = [];
    RepeatDayStringList?: any[] = [];
    Type?: number;
    IsActive?: boolean;
    Repeat?: number;  //daily or custom
    selectedShifts?: any[] = [];
    selectedRepeatList?: any[] = [];
    TypeId?:number;
    RepeatEveryDay?: number; 
    }

    export class ConflictDates {      
        Date: string;        
      }