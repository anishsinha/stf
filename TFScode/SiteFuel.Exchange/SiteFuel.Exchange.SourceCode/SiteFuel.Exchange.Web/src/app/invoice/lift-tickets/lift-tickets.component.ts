import { Component, OnInit, Input, EventEmitter, Output, ViewChildren, QueryList, OnChanges, SimpleChanges } from '@angular/core';
import { FormGroup, FormArray, FormBuilder } from '@angular/forms';
import { LiftTicketDetail, LiftProductModel, AddressModel } from '../models/DropDetail';
import { AddressService } from 'src/app/address.service';
import { ImageuploadComponent } from 'src/app/imageupload/imageupload.component';

@Component({
	selector: 'app-lift-tickets',
	templateUrl: './lift-tickets.component.html',
	styleUrls: ['./lift-tickets.component.css']
})
export class LiftTicketsComponent implements OnInit, OnChanges {

	@Input() public invoiceForm: FormGroup;
	@Input() public TicketDetails: FormArray;
	@Input() public Model: LiftTicketDetail[];
	@Output() public onTicketEditRequest: EventEmitter<any> = new EventEmitter<any>();
    @ViewChildren(ImageuploadComponent) children: QueryList<ImageuploadComponent>;

    //Liftticket number Duplicate validation methods 
    @Output() public onLiftTicketAdded: EventEmitter<any> = new EventEmitter<any>();
    @Output() public OnLiftTicketDeleted: EventEmitter<any> = new EventEmitter<any>();
    @Output() public onLiftTicketEdit: EventEmitter<any> = new EventEmitter<any>()
    @Output() public OnLiftQuantitiesAdded: EventEmitter<any> = new EventEmitter<any>();

    @Input() public IsImageRequired: boolean;
    public IsBolImageRequired: boolean = false;

	constructor(private fb: FormBuilder, private addressService: AddressService) { }

	ngOnInit() {

	}

	ngOnChanges(change: SimpleChanges) {
		if (change.Model && change.Model.currentValue != null) {
			var lifts = change.Model.currentValue as LiftTicketDetail[];
			lifts.forEach(x => this.ticketDetailAdded(x));
        }
        if (change.IsImageRequired && change.IsImageRequired.currentValue != null) {
            this.IsBolImageRequired = change.IsImageRequired.currentValue;
        }
    }


	buildForm(model: LiftTicketDetail): FormGroup {
		var products = this.fb.array([]);
		var currentObj = this;
		model.Products.forEach(function (elem, idx) {
			products.push(currentObj.buildProduct(elem));
		});

		return this.fb.group({
			Id: this.fb.control(model.Id),
			LiftTicketNumber: this.fb.control(model.LiftTicketNumber),
			LiftDate: this.fb.control(model.LiftDate),
			Products: products,
            LiftImages: this.fb.control(''),
            BadgeNumber: this.fb.control(model.BadgeNumber),
            LiftStartTime: this.fb.control(model.LiftStartTime),
            LiftEndTime: this.fb.control(model.LiftEndTime),
            CommonBulkPlantId: this.fb.control(model.Products ? model.Products[0].BulkPlantId : model.CommonBulkPlantId),
            CommonBulkPlantName: this.fb.control(model.Products ? model.Products[0].BulkPlantName : model.CommonBulkPlantName),
            CommonAddress: this.buildAddress(model.Products? model.Products[0].Address:new AddressModel()),
		});
	}

    buildProduct(model: LiftProductModel): FormGroup {
        return this.fb.group({
            ProductId: this.fb.control(model.ProductId),
            ProductName: this.fb.control(model.ProductName),
            NetQuantity: this.fb.control(model.NetQuantity),
            DeliveredQuantity: this.fb.control(model.DeliveredQuantity),
            GrossQuantity: this.fb.control(model.GrossQuantity),
            BulkPlantId: this.fb.control(model.BulkPlantId),
            BulkPlantName: this.fb.control(model.BulkPlantName),
            Address: this.buildAddress(model.Address),
            QuantityIndicatorTypeId: this.fb.control(model.QuantityIndicatorTypeId)
        });
    }

    buildAddress(model: AddressModel): FormGroup {
        var product = this.fb.group({
            Address: this.fb.control(''),
            Latitude: this.fb.control(''),
            Longitude: this.fb.control(''),
            City: this.fb.control(''),
            CountyName: this.fb.control(''),
            State: this.fb.group({
                Id: this.fb.control(''),
                Code: this.fb.control('')
            }),
            Country: this.fb.group({
                Id: this.fb.control(''),
                Code: this.fb.control('')
            }),
            ZipCode: this.fb.control(''),
        });
        product.patchValue(model);
        return product;
    }

    ticketDetailAdded(item: LiftTicketDetail): void {
        if (item.LiftDate && item.LiftDate.indexOf('/Date(') >= 0) {
            item.LiftDate = item.DisplayLiftDate || '';
        }
        this.TicketDetails.push(this.buildForm(item));
        this.onLiftTicketAdded.emit(this.TicketDetails.value);
        this.OnLiftQuantitiesAdded.emit();
    }

    ticketDetailUpdated(item: any): void {
        var formG = this.TicketDetails.controls[item.index] as FormGroup;
        formG.patchValue(item.ticketDetail);
        this.OnLiftQuantitiesAdded.emit();
    }

    updateBulkPlantDetail(event: any) {
        var currentObj = this; var updated = false;
        currentObj.TicketDetails.controls.forEach(function (elem: FormGroup, idx) {
            var _products = elem.get('Products') as FormArray;
            var _matched = _products.controls.find(function (elem: FormGroup) {
                return elem.get('ProductId').value == event.ProductId;
            });
            if (_matched != undefined) {
                (_matched as FormGroup).patchValue(event);
                updated = true;
            }
        });
        if (!updated) {
            var _lift = new LiftTicketDetail();
            _lift.Products = [event];
            currentObj.ticketDetailAdded(_lift);
        }
    }

    editTicketDetail(ticketDetail: FormGroup, i: number): any {
        this.onTicketEditRequest.emit({ ticketDetail: ticketDetail.value, index: i });
        this.onLiftTicketEdit.emit(ticketDetail.value);
    }

    deleteTicketDetail(i: number): void {
        this.TicketDetails.removeAt(i);
        this.OnLiftTicketDeleted.emit(this.TicketDetails.value);
        this.OnLiftQuantitiesAdded.emit();
    }
}
