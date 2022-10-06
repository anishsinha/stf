import { Observable, of } from 'rxjs';
import { loginURL } from '../app.constants';

export class HandleError {
    /**
     * Handle Http operation that failed.
     * Let the app continue.
     * @param operation - name of the operation that failed
     * @param result - optional value to return as the observable result
     */
    public handleError<T>(operation = 'operation', result?: T) {
        return (error: any): Observable<T> => {

            // TODO: send the error to remote logging infrastructure
            console.error(error); // log to console instead

            // TODO: better job of transforming error for user consumption
            console.log(`${operation} failed: ${error.message}`);

            if (error && error.url && error.url.includes(loginURL)) { //&& error.message && error.message.includes(loginURL) 
                window.location.href = loginURL
            }

            // Let the app keep running by returning an empty result.
            return of(result as T);
        };
    }
}