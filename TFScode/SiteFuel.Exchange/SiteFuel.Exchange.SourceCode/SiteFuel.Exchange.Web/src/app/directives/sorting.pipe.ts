import { Injectable, Pipe, PipeTransform } from "@angular/core";

@Pipe({ name: 'sortingPipe' })

export class SortingPipe implements PipeTransform {

    transform(array: any[], filterArgs: any) {

        if (filterArgs) {

            return array.sort((t1, t2) => {

                const a = t1[filterArgs.key];
                const b = t2[filterArgs.key];

                if (a === b)
                    return 0;
                // nulls sort after anything else
                else if (a === null)
                    return 1;
                else if (b === null)
                    return -1;
                // otherwise, if we're ascending, lowest sorts first
                else if (filterArgs.asc)
                    return a < b ? -1 : 1;
                // if descending, highest sorts first
                else
                    return a < b ? 1 : -1;
            });
        }
        else
            return array;
    }
}

@Injectable({ providedIn: 'root' })

export class DatatableCustomSortingService {

    configColumnDefsNullToBottom(){
        jQuery.extend(jQuery.fn.dataTable.ext.oSort, {

            // "null-at-bottom-pre": function (_a) {
            //     _a = _a.replace(/\,/g,'');
            //     let _value = (_a == '' || _a == null || _a == '--' || _a == 'NA')? null: Number.parseFloat(_a);
            //     return _value;
            //     ////parseString(a);
            // },

            "null-at-bottom-asc": function(_a: any, _b: any) {

                _a = _a.replace(/\,/g,''); _b = _b.replace(/\,/g,'');

                const a = (_a == '' || _a == null || _a == '--'  || _a == 'NA')? null: Number.parseFloat(_a);
                const b = (_b == '' || _b == null || _b == '--' || _b == 'NA')? null: Number.parseFloat(_b);
                
                if (a === b)
                    return 0;
                if (a === null)
                    return 1;
                if (b === null)
                    return -1;
                return a < b ? -1 : 1;
            },

            "null-at-bottom-desc": function(_a: any, _b: any) {

                _a = _a.replace(/\,/g,''); _b = _b.replace(/\,/g,'');

                const a = (_a == '' || _a == null || _a == '--'  || _a == 'NA')? null: Number.parseFloat(_a);
                const b = (_b == '' || _b == null || _b == '--' || _b == 'NA')? null: Number.parseFloat(_b);

                if (a === b)
                    return 0;
                if (a === null)
                    return 1;
                if (b === null)
                    return -1;
                return a < b ? 1 : -1;
            }
        });
    }

}