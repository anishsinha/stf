import { Injectable } from '@angular/core';
import { HandleError } from 'src/app/errors/HandleError';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { catchError } from 'rxjs/operators';


@Injectable({
	providedIn: 'root'
})
export class OrderService extends HandleError {
	private getCustomerListUrl = '/Supplier/OrderGroup/GetCustomersForSupplier';
	private getJobListUrl = '/Supplier/OrderGroup/GetJobsForCustomer?buyerCompanyId=';

	private getCommonJobListUrl = '/OrderBase/GetJobsForCustomers?companyId=';
	private postCreateGroupUrl: string = '/OrderBase/CreateOrderGroup';
	private postEditGroupUrl: string = '/OrderBase/EditOrderGroup';
	private getFilteredOrders = '/OrderBase/GetFilteredOrders';
	private getFuelTypes = '/OrderBase/GetFuelTypes?';
	private getCurrentUserUrl = '/OrderBase/GetUserContext';
	private getSupplierListUrl = '/Buyer/OrderGroup/GetSuppliersForCustomer';
	private getGroupDetailsUrl = '/OrderBase/GetOrderGroupDetails?groupId=';
	private getBlendedGroupDetailstUrl = '/OrderBase/GetBlendedGroupDetails?groupId=';

	constructor(private httpClient: HttpClient) { super(); }

	getCustomerList(): Observable<any> {
		return this.httpClient.get(this.getCustomerListUrl).pipe(catchError(this.handleError<any>('getCustomerList', null)));
	}

	getBlendGroupDetails(groupId: number): Observable<any> {
		return this.httpClient.get(this.getBlendedGroupDetailstUrl + groupId).pipe(catchError(this.handleError<any>('getBlendGroupDetails', null)));
	}

	getSupplierList(): Observable<any> {
		return this.httpClient.get(this.getSupplierListUrl).pipe(catchError(this.handleError<any>('getSupplierList', null)));
	}

	getJobList(customerId: number): Observable<any> {
		return this.httpClient.get(this.getJobListUrl + customerId).pipe(catchError(this.handleError<any>('getJobList', null)));
	}

	getCommonJobList(customerId: number): Observable<any> {
        return this.httpClient.get(this.getCommonJobListUrl + customerId).pipe(catchError(this.handleError<any>('getCommonJobList', null)));
	}
	getFilteredOrdersList(customerId: number, jobId: number, tfxFuelTypeIds: number[], groupId: number): Observable<any> {
		return this.httpClient.post(this.getFilteredOrders, { customerId: customerId, jobId: jobId, tfxFuelTypeIds: tfxFuelTypeIds, groupId: groupId }).pipe(catchError(this.handleError<any>('getFilteredOrdersList', null)));
	}

	getJobListByFuelGroupType(customerId: number, supplierId: number, fuelGroupType: number): Observable<any> {
		return this.httpClient.get(this.getJobListByFuelGroupUrl(customerId, supplierId, fuelGroupType)).pipe(catchError(this.handleError<any>('getJobListByFuelGroupType', null)));
	}

	getOrderList(buyerCompanyId: number, supplierCompanyId: number, fuelGroupType: number, jobId: number): Observable<any> {
		return this.httpClient.get(this.getOrderListUrl(buyerCompanyId, supplierCompanyId, fuelGroupType, jobId)).pipe(catchError(this.handleError<any>('getOrderList', null)));
	}

	getCurrentUser(): Observable<any> {
		return this.httpClient.get(this.getCurrentUserUrl).pipe(catchError(this.handleError<any>('getCurrentUser', null)));
	}

	getGroupDetails(groupId: number): Observable<any> {
		return this.httpClient.get(this.getGroupDetailsUrl + groupId).pipe(catchError(this.handleError<any>('getGroupDetails', null)));
	}

	getOrderListUrl(a: number, b: number, c: number, d: number) {
		return `/OrderBase/GetOrdersForTierGroup?buyerCompanyId=${a}&supplierCompanyId=${b}&fuelGroupType=${c}&jobId=${d}`;
	}

	getFuelTypesList(customerId: number, jobId: number): Observable<any> {
		return this.httpClient.get(this.getFuelTypes + "customerId=" + customerId + "&jobId=" + jobId).pipe(catchError(this.handleError<any>('getFuelTypesList', null)));
	}

	getJobListByFuelGroupUrl(a: number, b: number, c: number) {
		return `/OrderBase/GetJobsByFuelGroup?buyerCompanyId=${a}&supplierCompanyId=${b}&fuelGroupType=${c}`;
	}

	postCreateGroup(groupModel: any): Observable<any> {
		return this.httpClient.post<any>(this.postCreateGroupUrl, groupModel)
			.pipe(catchError(this.handleError<any>('postCreateGroup', null)));
	}

	postEditGroup(groupModel: any): Observable<any> {
		return this.httpClient.post<any>(this.postEditGroupUrl, groupModel)
			.pipe(catchError(this.handleError<any>('postEditGroup', null)));
	}
}
