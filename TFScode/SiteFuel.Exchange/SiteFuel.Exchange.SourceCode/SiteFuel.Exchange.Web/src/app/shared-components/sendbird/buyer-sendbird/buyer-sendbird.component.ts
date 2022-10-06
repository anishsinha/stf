import { Component, OnInit, ChangeDetectionStrategy, ChangeDetectorRef } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { __await } from 'tslib';
import { Declarations } from 'src/app/declarations.module';
import { ScheduleBuilderService } from 'src/app/carrier/service/schedule-builder.service';
import { CompanyUsers } from 'src/app/carrier/models/DispatchSchedulerModels';
import { SendBirdCommonComponent } from '../common/sendbird.common.component';
import { chatService } from 'src/app/shared-components/sendbird/services/sendbird.service';
import { BackgroupChatIntialize, ChatMessage, Carriar, Driver, ChatData, GroupChannelInfo, chatResponse, MemberInfo } from '../sendbirdCommon';
declare var userName: string;
declare var userIntialName: string;
declare var currentUserCompanyId: number;
declare var userEmail: string;
declare const SendBird: any;
declare function IsUserActive(): boolean;

@Component({
    selector: 'app-buyer-sendbird',
    templateUrl: './buyer-sendbird.component.html',
    styleUrls: ['./buyer-sendbird.component.css'],
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class BuyerSendbirdComponent implements OnInit {
    public sb: any = Object();
    public ChannelHandler: any;
    public appKey: string = '';
    public sbSendbird: FormGroup;
    public backgrounddriverDetails: BackgroupChatIntialize[];
    public chatCollection: ChatMessage[] = [];
    public intervalTime: any;
    public static messageLoadLimit: number = 30;
    constructor(private chatService: chatService, private sbService: ScheduleBuilderService,
        private fb: FormBuilder, private cdr: ChangeDetectorRef) {
    }
    // #region ngEvent  
    ngOnInit() {
        this.sbSendbird = this.initSendbirdForm();
        this.getAPPKey();
    }
    ngOnDestroy() {
        if (this.intervalTime) {
            clearInterval(this.intervalTime);
        }
        this.sb.removeChannelHandler("onTypingStatusUpdated");
        this.sb.removeChannelHandler("onMessageReceived");
        this.sb.removeChannelHandler("onReadReceiptUpdated");
    }
    // #endregion 

    //intialize sendbird form.
    private initSendbirdForm() {
        let _dtForm = this.fb.group({
            textMessage: this.fb.control(''),
        });
        return _dtForm;
    }
    // #region privateMethods
    //intialize sendbird account
    private IntializeSendbird(driverId, regionID: string) {
        let currentChatWidgetIndex = this.chatCollection.findIndex(top => top.driverInfo.Id === driverId);
        if (currentChatWidgetIndex === -1) {
            let chatUserDetails = [];
            this.chatService.getDriverDetails(driverId).subscribe(t => {
                if (t.StatusCode === 0) {
                    //create the object of the class
                    let chatMessage: ChatMessage = new ChatMessage();
                    //chatMessage.regionId = regionID;
                    chatMessage.showsmallLoder = false;
                    chatMessage.loadmore = false;
                    let carrierObj: Carriar = new Carriar();
                    let driverObj: Driver = new Driver();
                    let chatData: ChatData[] = [];
                    let groupChannelInfo: GroupChannelInfo = new GroupChannelInfo();

                    //assign value of driver object
                    driverObj = t as Driver;
                    driverObj.firstMessage = true;

                    //define the loading..
                    chatMessage.chatLoading = true;
                    chatMessage.divId = "divusers_" + this.chatCollection.length;
                    //get companyUserDetails
                     {
                        let companyUserDetails: CompanyUsers[] = [];
                        let companyUser: CompanyUsers = new CompanyUsers();
                        companyUser.RegionDescription = userName;
                        companyUser.RegionID = userName;
                        companyUser.SendBirdRegionID = userName;
                        companyUser.EmailAddress = userEmail;
                        companyUser.FullName = userName;
                        companyUserDetails.push(companyUser);
                            if (companyUserDetails.length > 0) {

                                //set regionName and regionDescription and regionID.
                                chatMessage.regionName = companyUserDetails[0].RegionName;
                                chatMessage.regionDescription = companyUserDetails[0].RegionDescription;
                                chatMessage.regionId = companyUserDetails[0].SendBirdRegionID;
                                chatMessage.fullregionId = regionID;
                                //set groupName // OR Retrive groupName
                                groupChannelInfo.groupChannelName = "DR_" + driverId + "_COM_" + currentUserCompanyId + "_RE_" + companyUser;

                                //set companyCurrent User Information
                                chatMessage.companyUserInfo = companyUserDetails;
                                if (companyUserDetails.length > 0) {
                                    companyUserDetails.forEach(xitem => {
                                        chatUserDetails.push(xitem.EmailAddress);
                                    });
                                    chatUserDetails.push(driverObj.Email);
                                    chatMessage.driverInfo = driverObj;

                                    carrierObj.userName = userName;
                                    carrierObj.userIntialName = userIntialName;
                                    chatMessage.currentLoginUserInfo = carrierObj;
                                    chatMessage.chatData = chatData;
                                    chatMessage.groupChannelInfo = groupChannelInfo;
                                }
                            }
                            else {
                                chatMessage.companyUserInfo = [];
                            }
                            if (chatMessage.companyUserInfo.length > 0) {
                                this.chatCollection.push(chatMessage);
                                this.cdr.detectChanges();
                                let chatObj = this.chatCollection.find(top => top.driverInfo.Email === driverObj.Email);
                                if (chatObj != null) {
                                    this.createAccount(chatMessage.companyUserInfo, chatMessage.currentLoginUserInfo, chatMessage.driverInfo, chatObj);
                                }
                            } else {
                                Declarations.msgwarning("There is no dispatcher found in the region", undefined, undefined);
                            }
                        }

                }
                else {
                    Declarations.msgerror("Issue in getDriverDetails api..", undefined, undefined);
                }
            });
        }
    }
    //create account in sendbird and connect to groupchat
    private async createAccount(companyUserInfo: CompanyUsers[], currentUserLoginInfo: Carriar, driverObj: Driver, chatMessage: ChatMessage) {
        try {
            if (companyUserInfo.length > 0 && driverObj != null && currentUserLoginInfo != null) {
                //create driver account
                let carrierObjResult = await this.connectDispatcher(currentUserLoginInfo) as chatResponse;
                let driverObjResult = await this.connectDriver(driverObj) as chatResponse;
                if (driverObjResult.statusCode === 100 && carrierObjResult.statusCode === 100) {
                    //check channel exists or not..
                    let groupChannel = await this.sbIntializeGroupChannel(chatMessage.driverInfo.Email, chatMessage.companyUserInfo, chatMessage.groupChannelInfo) as chatResponse;
                    if (groupChannel.statusCode === 100) { // return group channel that already created.
                        //find the diffrence member between group member and company users
                        //so that we can add additional member to current channel
                        let sbjoinResponse = await this.sbJoinGroupChannel(chatMessage, groupChannel.object1, companyUserInfo, driverObj) as chatResponse;
                        if (sbjoinResponse.statusCode === 200) {
                        }
                        else {
                        }
                        let carrierObjResult = await this.connectDispatcher(currentUserLoginInfo) as chatResponse;
                        if (carrierObjResult.statusCode === 100) {
                            this.updateGroupChannelInfo(chatMessage, groupChannel);
                            let messageHistory = await this.sbLoadChatHistroy(chatMessage.groupChannelInfo.groupChannelObj, currentUserLoginInfo, chatMessage) as chatResponse;
                            if (messageHistory.statusCode === 100) {
                                this.updateChatData(chatMessage, messageHistory);
                                let memberInfos = await this.sbChannelMemberDetails(chatMessage.groupChannelInfo.groupChannelObj, driverObj, companyUserInfo) as MemberInfo[];
                                if (memberInfos.length > 0)
                                    chatMessage.groupChannelInfo.memberInfo = memberInfos;
                                if (groupChannel.statusCode === 100) {
                                    chatMessage.chatLoading = false;
                                    let dvObj = $("#" + chatMessage.divId);
                                    if (dvObj != undefined) {
                                        if (dvObj.hasClass('chat-minimized')) {
                                            dvObj.removeClass('chat-minimized');
                                            if (dvObj.find('div.chat-user').length !== 0) {
                                                dvObj.find('div.chat-user').addClass("bg-white");
                                            }
                                        }
                                    }
                                    this.cdr.detectChanges();
                                    setTimeout(function () { BuyerSendbirdComponent.scrollBottomDiv(driverObj); }, 1500);
                                }

                            }
                        }
                    }
                    else {
                        //create company users account - dispatcher,carrier => Role
                        let companyUserCount = companyUserInfo.length;
                        if (companyUserInfo.length > 0) {
                            for (const companyUser of companyUserInfo) {
                                let companyUserResponse = await this.connectCompanyUser(companyUser) as chatResponse;
                                if (companyUserResponse.statusCode === 100) {
                                    companyUser.ProfileURL = companyUserResponse.object.profileUrl;
                                    companyUserCount = companyUserCount - 1;
                                    if (companyUserCount == 0) {
                                        //connect current user account - sendbird
                                        let carrierObjResult = await this.connectDispatcher(currentUserLoginInfo) as chatResponse;
                                        if (carrierObjResult.statusCode === 100) {
                                            //create the groupChannel of All Company Users(Carrier,Dispatcher) 
                                            //1 to M => 1 Drivers to Many Dispacther.Carrier User
                                            //create group channel or retrive channel if created.
                                            let groupChannel = await this.sbIntializeGroupChannel(chatMessage.driverInfo.Email, chatMessage.companyUserInfo, chatMessage.groupChannelInfo) as chatResponse;
                                            if (groupChannel.statusCode === 100) { // return group channel that already created.
                                                this.updateGroupChannelInfo(chatMessage, groupChannel);
                                                let messageHistory = await this.sbLoadChatHistroy(chatMessage.groupChannelInfo.groupChannelObj, currentUserLoginInfo, chatMessage) as chatResponse;
                                                if (messageHistory.statusCode === 100) {
                                                    this.updateChatData(chatMessage, messageHistory);
                                                    let memberInfos = await this.sbChannelMemberDetails(chatMessage.groupChannelInfo.groupChannelObj, driverObj, companyUserInfo) as MemberInfo[];
                                                    if (memberInfos.length > 0)
                                                        chatMessage.groupChannelInfo.memberInfo = memberInfos;
                                                }
                                                else {
                                                }
                                            }
                                            else if (groupChannel.statusCode === 200) { //create the group channel
                                                let createdChannel = await this.sbCreateGroupChannel(groupChannel.object, groupChannel.object1, chatMessage.regionName, chatMessage.regionDescription) as chatResponse;
                                                if (createdChannel.statusCode === 100) {
                                                    this.updateGroupChannelInfo(chatMessage, createdChannel);
                                                    let messageHistory = await this.sbLoadChatHistroy(chatMessage.groupChannelInfo.groupChannelObj, currentUserLoginInfo, chatMessage) as chatResponse;
                                                    if (messageHistory.statusCode === 100) {
                                                        this.updateChatData(chatMessage, messageHistory);
                                                        let memberInfos = await this.sbChannelMemberDetails(chatMessage.groupChannelInfo.groupChannelObj, driverObj, companyUserInfo) as MemberInfo[];
                                                        if (memberInfos.length > 0)
                                                            chatMessage.groupChannelInfo.memberInfo = memberInfos;
                                                    }
                                                }
                                            }
                                            if (groupChannel.statusCode === 100 || groupChannel.statusCode === 200) {
                                                chatMessage.chatLoading = false;
                                                let dvObj = $("#" + chatMessage.divId);
                                                if (dvObj != undefined) {
                                                    if (dvObj.hasClass('chat-minimized')) {
                                                        dvObj.removeClass('chat-minimized');
                                                        if (dvObj.find('div.chat-user').length !== 0) {
                                                            dvObj.find('div.chat-user').addClass("bg-white");
                                                        }
                                                    }
                                                }
                                                this.cdr.detectChanges();
                                                setTimeout(function () { BuyerSendbirdComponent.scrollBottomDiv(driverObj); }, 1500);
                                            }

                                        }
                                    }
                                }
                            }

                        }

                    }
                }

            }
            else {
                if (chatMessage.companyUserInfo.length == 0) {
                    chatMessage.chatLoading = false;
                    var chatObjIndex = this.chatCollection.findIndex(top => top.driverInfo.Email === driverObj.Email);
                    if (chatObjIndex != -1) {
                        this.chatCollection.splice(chatObjIndex, 1);
                        this.cdr.detectChanges();
                    }
                    Declarations.msgwarning("This driver is not assigned to any region", undefined, undefined);
                }
            }
        }
        catch (error) {
        }
    }
    public async connectCompanyUser(companyUserObj: CompanyUsers) {
        //create company user if not created..
        let companyObjResult = await this.sbConnect(companyUserObj.EmailAddress) as chatResponse;
        if (companyObjResult.statusCode === 100) {
            let companyObjData = companyObjResult.object;
            companyUserObj.ProfileURL = companyObjData.profileUrl;
            if (companyUserObj.FullName != companyObjData.nickname) {
                let updateCarrierInfo = await this.sbUpdateCurrentUserInfo(companyUserObj.FullName, companyObjData.profileUrl) as any;
            }
        }
        return companyObjResult;
    }

    private async connectDispatcher(carrierObj: Carriar) {
        //create carrier user if not created..
        let carrierObjResult = await this.sbConnect(carrierObj.userName) as chatResponse;
        if (carrierObjResult.statusCode === 100) {
            let carrierObjData = carrierObjResult.object;
            carrierObj.ProfileURL = carrierObjData.profileUrl;
            if (carrierObj.userIntialName != carrierObjData.nickname) {
                let updateCarrierInfo = await this.sbUpdateCurrentUserInfo(carrierObj.userIntialName, carrierObjData.profileUrl) as any;
            }
        }
        return carrierObjResult;
    }
    private async connectDriver(driverObj: Driver) {
        //create driver user if not created..
        let driverdetails = [];
        driverdetails.push(driverObj.Email);
        let userAvailable = await this.sbGetUserDetails(driverdetails) as chatResponse;
        let driverObjResult = new chatResponse();
        if (userAvailable.statusCode == 100) {
            if (userAvailable.object.length === 0) {
                driverObjResult = await this.sbConnect(driverObj.Email) as chatResponse;
                if (driverObjResult.statusCode === 100) {
                    let driverObjData = driverObjResult.object;
                    driverObj.ProfileURL = driverObjData.profileUrl;
                    let connectionStatusResponse = await this.checkUserOnline(driverObj.Email) as chatResponse;
                    if (connectionStatusResponse.statusCode === 100) {
                        driverObj.ConnectionStatus = connectionStatusResponse.object.toUpperCase() === 'ONLINE' ? true : false;
                    }
                    if (driverObj.ConnectionStatus === false) {
                        if (driverObjData.lastSeenAt > 0) {
                            let lastSeen = new Date(driverObjData.lastSeenAt);
                            let hourDate = BuyerSendbirdComponent.formatAMPM(lastSeen);
                            let monthName = BuyerSendbirdComponent.getMonthName(lastSeen);
                            let day = lastSeen.getDate();
                            driverObj.templastSeenAt = monthName + "," + day + "," + hourDate;
                            driverObj.lastSeenAt = SendBirdCommonComponent.getLastSeen(lastSeen);
                        }
                        else {
                            driverObj.lastSeenAt = 'NO';
                        }
                    }
                    if (driverObj.FullName != driverObjData.nickname) {
                        let updateDriverInfo = await this.sbUpdateCurrentUserInfo(driverObj.FullName, driverObjData.profileUrl) as any;
                    }
                }
            }
            else {
                driverObjResult.statusCode = 100;
                driverObjResult.object = userAvailable.object[0];
                driverObj.ConnectionStatus = driverObjResult.object.connectionStatus.toUpperCase() === 'ONLINE' ? true : false;
                driverObj.ProfileURL = driverObjResult.object.profileUrl;
                if (driverObj.ConnectionStatus === false) {
                    if (driverObjResult.object.lastSeenAt > 0) {
                        let lastSeen = new Date(driverObjResult.object.lastSeenAt);
                        let hourDate = BuyerSendbirdComponent.formatAMPM(lastSeen);
                        let monthName = BuyerSendbirdComponent.getMonthName(lastSeen);
                        let day = lastSeen.getDate();
                        driverObj.templastSeenAt = monthName + "," + day + "," + hourDate;
                        driverObj.lastSeenAt = SendBirdCommonComponent.getLastSeen(lastSeen);
                    }
                    else {
                        driverObj.lastSeenAt = 'NO';
                    }
                }
            }
        }
        return driverObjResult;
    }
    private static scrollBottomDiv(driverObj: Driver) {
        if ($(".driver_" + driverObj.Id)[0]) {
            $(".driver_" + driverObj.Id).scrollTop($(".driver_" + driverObj.Id)[0].scrollHeight);
        }
    }
    private updateGroupChannelInfo(chatMessage: ChatMessage, groupChannel: chatResponse) {
        chatMessage.groupChannelInfo.groupChannelURL = groupChannel.object;
        if (chatMessage.groupChannelInfo.groupChannelObj == null) {
            chatMessage.groupChannelInfo.groupChannelObj = groupChannel.object1;
        }
    }
    private updateChatData(chatMessage: ChatMessage, chatReponse: chatResponse) {
        chatMessage.chatData = chatReponse.object as ChatData[];
        if (chatMessage.chatData.length >= BuyerSendbirdComponent.messageLoadLimit) {
            chatMessage.loadmore = true;
        }
    }
    private sbGetUserDetails = userIds => {
        let response = new chatResponse();
        return new Promise((resolve) => {
            // Retrieving certain users using the UserID filter
            let applicationUserListQueryByIds = this.sb.createApplicationUserListQuery();
            applicationUserListQueryByIds.userIdsFilter = userIds;
            applicationUserListQueryByIds.next((user, error) => {
                if (error) {
                    response.statusCode = 404;
                    response.object = error;
                }
                else {
                    response.statusCode = 100;
                    response.object = user;
                }
                resolve(response);
            });
        });
    };
    private sbConnect = userId => {
        return new Promise((resolve, reject) => {
            let response: chatResponse = new chatResponse();
            this.sb.connect(userId, (user, error) => {
                if (error) {
                    response.statusCode = 200;
                    response.message = 'SendBird Login Failed.';
                    reject(response);
                }
                else {
                    response.statusCode = 100;
                    response.object = user;
                    resolve(response);
                }
            });
        });
    };
    private sbUpdateCurrentUserInfo = (nickname, profileUrl) => {
        return new Promise((resolve, reject) => {
            this.sb.updateCurrentUserInfo(nickname, profileUrl, (user, error) => {
                error ? reject('Unable to update user profile') : resolve(user);
            });
        });
    };
    private sbIntializeGroupChannel = (driverId, companyUserDetails: CompanyUsers[], groupChannelInfo: GroupChannelInfo) => {
        return new Promise((resolve) => {
            let userDetails = [];
            //push company user to array
            companyUserDetails.forEach(comItem => {
                userDetails.push(comItem.EmailAddress);
            });
            //push driver details to array
            userDetails.push(driverId);

            let response: chatResponse = new chatResponse();
            let filteredQuery = this.sb.GroupChannel.createMyGroupChannelListQuery();
            filteredQuery.channelNameContainsFilter = groupChannelInfo.groupChannelName;
            filteredQuery.includeEmpty = true;
            filteredQuery.limit = 15;
            filteredQuery.next(function (groupChannels, error) {
                if (error) {
                    response.statusCode = 404;
                    response.object = error;
                    resolve(chatResponse);
                }
                else {
                    if (groupChannels.length > 0) {
                        //return the group channel
                        response.object = groupChannels[0].url;
                        response.object1 = groupChannels[0];
                        response.statusCode = 100;
                        resolve(response);
                    }
                    else {
                        //create the group channel
                        response.statusCode = 200;
                        response.object = userDetails;
                        response.object1 = groupChannelInfo.groupChannelName;
                        resolve(response);
                    }
                }
            });
        });
    };
    private sbCreateGroupChannel = (userIds, groupChannelName, regionName, regionDecription) => {
        return new Promise((resolve) => {
            let response = new chatResponse();
            let groupChannelparams = new this.sb.GroupChannelParams();
            groupChannelparams.isPublic = true;
            groupChannelparams.isEphemeral = false;
            groupChannelparams.isDistinct = false;
            groupChannelparams.addUserIds(userIds);
            groupChannelparams.name = groupChannelName;
            groupChannelparams.data = regionDecription;
            groupChannelparams.customType = regionName;
            this.sb.GroupChannel.createChannel(groupChannelparams, (groupChannel, error) => {
                if (error) {
                    response.statusCode = 404;
                    response.object = error;
                }
                else {
                    response.statusCode = 100;
                    response.object = groupChannel.url;
                    response.object1 = groupChannel;
                }
                resolve(response);
            });
        });
    };
    private sbLoadChatHistroy = (chatting_groupChannel, carrierObj: Carriar, chatObj: ChatMessage) => {
        return new Promise((resolve) => {
            let response = new chatResponse();
            let messageListQuery = chatting_groupChannel.createPreviousMessageListQuery();
            messageListQuery.limit = BuyerSendbirdComponent.messageLoadLimit;
            messageListQuery.reverse = false;
            // Retrieving  messages.
            messageListQuery.load(function (messages, error) {
                if (error) {
                    response.statusCode = 404;
                    response.object = error;
                    resolve(response);
                }
                else {
                    let chatData: ChatData[] = [];
                    messages.forEach(function (value) {
                        let date = new Date(value.createdAt);
                        let hourDate = BuyerSendbirdComponent.formatAMPM(date);
                        let monthName = BuyerSendbirdComponent.getMonthName(date);
                        let monthDName = monthName.substring(0, 3).toUpperCase();
                        let day = date.getDate();
                        let year = date.getFullYear();
                        if (value._sender.nickname.trim().length === 0) {
                            if (chatObj.driverInfo.Email === value._sender.userId) {
                                value._sender.nickname = chatObj.driverInfo.FullName;
                            }
                            else {
                                var dispatcherDetails = chatObj.companyUserInfo.find(top => top.EmailAddress === value._sender.userId);
                                if (dispatcherDetails != null) {
                                    value._sender.nickname = dispatcherDetails.FullName;
                                }
                            }
                        }
                        if (value._sender.userId === carrierObj.userName) {
                            let chatItem = new ChatData();
                            chatItem.messageId = value.messageId;
                            chatItem.createdAt = value.createdAt;
                            chatItem.channelURL = value.channelUrl;
                            chatItem.channelType = value.channelType;
                            chatItem.messageType = 1;
                            var dispatcherDetails = chatObj.companyUserInfo.find(top => top.EmailAddress === value._sender.userId);
                            if (dispatcherDetails != null) {
                                chatItem.nickname = dispatcherDetails.FullName;
                            }
                            else {
                                chatItem.nickname = value._sender.nickname;
                            }

                            chatItem.message = value.message;
                            chatItem.dateTime = hourDate;
                            chatItem.fulldateTime = hourDate + "," + monthName + "," + day;
                            if (chatData.findIndex(top => top.headerText === day + " " + monthDName + " " + year) === -1) {
                                chatItem.headerText = day + " " + monthDName + " " + year;
                            }
                            else {
                                chatItem.headerText = '';
                            }
                            chatItem.userId = value._sender.userId;
                            chatData.push(chatItem);
                        }
                        else {
                            let chatItem = new ChatData();
                            chatItem.messageId = value.messageId;
                            chatItem.createdAt = value.createdAt;
                            chatItem.channelURL = value.channelUrl;
                            chatItem.channelType = value.channelType;
                            chatItem.messageType = 2;
                            var dispatcherDetails = chatObj.companyUserInfo.find(top => top.EmailAddress === value._sender.userId);
                            if (dispatcherDetails != null) {
                                chatItem.nickname = dispatcherDetails.FullName;
                            }
                            else {
                                if (chatObj.driverInfo.Email === value._sender.userId) {
                                    chatItem.nickname = chatObj.driverInfo.FullName;
                                }
                                else {
                                    chatItem.nickname = value._sender.nickname;
                                }
                            }
                            chatItem.message = value.message;
                            chatItem.dateTime = hourDate;
                            chatItem.fulldateTime = hourDate + "," + monthName + "," + day;
                            if (chatData.findIndex(top => top.headerText === day + " " + monthDName + " " + year) === -1) {
                                chatItem.headerText = day + " " + monthDName + " " + year;
                            }
                            else {
                                chatItem.headerText = '';
                            }
                            chatItem.userId = value._sender.userId;
                            chatData.push(chatItem);
                        }
                    });
                    response.statusCode = 100;
                    response.object = chatData;
                    resolve(response);
                }
            });

        });
    };
    private sbChannelMemberDetails = (chatting_groupChannel, driverObj: Driver, companyUserInfo: CompanyUsers[]) => {
        let memberName = "";
        let memberInfos: MemberInfo[] = [];
        return new Promise((resolve) => {
            chatting_groupChannel.refresh(function (response, error) {
                if (response) {
                    let memberdetails = response.members;
                    memberdetails.forEach(xItem => {

                        if (driverObj && companyUserInfo) {
                            if (driverObj.Email === xItem.userId) {
                                memberName = driverObj.FullName;
                            }
                            else {
                                if (companyUserInfo) {
                                    let compInfo = companyUserInfo.findIndex(top => top.EmailAddress === xItem.userId);
                                    if (compInfo != -1) {
                                        memberName = companyUserInfo[compInfo].FullName;
                                    }
                                }
                            }
                        }
                        let response = new MemberInfo();
                        response.memberId = xItem.memberId;
                        response.profileUrl = xItem.profileUrl;
                        response.nickname = xItem.nickname === "" ? memberName : xItem.nickname;
                        response.userId = xItem.userId;
                        response.connectionStatus = xItem.connectionStatus.toUpperCase();
                        if (xItem.lastSeenAt > 0) {
                            let lastSeen = new Date(xItem.lastSeenAt);
                            response.lastSeenAt = SendBirdCommonComponent.getLastSeen(lastSeen);
                        }
                        else {
                            response.lastSeenAt = '-';
                        }
                        memberInfos.push(response);
                    });
                    resolve(memberInfos);
                }
                if (error) {
                    resolve(memberInfos);
                }
            });

        });

    };
    private sbJoinGroupChannel = (chatMessage: ChatMessage, groupChannel: any, companyUserDetails: CompanyUsers[], driverinfo: Driver) => {
        return new Promise(async (resolve) => {
            let response = new chatResponse();
            try {


                let joinmemberdetails = '';
                let leavememberdetails = '';
                let currentGroupMemberDetails = [];
                let currentCompanyMemberDetails = [];
                currentCompanyMemberDetails.push(driverinfo.Email);
                groupChannel.members.forEach(xItem => {
                    currentGroupMemberDetails.push(xItem.userId);
                });
                companyUserDetails.forEach(xItem => {
                    currentCompanyMemberDetails.push(xItem.EmailAddress);
                });
                let diffMembers = this.arr_diff(currentCompanyMemberDetails, currentGroupMemberDetails);
                let totalMember = diffMembers.length;
                if (diffMembers.length > 0) {
                    for (const dmember of diffMembers) {
                        let companyUsers = companyUserDetails.find(top => top.EmailAddress === dmember);
                        let userExists = currentGroupMemberDetails.find(top => top === dmember);
                        if (companyUsers != null && userExists == null) {
                            let connectUsers = await this.connectCompanyUser(companyUsers) as chatResponse;
                            if (connectUsers.statusCode === 100) {
                                if (groupChannel.isPublic) {
                                    groupChannel.join(function (response, error) {
                                        if (error) {
                                            console.log("groupChannel-join-error", error);
                                        }
                                        joinmemberdetails = joinmemberdetails + "," + companyUsers.EmailAddress;
                                        totalMember = totalMember - 1;
                                        if (totalMember === 0) {
                                            response.object = joinmemberdetails;
                                            response.object1 = leavememberdetails;
                                            response.statusCode = 100;
                                            groupChannel.refresh(function (response, error) {
                                                if (response) {
                                                    chatMessage.groupChannelInfo.groupChannelObj = response;
                                                }
                                                if (error) {
                                                    this.co
                                                    console.log("groupChannel-refresh-error", error);
                                                }
                                            });
                                            resolve(response);

                                        }
                                    });
                                }
                            }

                        }
                        else if (companyUsers == null && userExists != null) {
                            let connectUsers = await this.sbConnect(dmember) as chatResponse;
                            if (connectUsers.statusCode === 100) {
                                groupChannel.leave(function (response, error) {
                                    if (error) {
                                        console.log("groupChannel-leave-error", error);
                                    }
                                    leavememberdetails = leavememberdetails + "," + dmember;
                                    totalMember = totalMember - 1;
                                    if (totalMember === 0) {
                                        if (response == null) {
                                            response = new chatResponse();
                                        }
                                        response.object1 = leavememberdetails;
                                        response.object = joinmemberdetails;
                                        response.statusCode = 100;
                                        groupChannel.refresh(function (response, error) {
                                            if (response) {
                                                chatMessage.groupChannelInfo.groupChannelObj = response;
                                            }
                                            if (error) {
                                                console.log("groupChannel-refresh-error", error);
                                            }
                                        });
                                        resolve(response);
                                    }
                                });
                            }
                        }
                        else {
                            totalMember = totalMember - 1;
                        }
                    }
                }
                else {
                    response.statusCode = 200;
                    resolve(response);
                }
            } catch (e) {
                response.statusCode = 404;
                response.object = e;
                resolve(response);
            }
        });
    };
    private static formatAMPM(date: any) {
        let hours = date.getHours();
        let minutes = date.getMinutes();
        let ampm = hours >= 12 ? 'PM' : 'AM';
        hours = hours % 12;
        hours = hours ? hours : 12; // the hour '0' should be '12'
        minutes = minutes < 10 ? '0' + minutes : minutes;
        let strTime = hours + ':' + minutes + ' ' + ampm;
        return strTime;
    }
    private static getMonthName(date: any) {
        let monthNames = ["January", "February", "March", "April", "May", "June",
            "July", "August", "September", "October", "November", "December"
        ];
        return monthNames[date.getMonth()];
    }
    private getAPPKey() {
        this.chatService.getSendBirdAPIKey().subscribe(t => {
            this.appKey = t.Value as string;
            this.sb = new SendBird({ appId: this.appKey });
            this.IntializeChannelHandler();
            this.intervalTime = setInterval(() => {
                if (IsUserActive()) {
                    this.sbCheckUserOnline();
                }
            }, 45000);
        });
    }
    private IntializeChannelHandler() {
        this.ChannelHandler = new this.sb.ChannelHandler();
        this.ChannelHandler.onTypingStatusUpdated = this.onTypingStatusUpdated;
        this.ChannelHandler.onMessageReceived = this.onMessageReceived;
        this.ChannelHandler.onReadReceiptUpdated = this.onReadReceiptUpdated;
        this.sb.addChannelHandler("onTypingStatusUpdated", this.ChannelHandler);
        this.sb.addChannelHandler("onMessageReceived", this.ChannelHandler);
        this.sb.addChannelHandler("onReadReceiptUpdated", this.ChannelHandler);
    }
    public callOffline(phoneNumber: any) {
        Declarations.msginfo("Driver contact details : " + phoneNumber, undefined, undefined);
    }
    public closeChat(email: string) {
        let index = this.chatCollection.findIndex(top => top.driverInfo.Email === email);
        if (index > -1) {
            let drItem = this.chatCollection.find(top => top.driverInfo.Email === email);
            if (drItem != null) {
                if (this.backgrounddriverDetails == undefined) {
                    this.backgrounddriverDetails = [];
                }
                let bkindex = this.backgrounddriverDetails.findIndex(x => x.driverInfo.Id === drItem.driverInfo.Id);
                if (bkindex === -1) {
                    let backobj = new BackgroupChatIntialize();
                    backobj.regionId = drItem.fullregionId;
                    backobj.driverInfo = drItem.driverInfo;
                    this.backgrounddriverDetails.push(backobj);

                }
            }
            this.chatCollection.splice(index, 1);
            this.cdr.detectChanges();
        }
    }
    public minimizeChat(divId: string, email: string, iconId: string) {
        let dvObj = $("#" + divId);
        let iconObj = $("#" + iconId);
        let chatObj = this.chatCollection.find(top => top.driverInfo.Email === email);
        if (chatObj != null) {
            chatObj.divId = divId;
        }
        if (dvObj != undefined && iconObj != undefined) {
            if (dvObj.hasClass('chat-minimized')) {
                dvObj.removeClass('chat-minimized');
                dvObj.removeClass('chat-active');
                if (chatObj != null) {
                    BuyerSendbirdComponent.scrollBottomDiv(chatObj.driverInfo);
                }
                if (iconObj.hasClass('fa-window-maximize')) {
                    iconObj.addClass('fa-window-minimize');
                    iconObj.removeClass('fa-window-maximize');
                }
            }
            else {
                dvObj.addClass('chat-minimized');
                if (iconObj.hasClass('fa-window-minimize')) {
                    iconObj.removeClass('fa-window-minimize');
                    iconObj.addClass('fa-window-maximize');
                }
            }
            this.cdr.detectChanges();
        }
    }
    async sendMessage(email: string) {
        let chatObj = this.chatCollection.find(top => top.driverInfo.Email === email);
        if (chatObj != null) {
            let chatChannel = chatObj.groupChannelInfo.groupChannelObj;
            let textMessage = this.sbSendbird.get('textMessage').value;
            if (chatChannel != null && textMessage != '') {
                const sendMessage = new this.sb.UserMessageParams();
                sendMessage.message = textMessage;
                sendMessage.mentionType = 'channel';                       // Either 'users' or 'channel'
                sendMessage.metaArrayKeys = ['linkTo', 'itemType'];
                sendMessage.customType = 'GROUPCHAT';
                let response = await this.sbSendMessage(chatChannel, sendMessage, chatObj) as chatResponse;
                if (response.statusCode === 100) {
                    chatObj.chatData.push(response.object);
                    this.cdr.detectChanges();
                    chatChannel.endTyping();
                    BuyerSendbirdComponent.scrollBottomDiv(chatObj.driverInfo);
                    await this.sendPushNotification(chatObj, textMessage, email);
                }
                else {
                    Declarations.msgerror("Error in send message-sendbird." + response.object, undefined, undefined);
                }
                this.sbSendbird.get('textMessage').setValue('');
            }

        }
    }
    public sbSendMessage(chatChannel: any, sendMessage: any, chatObj: ChatMessage) {
        return new Promise((resolve) => {
            let response = new chatResponse();
            chatChannel.sendUserMessage(sendMessage, function (message, error) {
                if (error) {
                    response.statusCode = 404;
                    response.object = error;
                    resolve(response);
                }
                else {
                    let date = new Date(message.createdAt);
                    let hourDate = BuyerSendbirdComponent.formatAMPM(date);
                    let monthName = BuyerSendbirdComponent.getMonthName(date);
                    let day = date.getDate();
                    let year = date.getFullYear();
                    let chatMessageObj = BuyerSendbirdComponent.loadsendMessage(chatObj.chatData, chatObj.currentLoginUserInfo, message, hourDate, monthName, day, year, chatObj) as ChatData;
                    response.statusCode = 100;
                    response.object = chatMessageObj;
                    resolve(response);
                }
            });
        });
    }
    typingIndicatorEvent(email: string, event: any) {
        let chatObj = this.chatCollection.find(top => top.driverInfo.Email === email);
        if (chatObj != null) {
            let chatChannel = chatObj.groupChannelInfo.groupChannelObj;
            chatChannel.startTyping();
        }
        let keyCode = (event.keyCode ? event.keyCode : event.which);
        if (keyCode == '13') {
            this.sendMessage(email);
        }
    }
    public async sendPushNotification(chatObj: ChatMessage, textMessage: string, email: string) {
        if (chatObj.driverInfo.firstMessage === false && chatObj.driverInfo.ConnectionStatus === false) {
            let userIds = [email];
            let userDetails = await this.sbGetUserDetails(userIds) as chatResponse;
            if (userDetails.statusCode === 100) {
                let users = userDetails.object;
                users.forEach(xitem => {
                    if (xitem.connectionStatus.toUpperCase() != 'ONLINE') {
                        //send push notification to driver.
                        this.chatService.sendPushNotification(textMessage, chatObj.driverInfo.Id).subscribe(t => {
                            let statusMessage = t as any;
                            if (statusMessage.StatusCode === 0) {
                                Declarations.msgsuccess("Notification sent to driver successfully.", undefined, undefined);
                            }
                            else {
                                Declarations.msgwarning(statusMessage.StatusMessage, undefined, undefined);
                            }
                        });
                    }
                });
            }
        }
        if (chatObj.driverInfo.firstMessage) {
            //send push notification to driver.
            this.chatService.sendPushNotification(textMessage, chatObj.driverInfo.Id).subscribe(t => {
                let statusMessage = t as any;
                if (statusMessage.StatusCode === 0) {
                    Declarations.msgsuccess("Notification sent to driver successfully.", undefined, undefined);
                }
                else {
                    Declarations.msgwarning(statusMessage.StatusMessage, undefined, undefined);
                }
            });

            chatObj.driverInfo.firstMessage = false;
        }
    }
    public static loadsendMessage(chatData: ChatData[], currentUserInfo: Carriar, value: any, hourDate: any, monthName: any, day: any, year: any, chatObj: ChatMessage) {
        let chatItem = new ChatData();
        let monthDName = monthName.substring(0, 3).toUpperCase();
        if (value._sender.nickname.trim().length === 0) {
            if (chatObj.driverInfo.Email === value._sender.userId) {
                value._sender.nickname = chatObj.driverInfo.FullName;
            }
            else {
                var dispatcherDetails = chatObj.companyUserInfo.find(top => top.EmailAddress === value._sender.userId);
                if (dispatcherDetails != null) {
                    value._sender.nickname = dispatcherDetails.FullName;
                }
            }
        }
        if (value._sender.userId === currentUserInfo.userName) {
            chatItem.messageId = value.messageId;
            chatItem.createdAt = value.createdAt;
            chatItem.channelURL = value.channelUrl;
            chatItem.channelType = value.channelType;
            chatItem.messageType = 1;
            chatItem.nickname = value._sender.nickname;
            chatItem.message = value.message;
            chatItem.dateTime = hourDate;
            chatItem.fulldateTime = hourDate + "," + monthName + "," + day;
            if (chatData.findIndex(top => top.headerText === day + " " + monthDName + " " + year) === -1) {
                chatItem.headerText = day + " " + monthDName + " " + year;
            }
            else {
                chatItem.headerText = '';
            }
            chatItem.userId = value._sender.userId;
        }
        else {
            chatItem.messageId = value.messageId;
            chatItem.createdAt = value.createdAt;
            chatItem.channelURL = value.channelUrl;
            chatItem.channelType = value.channelType;
            chatItem.messageType = 2;
            chatItem.nickname = value._sender.nickname;
            chatItem.message = value.message;
            chatItem.dateTime = hourDate;
            chatItem.fulldateTime = hourDate + "," + monthName + "," + day;
            if (chatData.findIndex(top => top.headerText === day + " " + monthDName + " " + year) === -1) {
                chatItem.headerText = day + " " + monthDName + " " + year;
            }
            else {
                chatItem.headerText = '';
            }
            chatItem.userId = value._sender.userId;
        }
        return chatItem;
    }
    public checkUserOnline = (userId) => {
        return new Promise((resolve) => {
            let response = new chatResponse();
            let applicationUserListQuery = this.sb.createApplicationUserListQuery();
            applicationUserListQuery.userIdsFilter = [userId];
            applicationUserListQuery.next(function (users, error) {
                if (error) {
                    response.statusCode = 404;
                    response.object = error;
                }
                if (users[0].connectionStatus.toUpperCase() === "ONLINE") {
                    response.statusCode = 100;
                    response.object = "ONLINE";
                    // User.connectionStatus consists of NON_AVAILABLE, ONLINE, and OFFLINE.
                }
                if (users[0].connectionStatus.toUpperCase() === "OFFLINE") {
                    response.statusCode = 100;
                    response.object = "OFFLINE";
                    // User.connectionStatus consists of NON_AVAILABLE, ONLINE, and OFFLINE.
                }
                if (users[0].connectionStatus.toUpperCase() === "NON_AVAILABLE") {
                    response.statusCode = 100;
                    response.object = "NON_AVAILABLE";
                    // User.connectionStatus consists of NON_AVAILABLE, ONLINE, and OFFLINE.
                }
                resolve(response);
            });
        });
    }

    public sbLoadMoreMessage = async (userId) => {
        let groupChannelObj = this.chatCollection.find(top => top.driverInfo.Email === userId);
        if (groupChannelObj) {
            groupChannelObj.showsmallLoder = true;
            let carrierObj = groupChannelObj.currentLoginUserInfo;
            let groupChannel = groupChannelObj.groupChannelInfo.groupChannelObj;
            let messageID = groupChannelObj.chatData[0].messageId;
            let userIds: any = [];
            groupChannelObj.groupChannelInfo.memberInfo.forEach(comItem => {
                userIds.push(comItem.userId);
            });
            groupChannelObj.chatData.forEach(comItem => {
                comItem.headerText = '';
            });
            let response = await this.sbGetPreviousMessage(groupChannel, messageID, groupChannelObj, carrierObj, userIds) as chatResponse;
            if (response.statusCode === 100) {
                let chatData = response.object as ChatData[];
                let finalData = chatData.concat(groupChannelObj.chatData);
                finalData.forEach(xItem => {
                    let date = new Date(xItem.createdAt);
                    let monthName = BuyerSendbirdComponent.getMonthName(date);
                    let monthDName = monthName.substring(0, 3).toUpperCase();
                    let day = date.getDate();
                    let year = date.getFullYear();
                    if (finalData.findIndex(top => top.headerText === day + " " + monthDName + " " + year) === -1) {
                        xItem.headerText = day + " " + monthDName + " " + year;
                    }
                    else {
                        xItem.headerText = '';
                    }
                });
                groupChannelObj.chatData = finalData;
                groupChannelObj.showsmallLoder = false;
            }
        }
    };
    public async sbGetPreviousMessage(groupChannel: any, messageID: any, groupChannelObj: ChatMessage, carrierObj: Carriar, userIds: []) {
        return new Promise((resolve) => {
            let response = new chatResponse();
            response.statusCode = 100;
            groupChannel.getPreviousMessagesByID(messageID, true, BuyerSendbirdComponent.messageLoadLimit, false, 'MESG', '', userIds, false, false,
                function (messages, error) {
                    if (error) {
                        response.statusCode = 400;
                        response.object = error;
                        resolve(response);
                    }
                    else {
                        if (messages.length > 0) {
                            if (messages.length >= BuyerSendbirdComponent.messageLoadLimit) {
                                groupChannelObj.loadmore = true;
                            }
                            else {
                                groupChannelObj.loadmore = false;
                            }
                        }
                        let chatData: ChatData[] = [];
                        messages.forEach(function (value) {
                            let date = new Date(value.createdAt);
                            let hourDate = BuyerSendbirdComponent.formatAMPM(date);
                            let monthName = BuyerSendbirdComponent.getMonthName(date);
                            let day = date.getDate();
                            if (value._sender.nickname.trim().length === 0) {
                                if (groupChannelObj.driverInfo.Email === value._sender.userId) {
                                    value._sender.nickname = groupChannelObj.driverInfo.FullName;
                                }
                                else {
                                    var dispatcherDetails = groupChannelObj.companyUserInfo.find(top => top.EmailAddress === value._sender.userId);
                                    if (dispatcherDetails != null) {
                                        value._sender.nickname = dispatcherDetails.FullName;
                                    }
                                }
                            }
                            if (value._sender.userId === carrierObj.userName) {
                                let chatItem = new ChatData();
                                chatItem.messageId = value.messageId;
                                chatItem.createdAt = value.createdAt;
                                chatItem.channelURL = value.channelUrl;
                                chatItem.channelType = value.channelType;
                                chatItem.messageType = 1;
                                chatItem.nickname = value._sender.nickname;
                                chatItem.message = value.message;
                                chatItem.dateTime = hourDate;
                                chatItem.fulldateTime = hourDate + "," + monthName + "," + day;
                                chatItem.headerText = '';
                                chatItem.userId = value._sender.userId;
                                chatData.push(chatItem);
                            }
                            else {
                                let chatItem = new ChatData();
                                chatItem.messageId = value.messageId;
                                chatItem.createdAt = value.createdAt;
                                chatItem.channelURL = value.channelUrl;
                                chatItem.channelType = value.channelType;
                                chatItem.messageType = 2;
                                chatItem.nickname = value._sender.nickname;
                                chatItem.message = value.message;
                                chatItem.dateTime = hourDate;
                                chatItem.fulldateTime = hourDate + "," + monthName + "," + day;
                                chatItem.headerText = '';
                                chatItem.userId = value._sender.userId;
                                chatData.push(chatItem);
                            }
                        });
                        response.object = chatData;
                        resolve(response);
                    }
                });
        });
    }
    public async sbCheckUserOnline() {
        let userDetailsArray = [];
        this.chatCollection.forEach(xitem => {
            userDetailsArray.push(xitem.driverInfo.Email);
        });
        let userDetails = await this.sbGetUserDetails(userDetailsArray) as chatResponse;
        if (userDetails.statusCode == 100) {
            let users = userDetails.object;
            users.forEach(xitem => {
                let userCollection = this.chatCollection.find(x => x.driverInfo.Email === xitem.userId);
                if (userCollection) {
                    userCollection.driverInfo.ConnectionStatus = xitem.connectionStatus.toUpperCase() === 'ONLINE' ? true : false;
                    if (userCollection.driverInfo.ConnectionStatus === false) {
                        if (xitem.lastSeenAt > 0) {
                            let lastSeen = new Date(xitem.lastSeenAt);
                            let hourDate = BuyerSendbirdComponent.formatAMPM(lastSeen);
                            let monthName = BuyerSendbirdComponent.getMonthName(lastSeen);
                            let day = lastSeen.getDate();
                            userCollection.driverInfo.templastSeenAt = monthName + "," + day + "," + hourDate;
                            userCollection.driverInfo.lastSeenAt = SendBirdCommonComponent.getLastSeen(lastSeen);
                        }
                        else {
                            userCollection.driverInfo.lastSeenAt = 'NO';
                        }
                    }
                    this.cdr.detectChanges();
                }
            });
        }
    }
    // #endregion
    // #region sendbirdEventHandler
    public onTypingStatusUpdated = (groupChannel) => {
        this.chatCollection.forEach(xitem => {
            xitem.driverInfo.TypingIndicator = false;
        });
        if (groupChannel) {
            let objectFound = this.chatCollection.find(top => top.groupChannelInfo.groupChannelName === groupChannel.name);
            if (objectFound) {
                let members = groupChannel.getTypingMembers();
                members.forEach(function (value) {
                    if (objectFound.driverInfo.Email === value.userId) {
                        if (value.connectionStatus.toUpperCase() === 'ONLINE') {
                            objectFound.driverInfo.ConnectionStatus = true;
                        }
                    }
                    if (objectFound.currentLoginUserInfo.userName === value.userId) {
                        objectFound.driverInfo.TypingIndicator = false;
                    }
                    else {
                        objectFound.driverInfo.TypingIndicator = true;
                    }
                    objectFound.driverInfo.State = value.state.toUpperCase();
                    objectFound.driverInfo.TypingMemberName = value.nickname;
                });
            }
        }
        this.cdr.detectChanges();
    };
    public onMessageReceived = (channel, message) => {
        if (channel != null && message != null) {
            let chatObj = this.chatCollection.find(top => top.groupChannelInfo.groupChannelName === channel.name);
            if (chatObj) {
                let date = new Date(message.createdAt);
                let hourDate = BuyerSendbirdComponent.formatAMPM(date);
                let monthName = BuyerSendbirdComponent.getMonthName(date);
                let day = date.getDate();
                let year = date.getFullYear();
                let chatMesage = BuyerSendbirdComponent.loadsendMessage(chatObj.chatData, chatObj.currentLoginUserInfo, message, hourDate, monthName, day, year, chatObj) as ChatData;
                let messageFound = chatObj.chatData.findIndex(top => top.messageId === chatMesage.messageId);
                if (messageFound == -1) {
                    chatObj.chatData.push(chatMesage);
                    this.cdr.detectChanges();
                    //add chat-active class
                    if (chatObj.divId != '') {
                        let dvObj = $("#" + chatObj.divId);
                        if (dvObj != undefined) {
                            if (dvObj.hasClass('chat-minimized')) {
                                if (!dvObj.hasClass('chat-active')) {
                                    dvObj.addClass('chat-active');
                                }
                            }
                        }
                    }
                }
            }
            else {
                //call from the background.
                let channelMember = [], currentDriverDetails = [];
                let groupMemberDetails = channel.members;
                groupMemberDetails.forEach(item => {
                    channelMember.push(item.userId);
                });
                this.backgrounddriverDetails.forEach(item => {
                    currentDriverDetails.push(item.driverInfo.Email)
                });
                //Get common elements of arr1, arr2
                let commonElements = this.getCommon(channelMember, currentDriverDetails);
                if (commonElements.length > 0) {
                    for (const commonelement of commonElements) {
                        let driverDetails = this.backgrounddriverDetails.find(x => x.driverInfo.Email === commonelement);
                        if (driverDetails != null) {
                            //remove item from backgroup thread if exists
                            this.IntializeDriverChat(driverDetails.driverInfo.Id, driverDetails.regionId);
                            let index = this.backgrounddriverDetails.findIndex(x => x.driverInfo.Id === driverDetails.driverInfo.Id);
                            if (index != -1) {
                                this.backgrounddriverDetails.splice(index, 1);
                            }
                        }
                    }
                }
            }
            this.cdr.detectChanges();
        }
    };
    public onReadReceiptUpdated = (groupChannel) => {

    };
    // #endregion


    //call for schedulebuilder component.
    public IntializeDriverChat(driverId, regionID) {
        this.IntializeSendbird(driverId, regionID);

    }
    //intialize background chat
    public IntializeChatDefault(driverDetails: any[], regionId) {
        this.backgrounddriverDetails = [];
        this.chatService.getSendBirdAPIKey().subscribe(t => {
            this.appKey = t.Value as string;
            this.sb = new SendBird({ appId: this.appKey });
            this.IntializeChannelHandler();
            this.intervalTime = setInterval(() => {
                if (IsUserActive()) {
                    this.sbCheckUserOnline();
                }
            }, 45000);
            if (driverDetails.length > 0) {
                this.chatService.getDriversDetails(driverDetails).subscribe(t => {
                    if (t.StatusCode === 302) {
                        let driverObj = t.Data as Driver[];
                        driverObj.forEach(xitem => {
                            let findIndex = this.backgrounddriverDetails.findIndex(x => x.driverInfo.Email === xitem.Email);
                            if (findIndex === -1) {
                                let chatdefault = new BackgroupChatIntialize();
                                chatdefault.driverInfo = xitem;
                                chatdefault.regionId = regionId;
                                this.backgrounddriverDetails.push(chatdefault);
                            }
                            this.sb.connect(xitem.Email, function (user, error) {
                                if (error) {
                                    return;
                                }
                                else {
                                    console.log("IntializeChatDefault", user);
                                }
                            });
                        });
                    }

                });
            }

        })
    }
    public async groupInfo(email: string) {
        let records = this.chatCollection.find(top => top.driverInfo.Email === email);
        if (records != null) {
            this.chatService.loaderElement(true);
            let groupChannel = records.groupChannelInfo.groupChannelObj;
            let driverObj = records.driverInfo;
            let companyUserInfo = records.companyUserInfo;
            let memberInfos = await this.sbChannelMemberDetails(groupChannel, driverObj, companyUserInfo) as MemberInfo[];
            if (memberInfos.length > 0) {
                groupChannel.memberInfo = memberInfos;
                this.chatService.sendMemberInfo(memberInfos);
            }
            else {
                this.chatService.loaderElement(false);
            }
        }
    }

    // #region CommonFunctions
    private getCommon(arr1, arr2) {
        let common = [];                   // Array to contain common elements
        for (let i = 0; i < arr1.length; ++i) {
            for (let j = 0; j < arr2.length; ++j) {
                if (arr1[i] == arr2[j]) {       // If element is in both the arrays
                    common.push(arr1[i]);        // Push to common array
                }
            }
        }
        return common;                     // Return the common elements
    }
    private arr_diff(a1, a2) {

        let a = [], diff = [];
        for (let i = 0; i < a1.length; i++) {
            a[a1[i]] = true;
        }
        for (let i = 0; i < a2.length; i++) {
            if (a[a2[i]]) {
                delete a[a2[i]];
            } else {
                a[a2[i]] = true;
            }
        }
        for (let k in a) {
            diff.push(k);
        }

        return diff;
    }

    // #endregion
}   
