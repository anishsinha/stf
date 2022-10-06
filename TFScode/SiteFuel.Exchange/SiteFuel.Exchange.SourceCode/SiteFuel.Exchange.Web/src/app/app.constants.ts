import { DropdownItem } from "src/app/statelist.service";
import { DropDownItem } from "./buyer-wally-board/Models/BuyerWallyBoard";

//REGULAR EXPRESSION CONSTANTS
export const RegExConstants = {
    UsaZip: /^\d{5}(?:[-\s]\d{4})?$/,
    CanZip: /^[A-Za-z]\d[A-Za-z][ ]\d[A-Za-z]\d$/,
    Phone: /^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$/,
    Email: /^(([^<>()[\]\.,;:\s@\"]+(\.[^<>()[\]\.,;:\s@\"]+)*)|(\".+\"))@(([^<>()[\]\.,;:\s@\"]+\.)+[^<>()[\]\.,;:\s@\"]{2,})$/i,
    DateFormat: new RegExp("^((0?[1-9]|1[012])[-/.](0?[1-9]|[12][0-9]|3[01])[-/.](20)?[0-9]{4})*$"),
    Integer: /^\d+$/,
    Float: /^((\d+(\.\d *)?)|((\d*\.)?\d+))$/,
    Quantity: "^(\0*[1-9]*[1-9][0-9]*(\.[0-9]+)?|[0]*\.[0-9]*[1-9][0-9]*)$",// Use for Net, Gross, lift quantity
    DecimalNumber: /^((\d|[1-9]\d+)(\.\d{1,8})?|\.\d{1,8})$/,
    Fee : "/^[0-9]\d*(\.\d+)?$/",
    Date: new RegExp(/^(0?[1-9]|1[012])[\/\-](0?[1-9]|[12][0-9]|3[01])[\/\-]\d{4}$/),
}

//CONSTANTS FOR SCHEDULE BUILDER MODULE
export const SBConstants = {
    CompletedScheduleStatuses: [7, 8, 9, 10, 24],
    OnTheWayScheduleStatuses: [1, 3, 9, 11, 12, 13, 15, 16, 17, 18, 19, 20],
    DraggableScheduleStatuses: [0, 1, 2, 3, 4, 6, 14, 15, 16, 17, 18, 19, 23],
    CancelledScheduleStatuses: [5],
    SubDrOtherThanCancellStatus: [3, 23, 17, 11, 20, 15, 14, 1, 16, 6, 12],
    CancelCompletedDraftStatus: [7, 8, 5, 0]
}

//RACK AVG TYPE
export const RackAvgTypes: DropdownItem[] = [
    { Id: 1, Name: '+$', Code: '' },
    { Id: 2, Name: '-$', Code: '' },
    { Id: 3, Name: '+%', Code: '' },
    { Id: 4, Name: '-%', Code: '' }
]

export interface DropDown {
    id: string;
    text: string;
    disabled?: boolean;
    children?: Array<DropDown>;
    additional?: any;
}

export const NumberConstants = {
    MinQuantity: 0.00001,
    MaxQuantity: 9999999999,

    MinPercent: 0.00001,
    MaxPercent: 100,
}

export const allowedFileSizeInMB: number = 5; //MB

export const additiveProductTypeId: number = 26;

export const loginURL: string = "/Account/Login";

export function IsValidFileSize(sizeInBytes: number) {
    if (sizeInBytes) {
        return (Math.round(sizeInBytes / 1024) / 1024) <= allowedFileSizeInMB;
    }
    return false;
}

export function convertTo24Hour(time) {
    if (/(1[2]):[0][0] ?[Aa][mM]$/.test(time)) {
        return '00:00';
    }
    if (/(1[2]):[0][0]:[0][0] ?[Aa][mM]$/.test(time)) {
        return '00:00:00';
    }
    var timewithFormat = time.split(' ');
    var response = timewithFormat[0].split(':');
    var hours = response[0], minutes = response[1];
    if (time.toLowerCase().indexOf('am') !== -1 && hours == 12) {
        hours = 0;
    }
    if (time.toLowerCase().indexOf('pm') !== -1 && hours < 12) {
        hours = parseInt(hours) + 12;
    }
    response = hours + ':' + minutes + (response.length == 3 ? ':' + response[2] : '');
    return response;
}

export function getMinutes(time) {
    return (time.split(':')[0] * 60) + time.split(':')[1];
}

export function getSeconds(time) {
    return (getMinutes(time) * 60) + (time.split(':').length === 3 ? time.split(':')[2] : 0);
}

export const ColorConstants = ['HotPink', 'CornflowerBlue', 'Olive', 'SteelBlue', 'Gray', 'Tomato', 'Orange', 'Gold', 'DarkCyan', 'ForestGreen', 'FireBrick', 'YellowGreen', 'SkyBlue', 'Orchid', 'Wheat', 'DarkTurquoise', 'Crimson', 'Chocolate', 'LightBlue', 'Lime', 'Maroon', 'Teal',];

export const OptimizedCapacityConfirmation = 'You have a non-optimized load being published, Do you wish to continue publishing or correct the non optimized load?';

export const LoadPriorities: DropDownItem[] =
    [
        { Id: 1, Name: 'Must Go' },
        { Id: 2, Name: 'Should Go' },
        { Id: 3, Name: 'Could Go' }
    ];

export const InventoryDataCaptureList: DropDownItem[] =
    [
        { Id: 0, Name: 'Not specified' },
        { Id: 1, Name: 'Connected' },
        { Id: 2, Name: 'Manual' },
        { Id: 3, Name: 'Call-In' },
        { Id: 4, Name: 'Mixed' }
    ];

export const ScheduleTypes : DropDownItem[] = [
    { Id: 1, Name: "Weekly" },
    { Id: 2, Name: "Bi-Weekly" },
    { Id: 3, Name: "Monthly" },
    { Id: 4, Name: "Specific Date(s)" },
]

export const ScheduleDays = [
    { id: "1", text: "Mon" },
    { id: "2", text: "Tue" },
    { id: "3", text: "Wed" },
    { id: "4", text: "Thu" },
    { id: "5", text: "Fri" },
    { id: "6", text: "Sat" },
    { id: "7", text: "Sun" }
]

export const ScheduleQuantityType = [
    {
        Id: 1, Code: "1", Name: "Quantity", DisplayName: null, DRNote: null, DeliveryLevelPO: null
    },
    {
        Id: 2, Code: "2", Name: "Balance", DisplayName: null, DRNote: null, DeliveryLevelPO: null
    },
    {
        Id: 3, Code: "3", Name: "Full Load", DisplayName: null, DRNote: null, DeliveryLevelPO: null
    },
    {
        Id: 4, Code: "4", Name: "Small Compartment", DisplayName: null, DRNote: null, DeliveryLevelPO: null
    },
    {
        Id: 5, Code: "5", Name: "Not Specified", DisplayName: null, DRNote: null, DeliveryLevelPO: null
    }
]
