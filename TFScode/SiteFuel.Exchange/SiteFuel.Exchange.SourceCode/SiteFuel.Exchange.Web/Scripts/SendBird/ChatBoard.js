
$(document).ready(function () {
    var typingIndicatorIds = [];
    var currentMessageList = [];
    var unreadMessageCountList = [];
    var channelURL = '';
    var chatting_groupChannel = null;
    var accessToken = 'c9b55be7e1eb4d44e7124a6d616df3f56ca4c98b';
    sb.connect(USER_ID, accessToken, function (user, error) {
        if (error) {
            return;
        }
        if (sb.currentUser.nickname != DisplayName) {
            sb.updateCurrentUserInfo(DisplayName, sb.currentUser.profileUrl, function (response, error) {
                if (error) {
                    return;
                }
                if (response) {
                    return;
                }
            });
        }
    });

    var channelListQuery = sb.GroupChannel.createMyGroupChannelListQuery();
    channelListQuery.includeEmpty = true;
    channelListQuery.order = 'latest_last_message'; // 'chronological', 'latest_last_message', 'channel_name_alphabetical', and 'metadata_value_alphabetical'
    channelListQuery.limit = 15;    // The value of pagination limit could be set up to 100.
    if (channelListQuery.hasNext) {
        channelListQuery.next(function (channelList, error) {
            if (error) {
                return;
            }
            if (channelList) {
                var htmldata = '';
                $.each(channelList, function (index, value) {
                    var members = value.members[0];
                    var uniqueId = value._iid.split('-')[0];
                    if (value.userId != USER_ID) {
                        htmldata = htmldata + '<div class="chat_list" onclick="retriveChat(\'' + value.name + '\',this);" id="div_' + uniqueId + '"><div class="chat_people" id=div_' + uniqueId + '><div class="chat_img"><img src=' + members.profileUrl + ' alt=' + value.name + '></div><div class="chat_ib"><h5>' + value.name + ' <div class="badge" style="display:none;"></div><span class="chat_date">' + members.connectionStatus.toUpperCase() + '</span></h5></div></div></div>';
                    }
                });
                $('.inbox_chat').html('');
                $('.inbox_chat').html(htmldata);
            }
        });
    }
    channelListQuery.next(function (users, error) {
        if (error) {
            return;
        }
        if (users) {
            var htmldata = '';
            $.each(sortByKeyDesc(users, "connectionStatus"), function (index, value) {
                var uniqueId = value._iid.split('-')[0];
                if (value.userId != USER_ID) {
                    htmldata = htmldata + '<div class="chat_list" onclick="retriveChat(\'' + value.userId + '\',this);" id="div_' + value.userId + '"><div class="chat_people" id=div_' + uniqueId + '><div class="chat_img"><img src=' + value.profileUrl + ' alt=' + value.nickname + '></div><div class="chat_ib"><h5>' + value.nickname + ' <div class="badge" style="display:none;"></div><span class="chat_date">' + value.connectionStatus.toUpperCase() + '</span></h5></div></div></div>';
                }
            });
            $('.inbox_chat').html('');
            $('.inbox_chat').html(htmldata);
        }
    });

});
$(document).on("keypress", "#txtText", function (event) {
    chatting_groupChannel.startTyping();
    var keycode = (event.keyCode ? event.keyCode : event.which);
    if (keycode == '13') {
        sendMsg(activeUserId);
    }
});
//function started

