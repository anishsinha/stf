import { Injectable } from '@angular/core';
import { HandleError } from 'src/app/errors/HandleError';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';

const httpOptions = {
	headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};
@Injectable({
	providedIn: 'root'
})
export class AssigncarrierService extends HandleError {

	private carrierUrl = '/Settings/Profile/GetCarriers';
	private carrierUsersUrl = '/Settings/Profile/GetAssignedCarriers';
	private jobsUrl = '/Settings/Profile/GetJobsForSupplierToCarrier';
	private assignedCarriersUrl = '/Settings/Profile/GetAssignedCarriersForSupplier';
	private createUrl = '/Settings/Profile/AssignCarriers';
	private updateUrl = '/Settings/Profile/UpdateAssignedCarrier';
	private deleteUrl = '/Settings/Profile/DeleteAssignedCarrier';
    private createFreightOrderUrl = '/Carrier/Order/createFreightOrdersForAssignedCarrier';
    private editFreightOnlyOrderUrl = '/Carrier/Order/EditFreightOnlyOrders';
    private closeFreightOnlyOrderUrl = '/Carrier/Order/closeAssignedOrdersforCarrier';
    private GetCarrierUserEmailsUrl = '/Supplier/Order/GetCarrierUserEmails';
    private BulkUploadCarrierUrl = '/Settings/Profile/BulkUploadCarrier';
	constructor(private httpClient: HttpClient) {
		super();
	}

	GetCarrierUserEmails(assignedCarrierCompanyId: number): Observable<DropdownItem[]> {
		return this.httpClient.get<DropdownItem[]>(this.GetCarrierUserEmailsUrl +'?assignedCarrierCompanyId='+assignedCarrierCompanyId)
			.pipe(catchError(this.handleError<DropdownItem[]>('GetCarrierUserEmails', [])));
	}

	getCarriers(): Observable<DropdownItem[]> {
		return this.httpClient.get<DropdownItem[]>(this.carrierUrl)
			.pipe(catchError(this.handleError<DropdownItem[]>('getCarriers', [])));
	}
	getAssignedCarrierUsers(): Observable<CarrierJobDetails[]> {
		return this.httpClient.get<CarrierJobDetails[]>(this.carrierUsersUrl)
			.pipe(catchError(this.handleError<CarrierJobDetails[]>('getAssignedCarrierUsers', [])));
	}
	getJobs(): Observable<CarrierJob[]> {
		return this.httpClient.get<CarrierJob[]>(this.jobsUrl)
			.pipe(catchError(this.handleError<CarrierJob[]>('getJobs', [])));
	}

	getAssignedCarriers(): Observable<Carrier[]> {
		return this.httpClient.get<Carrier[]>(this.assignedCarriersUrl)
			.pipe(catchError(this.handleError<Carrier[]>('getAssignedCarriers', [])));
	}

	assignCarriers(carriers: Carrier[]): Observable<any> {
		return this.httpClient.post<any>(this.createUrl, carriers, httpOptions)
			.pipe(catchError(this.handleError<any>('assignCarriers', carriers)));
	}

	updateAssignedCarrier(carrier: Carrier): Observable<any> {
		return this.httpClient.post<any>(this.updateUrl, carrier, httpOptions)
			.pipe(catchError(this.handleError<any>('updateAssignedCarrier', carrier)));
	}

	deleteAssignedCarrier(carrier: Carrier): Observable<any> {
		return this.httpClient.post<any>(this.deleteUrl, carrier, httpOptions)
			.pipe(catchError(this.handleError<any>('deleteAssignedCarrier', carrier)));
    }
    createFreightOrder(carriers: Carrier[]): Observable<any> {
        return this.httpClient.post<any>(this.createFreightOrderUrl, carriers)
            .pipe(catchError(this.handleError<any>('createFreightOrder', carriers)))
    }
    editFreightOnlyOrders(JobIdsToEdit: EditFreightOnlyOrder): Observable<any> {
        return this.httpClient.post<any>(this.editFreightOnlyOrderUrl, JobIdsToEdit)
            .pipe(catchError(this.handleError<any>('editFreightOnlyOrders', JobIdsToEdit)))
    }
    closeAssignedOrdersforCarrier(EditFreightOnlyOrder: EditFreightOnlyOrder): Observable<any>{
        return this.httpClient.post<any>(this.closeFreightOnlyOrderUrl, EditFreightOnlyOrder)
            .pipe(catchError(this.handleError<any>('closeAssignedOrdersforCarrier', EditFreightOnlyOrder)))
    }
    upload(file, IsCreateFreightOrder): Observable<any> {
        const formData = new FormData();
        formData.append("file", file, file.name);
        formData.append("IsCreateFreightOrder", IsCreateFreightOrder);
        return this.httpClient.post(this.BulkUploadCarrierUrl, formData)
    } 
}

export class DropdownItem {
	Id: number;
	Name: string;
	Code: string;
	IsSelected: boolean;
}
export class JobWithEmails {
	Id: number;
	Name: string;
	Code: string;
	Emails: DropdownItem[];
    IsSelected: boolean;
    IsEmailEdit: boolean;
}
export class DriverModel {
    TfxId: number;
    TfxName: string;
}
export class Carrier {
	public Id: string;
	public Carrier: DropdownItem;
    public Jobs: CarrierJob[];
    public assignedLocations: string;
}

export class CarrierJob {
	public Job: JobWithEmails;
	public BuyerCompanyId: number;
	public BuyerCompanyName: string;
}

export class EditFreightOnlyOrder {
    public removedJobsIds: number[];
    public newJobsIds: number[];
    public IsCreateOrder: boolean;
    public CarrierCompanyId: number;
}

export class CarrierJobDetails{
	public CarrierCompanyName: string;
	public CarrierCompanyId: number;
	public LocationCount: number;
	public AssignedLocations: string;
}
