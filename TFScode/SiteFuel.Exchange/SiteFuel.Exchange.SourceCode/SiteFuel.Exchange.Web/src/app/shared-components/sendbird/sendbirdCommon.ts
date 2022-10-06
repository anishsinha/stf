import { CompanyUsers } from 'src/app/carrier/models/DispatchSchedulerModels';

export class BackgroupChatIntialize {
    public regionId: string;
    public driverInfo: Driver;
}
export class ChatMessage {
    public driverInfo: Driver;
    public currentLoginUserInfo: Carriar;
    public groupChannelInfo: GroupChannelInfo;
    public chatData: ChatData[];
    public companyUserInfo: CompanyUsers[];
    public showsmallLoder: boolean = false;
    public loadmore: boolean = false;
    public regionId: string;
    public fullregionId: string;
    public regionName: string;
    public regionDescription: string;
    public unreadCount: number = 0;
    public divId: string;
    public chatLoading: boolean = false;
}
export class Carriar {
    public userName: string;
    public userIntialName: string;
    public ProfileURL: string;
    public ConnectionStatus: boolean;
    public templastSeenAt: string;
    public lastSeenAt: string;
}
export class Driver {
    public Id: number;
    public Email: string;
    public FullName: string;
    public ConnectionStatus: boolean = false;
    public lastSeenAt: string = '';
    public templastSeenAt: string = '';
    public ProfileURL: string;
    public LastSeenDateTime: string;
    public TypingIndicator: boolean = false;
    public TypingMemberName: string = '';
    public State: string;
    public Role: string;
    public PhoneNumber: string;
    public IsPhoneNumberConfirmed: boolean;
    public firstMessage: boolean = true;
    public StatusCode: number = 0;       
}
export class GroupChannelInfo {
    public groupChannelObj: any;
    public groupChannelURL: string;
    public groupChannelName: string;
    public groupChannelCustomType: string;
    public memberInfo: MemberInfo[] = [];
}
export class MemberInfo {
    public memberId: string;
    public nickname: string;
    public userId: string;
    public connectionStatus: string;
    public profileUrl: string;
    public lastSeenAt: string;
}
export class ChatData {
    public messageId: string;
    public createdAt: string;
    public channelURL: string;
    public channelType: string;
    public messageType: number;//sendMessage=1 , receiveMessage =2
    public nickname: string;
    public userId: string;
    public message: string;
    public dateTime: string;
    public fulldateTime: string;
    public headerText: string;
}

export class chatResponse {
    public statusCode: number;
    public object: any;
    public object1: any;
    public message: any;
}
export class Roles {
    public Id: number;
    public Name: string;
}