function retriveChat(userId, currentdom) {
    $(".chat_list").removeClass('active_chat');
    $(currentdom).addClass('active_chat');
   var activeUserId = userId;
    //create the channel
    createChannelANDRetriveChat(userId, USER_ID);
    //checkUserOnline(userId, currentdom);
}
function leaveChannel() {
    chatting_groupChannel.leave(function (response, error) {
        if (error) {
            return;
        }
    });
}
function loadtheChatData(userId) {
    var prevMessageListQuery = chatting_groupChannel.createPreviousMessageListQuery();
    prevMessageListQuery.limit = 30;
    prevMessageListQuery.reverse = false;
    // Retrieving previous messages.
    prevMessageListQuery.load(function (messages, error) {
        if (error) {
            return;
        }
        loadMsg(messages, USER_ID);
    });
}
//create the channel if not created. if created then retrive the existing channel.
function createChannelANDRetriveChat(channelName,userId) {


    //used for create the channel or if created then return the channel URL.
    var myChannelListQuery = sb.GroupChannel.createMyGroupChannelListQuery();
    myChannelListQuery.includeEmpty = true;
    myChannelListQuery.channelNameContainsFilter = channelName;
    myChannelListQuery.next(function (groupChannels, error) {
        if (error) {
            return;
        }
        if (!jQuery.isEmptyObject(groupChannels)) {
           var channelURL = groupChannels[0].url;
             var chatting_groupChannel = groupChannels[0];
            // Call the 'markAsRead()' when the current user views unread messages in a group channel. 
            chatting_groupChannel.markAsRead();
            loadtheChatData(USER_ID);
        }
        else {
            var groupChannelparams = new sb.GroupChannelParams();
            groupChannelparams.isPublic = false;
            groupChannelparams.isEphemeral = false;
            groupChannelparams.isDistinct = false;
            groupChannelparams.addUserIds(userIds);
            groupChannelparams.name = 'PERSONALCHAT';
            groupChannelparams.customType = 'CUSTOM_PERSONALCHAT';
            sb.GroupChannel.createChannel(groupChannelparams, function (groupChannel, error) {
                if (error) {
                    return;
                }
                if (groupChannel) {
                    channelURL = groupChannel.url;
                    chatting_groupChannel = groupChannel;
                    // Call the 'markAsRead()' when the current user views unread messages in a group channel. 
                    chatting_groupChannel.markAsRead();
                    loadtheChatData(userId);
                }
            });
        }
    });
}
function loadMsg(sendMessageData, userId) {
    var chatHTML = '';
    var MessageTextboxHTML = '<div class="type_msg" id=div_' + userId + '><div class="input_msg_write"><input type="text" class="write_msg" placeholder="Type a message" id="txtText" txtuserid="' + userId + '" /> <button class="msg_send_btn" type="button" onclick=sendMsg(\'' + userId + '\')><i class="fa fa-paper-plane-o" aria-hidden="true"></i></button></div></div>';
    $.each(sendMessageData, function (key, value) {
        var date = new Date(value.createdAt);
        var hourDate = formatAMPM(date);
        var monthName = getMonthName(date);
        var day = date.getDay();
        var dateHTML = '<span class="time_date"> ' + hourDate + '    |    ' + monthName + ' ' + day + '</span>';
        if (value._sender.userId == USER_ID) {
            var outgoinghtml = '<div class="outgoing_msg">'
            var outgoinghtmlEND = '</div>';
            var sendMsgdiv = '<div class="sent_msg">';
            var sendMsgdivEND = '</div>';
            chatHTML = chatHTML + outgoinghtml + sendMsgdiv + '<p>' + value.message + '</p>' + dateHTML + sendMsgdivEND + outgoinghtmlEND;
        }
        else  {
            var incominghtml = '<div class="incoming_msg">'
            var incominghtmlEND = '</div>';
            var incomingMsgdiv = '<div class="received_msg">';
            var incomingMsgdivEND = '</div>';
            var incomingMsgwithddiv = '<div class="received_withd_msg">'
            var incomingMsgwithddivEND = '</div>';
            var profilePICdiv = '<div class="incoming_msg_img"><img src=' + value._sender.profileUrl + ' alt="' + value._sender.nickname + '"> </div>';
            chatHTML = chatHTML + incominghtml + profilePICdiv + incomingMsgdiv + incomingMsgwithddiv + '<p>' + value.message + '</p>' + dateHTML + incomingMsgwithddivEND + incomingMsgdivEND + incominghtmlEND;

        }

    });
    var finalHTML = mshHistoryHTML + chatHTML + mshHistoryHTMLEND + MessageTextboxHTML;
    $('.mesgs').html('');
    $('.mesgs').html(finalHTML);
    $(".msg_history").scrollTop($(".msg_history")[0].scrollHeight);
}
//load the messages
function loadSendMsg(sendMessageData, userId) {
    var chatHTML = '';
    var value = sendMessageData;
    var uniqueId = value.messageId;
    if ($("#div_out" + uniqueId)[0] != undefined) {
        return;
    }
    if ($("#div_inc" + uniqueId)[0] != undefined) {
        return;
    }
    var date = new Date(value.createdAt);
    var hourDate = formatAMPM(date);
    var monthName = getMonthName(date);
    var day = date.getDay();
    var dateHTML = '<span class="time_date"> ' + hourDate + '    |    ' + monthName + ' ' + day + '</span>';
    if (value._sender.userId == USER_ID) {
        var outgoinghtml = '<div class="outgoing_msg" id=div_out' + uniqueId + '>'
        var outgoinghtmlEND = '</div>';
        var sendMsgdiv = '<div class="sent_msg">';
        var sendMsgdivEND = '</div>';
        chatHTML = chatHTML + outgoinghtml + sendMsgdiv + '<p>' + value.message + '</p>' + dateHTML + sendMsgdivEND + outgoinghtmlEND;
    }
    else {
        var incominghtml = '<div class="incoming_msg" id=div_inc' + uniqueId + '>'
        var incominghtmlEND = '</div>';
        var incomingMsgdiv = '<div class="received_msg">';
        var incomingMsgdivEND = '</div>';
        var incomingMsgwithddiv = '<div class="received_withd_msg">'
        var incomingMsgwithddivEND = '</div>';
        var profilePICdiv = '<div class="incoming_msg_img"><img src=' + value._sender.profileUrl + ' alt="' + value._sender.nickname + '"> </div>';
        chatHTML = chatHTML + incominghtml + profilePICdiv + incomingMsgdiv + incomingMsgwithddiv + '<p>' + value.message + '</p>' + dateHTML + incomingMsgwithddivEND + incomingMsgdivEND + incominghtmlEND;

    }
    $('.msg_history').append(chatHTML);
    $(".msg_history").scrollTop($(".msg_history")[0].scrollHeight);
}
//get the monthName
function getMonthName(date) {
    const monthNames = ["January", "February", "March", "April", "May", "June",
        "July", "August", "September", "October", "November", "December"
    ];
    return monthNames[date.getMonth()];
}
//format AM PM.
function formatAMPM(date) {
    var hours = date.getHours();
    var minutes = date.getMinutes();
    var ampm = hours >= 12 ? 'PM' : 'AM';
    hours = hours % 12;
    hours = hours ? hours : 12; // the hour '0' should be '12'
    minutes = minutes < 10 ? '0' + minutes : minutes;
    var strTime = hours + ':' + minutes + ' ' + ampm;
    return strTime;
}
//send the message
function sendMsg(userId) {

    var textboxValue = $("#txtText").val(); //get the textbox value
    if (chatting_groupChannel != null) {
        const sendMessage = new sb.UserMessageParams();
        sendMessage.message = textboxValue;
        sendMessage.mentionType = 'channel';                       // Either 'users' or 'channel'
        sendMessage.metaArrayKeys = ['linkTo', 'itemType'];
        chatting_groupChannel.sendUserMessage(sendMessage, function (message, error) {
            if (error) {
                return;
            }
            loadSendMsg(message, USER_ID);
            $("#txtText").val('');
            //remove the Typing indicator.
            chatting_groupChannel.endTyping();
            currentMessageList.push(message);
            processOneUnreadMessageCount(chatting_groupChannel, message);

        });
    }
}

