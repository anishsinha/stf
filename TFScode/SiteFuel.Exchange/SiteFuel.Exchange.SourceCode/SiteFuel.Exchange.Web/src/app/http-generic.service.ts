import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { HandleError } from 'src/app/errors/HandleError';
import { catchError } from 'rxjs/operators';
@Injectable({
    providedIn: 'root'
})
export class HttpGenericService extends HandleError {


    constructor(private http: HttpClient) {
        super();
    }

    fetchAll(resourceName: string) {
        return this.http.get<any[]>(resourceName)
            .pipe(catchError(this.handleError<any>(resourceName, null)));
    }

    fetchSingle(resourceName: string) {
        return this.http.get<any>(resourceName)
            .pipe(catchError(this.handleError<any>(resourceName, null)));
    }

    postData(resourceName: string, data: any) {
        // data = JSON.stringify(data);
        return this.http.post(resourceName, data)
            .pipe(catchError(this.handleError<any>(resourceName, null)));
    }

    updateData(resourceName: string, data: any) {
        data = JSON.stringify(data);
        return this.http.put(resourceName, data)
            .pipe(catchError(this.handleError<any>(resourceName, null)));
    }

    private objectToQueryString(obj: any): string {
        var str = [];
        for (var p in obj)
            if (obj.hasOwnProperty(p)) {
                str.push(encodeURIComponent(p) + "=" + encodeURIComponent(obj[p]));
            }
        return str.join("&");
    }
}
