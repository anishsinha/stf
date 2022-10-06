
export class MyLocalStorage {

    // Schedule Builder filter Keys---------------------------------
    public static DSB_DATE_KEY = "date";
    public static DSB_REGION_KEY = "regionId";
    public static DSB_OBJECTFILTER_KEY = "objectFilter";
    public static DSB_DATEFILTER_KEY = "dateFilter";
    public static DSB_WINDOWMODE_KEY = "windowMode";
    public static DSB_TOGGLEREQUESTMODE_KEY = "toggleRequestMode";
    public static DSB_READONLY_KEY = "readOnlyMode";
    public static DSB_FILTER_KEY = "dsbviewFilter";
    // Wally Boards Filter Keys-------------------------------------
    public static WBF_REGION_KEY = "wbf_regionId";
    public static WBF_CUSTOMER_KEY = "wbf_customerId";
    public static WBF_SEARCHEDKEYWORD_KEY = "wbf_searchedKeyword";
    public static WBF_SELECTEDSTATES_KEY = "wbf_selectedStates";
    public static WBF_LOCATION_KEY = "wbf_selectedLocations";
    public static WBF_SELECTEDPRIORY_KEY = "wbf_selectedPriority";
    public static WBF_SELECTEDSUPPLIER_KEY = "wbf_selectedSupplier";
    public static WBF_SELECTEDCARRIER_KEY = "wbf_selectedCarrier";
    public static WBF_SELECTEDDISPACHER_KEY = "wbf_selectedDispacher";
    public static WBF_FROMDATE_KEY = "wbf_fromDate";
    public static WBF_TODATE_KEY = "wbf_toDate";
    
    public static setData(key: string, value: any): void {
        if (!value) {
            localStorage.removeItem(key);
        } else {
            localStorage.setItem(key, JSON.stringify(value));
        }
    }

    public static getData(key: string): any {
        let value = localStorage.getItem(key);
        if (value) {
            value = JSON.parse(value);
        }
        if (value == 'null') {
            value = null;
        }
        return value || '';
    }
}