//reset the chat history.
function resetChatHistory() {
    chatting_groupChannel.resetMyHistory(function (response, error) {
        if (error) {
            return;
        }
    });
}
//refresh the current Channel
function refreshChannel() {
    chatting_groupChannel.refresh(function (response, error) {
        if (error) {
            return;
        }
    });
}
//retrive the channel members
function retriveChannelMembers() {
    return chatting_groupChannel.members;
}

function checkUserOnline(userId, checkUserOnline) {
    var applicationUserListQuery = sb.createApplicationUserListQuery();
    applicationUserListQuery.userIdsFilter = [userId];
    applicationUserListQuery.next(function (users, error) {
        if (error) {
            return;
        }

        if (users[0].connectionStatus === sb.User.ONLINE) {
            $(checkUserOnline).find(".chat_date").text(sb.User.ONLINE.toUpperCase());
            // User.connectionStatus consists of NON_AVAILABLE, ONLINE, and OFFLINE.
        }
        if (users[0].connectionStatus === sb.User.OFFLINE) {
            $(checkUserOnline).find(".chat_date").text(sb.User.OFFLINE.toUpperCase());
            // User.connectionStatus consists of NON_AVAILABLE, ONLINE, and OFFLINE.
        }
        if (users[0].connectionStatus === sb.User.NON_AVAILABLE) {
            $(checkUserOnline).find(".chat_date").text(sb.User.NON_AVAILABLE.toUpperCase());
            // User.connectionStatus consists of NON_AVAILABLE, ONLINE, and OFFLINE.
        }
    });
}

