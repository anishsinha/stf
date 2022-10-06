import { __decorate } from "tslib";
import { Pipe } from '@angular/core';
let FilterPipe = class FilterPipe {
    transform(items, field, value) {
        if (!items) {
            return [];
        }
        if (!field || !value) {
            return items;
        }
        var filtered = items.filter(singleItem => singleItem[field].value == value);
        return filtered;
    }
};
FilterPipe = __decorate([
    Pipe({
        name: 'filter'
    })
], FilterPipe);
export { FilterPipe };
//# sourceMappingURL=filter.pipe.js.map