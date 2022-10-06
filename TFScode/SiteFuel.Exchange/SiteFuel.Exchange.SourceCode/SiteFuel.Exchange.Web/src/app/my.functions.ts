export const sum = (array: any, key: string) => {
    return array.reduce((sum, thisObject) => {
        return (thisObject['ScheduleQuantityType'] > 1) ? sum : sum + (thisObject[key] as number);
    }, 0);
}

export const min = (array: any, key: string) => {
    return array.reduce((min, thisObject) => {
        let thisNumber = thisObject[key] as number;
        min = min > thisNumber ? thisNumber : min;
        return min;
    }, Number.MAX_VALUE);
}

export const max = (array: any, key: string) => {
    return array.reduce((max, thisObject) => {
        let thisNumber = thisObject[key] as number;
        max = max > thisNumber ? max : thisNumber;
        return min;
    }, Number.MIN_VALUE);
}

// Accepts the array and key
export const groupBy = (array: any, key: string) => {
    // Return the end result
    let index = 0;
    return array.reduce((result, currentValue) => {
        // If an array already present for key, push it to the array. Else create an array and push the object
        (result[currentValue[key]] = result[currentValue[key]] || []).push(currentValue);
        // Return the current iteration `result` value, this will be taken as next iteration `result` value and accumulate
        return result;
    }, {}); // empty object is the initial value for result object
};

// Accepts the array and key
export const sortBy = (array: any, key: string) => {
    // Return the end result
    return array.sort((t1, t2) => {
        const value1 = t1[key];
        const value2 = t2[key];
        if (value1 > value2) { return 1; }
        if (value1 < value2) { return -1; }
        return 0;
    });
}

export const sortByDesc = (array: any, key: string) => {
    // Return the end result
    return array.sort((t1, t2) => {
        const value1 = t1[key];
        const value2 = t2[key];
        if (value1 < value2) { return 1; }
        if (value1 > value2) { return -1; }
        return 0;
    });
}

export const sortArrayTwice = (array: any, key1: string, key2: string) => {
    try {
        array.sort((a, b) => a[key1].toString().localeCompare(b[key1].toString()) || a[key2] - b[key2]);
        
    }
    catch (Error) {
        console.log(Error);
       
    }
    return array;
}
export const sortByKeyAsc = (array, key) => {
    return array.sort(function (a, b) {
        var x = a[key]; var y = b[key];
        return ((x < y) ? -1 : ((x > y) ? 1 : 0));
    });
}
export const groupDrsByProduct = (drs: any): any => {
    let groupedDrs = groupBy(drs, 'ProductType');
    let keys = Object.keys(groupedDrs)
    let productWiseDrs = [];
    keys.forEach(key => {
        let groupItems = groupedDrs[key];
        if (groupItems && groupItems.length > 0) {
            let productDr = JSON.parse(JSON.stringify(groupItems[0]));
            productDr.RequiredQuantity = sum(groupItems, 'RequiredQuantity');
            productDr.IsDRMissed = groupItems.findIndex(t => t.ParentId != null) != -1;
            productDr.IsDRExists = true;
            productDr.SelectedDate = groupItems[0].SelectedDate;
            productDr.ScheduleStartTime = groupItems[0].ScheduleStartTime;
            productDr.ScheduleEndTime = groupItems[0].ScheduleEndTime;
            productDr.Priority = min(groupItems, 'Priority');
            productDr.Notes = getExistingValue(groupItems, 'Notes');
            productWiseDrs.push(productDr);
        }
    });
    return productWiseDrs;
}
export const groupDrsByMultipleKey = (array, f): any => {
    let groups = {};
    array.forEach(function (o) {
        var group = JSON.stringify(f(o));
        groups[group] = groups[group] || [];
        groups[group].push(o);
    });
    var keys = Object.keys(groups);
    let productWiseDrs = [];
    keys.forEach(key => {
        let groupItems = groups[key];
        if (groupItems && groupItems.length > 0) {
            let productDr = JSON.parse(JSON.stringify(groupItems[0]));
            productDr.RequiredQuantity = sum(groupItems, 'RequiredQuantity');
            productDr.IsDRMissed = groupItems.findIndex(t => t.ParentId != null) != -1;
            productDr.IsDRExists = true;
            productDr.Priority = min(groupItems, 'Priority');
            productDr.Notes = getExistingValue(groupItems, 'Notes');
            productWiseDrs.push(productDr);
        }
    });
    return productWiseDrs;
}
//export const groupDrsByBlendGroupId = (drs: any): any => {
//    let groupedDrs = groupBy(drs, 'BlendedGroupId');
//    let keys = Object.keys(groupedDrs)
//    let productWiseDrs = [];
//    keys.forEach(key => {
//        let groupItems = groupedDrs[key];
//        if (groupItems && groupItems.length > 0) {
//            let productDr = JSON.parse(JSON.stringify(groupItems[0]));
//            productDr.RequiredQuantity = sum(groupItems, 'RequiredQuantity');
//            productDr.BlendedProductName = getBlendProductName(groupItems);
//            productDr.AdditiveProductName = getAdditiveProductName(groupItems);
//            productWiseDrs.push(productDr);
//        }
//    });
//    return productWiseDrs;
//}

//export const getBlendProductName = (drs: any): any => {
//    return drs.filter(t => !t.IsAdditive).map(function (item) {
//        return item.ProductType;
//    }).filter((v, i, a) => a.indexOf(v) === i).join(", ");
//}

//export const getAdditiveProductName = (drs: any): any => {
//    return drs.filter(t => t.IsAdditive).map(function (item) {
//        return item.FuelType;
//    }).join(", ");
//}

export const getExistingValue = (array: any, key: string) => {
    return array.reduce((val, thisObject) => {
        let thisValue = thisObject[key] as string;
        if (thisValue != undefined && thisValue != null && thisValue != "") {
            val = val + thisValue + " ";
        }
        return val;
    }, "");
}

export const getUniqueId = (): string => {
    return Date.now().toString(36) + Math.random().toString(36).substr(2);
}
export const getRecurringUniqueId = (): string => {
    return "recurring" + Math.random().toString(16).slice(2);
}