function sortByKeyDesc(array, key) {
    return array.sort(function (a, b) {
        var x = a[key]; var y = b[key];
        return ((x > y) ? -1 : ((x < y) ? 1 : 0));
    });
}
function sortByKeyAsc(array, key) {
    return array.sort(function (a, b) {
        var x = a[key]; var y = b[key];
        return ((x < y) ? -1 : ((x > y) ? 1 : 0));
    });
}
//function end

//channel handler
ChannelHandler.onMessageReceived = function (channel, message) {
    if (channel.url == channelURL) {
        loadSendMsg(message, activeUserId);
    }
};
sb.addChannelHandler("messageReceiveHandler", ChannelHandler);

ChannelHandler.onTypingStatusUpdated = function (groupChannel) {
    var members = groupChannel.getTypingMembers();
    $(".chat_ib p").remove();
    // Refresh typing status of members within channel.
    $.each(members, function (index, value) {
        var uniqueId = value._iid.split('-')[0];
        var divId = $("#div_" + uniqueId);
        var pTagId = 'pTag_' + uniqueId;
        if ($("#" + pTagId)[0] == undefined) {
            var pTag = '<p id=' + pTagId + '>Typing...</p>';
            $(divId).find('.chat_ib').append(pTag);
        }
    });

};

sb.addChannelHandler("onTypingStatusUpdated", ChannelHandler);

ChannelHandler.onReadReceiptUpdated = function (groupChannel) {
    processUnreadMessageCount(groupChannel);
};


sb.addChannelHandler("onReadReceiptUpdated", ChannelHandler);



//channel handler ended

//unload the window
$(window).on("unload", function (e) {
    sb.removeChannelHandler("onTypingStatusUpdated");
    sb.removeChannelHandler("messageReceiveHandler");
    sb.removeChannelHandler("onReadReceiptUpdated");
});

//methods.

const processOneUnreadMessageCount = (groupChannel, message) => {
    const unreadMembers = groupChannel.getUnreadMembers(message);
    var objIndex = unreadMessageCountList.findIndex((obj => obj.userId === unreadMembers[0].userId && obj.currentUserId === USER_ID));
    if (objIndex > -1) {
        unreadMessageCountList[objIndex].TotalUnreadCount = unreadMessageCountList[objIndex].TotalUnreadCount + unreadMembers.length;
    }
    else {
        var currentMsg = new Object();
        currentMsg.userId = unreadMembers[0].userId;
        currentMsg.currentUserId = USER_ID;
        currentMsg.chaanelURL = groupChannel.url;
        currentMsg.TotalUnreadCount = unreadMembers.length;
        unreadMessageCountList.push(currentMsg);
    }
};
const processUnreadMessageCount = (groupChannel) => {
        currentMessageList.forEach(message => {
            const unreadMembers = groupChannel.getUnreadMembers(message);
            if (unreadMembers.length > 0) {
                var objIndex = unreadMessageCountList.findIndex((obj => obj.userId === unreadMembers[0].userId && obj.currentUserId === USER_ID));
                if (objIndex > -1) {
                    unreadMessageCountList[objIndex].TotalUnreadCount = unreadMessageCountList[objIndex].TotalUnreadCount + unreadMembers.length;
                }
                else {
                    var currentMsg = new Object();
                    currentMsg.userId = unreadMembers[0].userId;
                    currentMsg.currentUserId = USER_ID;
                    currentMsg.chaanelURL = groupChannel.url;
                    currentMsg.TotalUnreadCount = unreadMembers.length;
                    unreadMessageCountList.push(currentMsg);
                }
            }
        });
        return unreadMessageCountList;
    

};
function disconnectuser() {
    sb.disconnect(USER_ID, function () {
        // A current user is discconected from SendBird server.
    });
}