
declare function msgsuccess(msgText, msgHeader, msgDuration): any;
declare function msgerror(msgText, msgHeader, msgDuration, msgPosition?): any;
declare function msgwarning(msgText, msgHeader, msgDuration): any;
declare function msginfo(msgText, msgHeader, msgDuration): any;

declare function closeDG(): any;
declare function showDG(): any;
declare function showOpenDG(): any;

declare function hideModal(selector): any;
declare function showModal(selector): any;
declare function showSliderPanel(): any;
declare function hideSliderLoader(): any;
declare function appendHTMLSliderContent(htmlContent): any;
declare function slidePanel(PanelId, PanelWidth): any;
declare function closeSlidePanel(): any;
declare function formatTime(timeInput): any;

export class Declarations {
    public static msgsuccess = msgsuccess;
    public static msgerror = msgerror;
    public static msgwarning = msgwarning;
    public static msginfo = msginfo;
    public static closeDG = closeDG;
    public static showDG = showDG;
    public static showOpenDG = showOpenDG;

    public static hideModal = hideModal;
    public static showModal = showModal;
    public static showSliderPanel = showSliderPanel;
    public static hideSliderLoader = hideSliderLoader;
    public static appendHTMLSliderContent = appendHTMLSliderContent;
    public static slidePanel = slidePanel;
    public static closeSlidePanel = closeSlidePanel;
    public static ShiftVisibleRows: number = 10;
    public static formatTime = formatTime;
}
