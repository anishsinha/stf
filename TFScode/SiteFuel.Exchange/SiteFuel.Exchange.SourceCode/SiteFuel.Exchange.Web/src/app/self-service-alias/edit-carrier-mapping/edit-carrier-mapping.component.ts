import { Component, OnInit, Output, Input, EventEmitter } from '@angular/core';
import { FormBuilder, FormGroup, Validators, AbstractControl } from '@angular/forms';
import { CarrierDetailsViewModel } from 'src/app/carrier/models/CarrierDetailsViewModel';
import { CarrierService } from 'src/app/carrier/service/carrier.service';
import { Declarations } from 'src/app/declarations.module';
declare function closeSlidePanel(): any;

@Component({
  selector: 'app-edit-carrier-mapping',
  templateUrl: './edit-carrier-mapping.component.html',
  styleUrls: ['./edit-carrier-mapping.component.css']
})
export class EditCarrierMappingComponent implements OnInit {

  //  public CarrierForm: FormGroup;
  //  public isShowLoader: boolean = false;
  //  @Input() CarrierDataToBeSend: CarrierDetailsViewModel;
  //  @Output() getCarrierData: EventEmitter<any> = new EventEmitter();

  //  constructor(public fb: FormBuilder, private carrierService: CarrierService) {
  //      this.CarrierForm = this.fb.group({
  //          Id: this.fb.control(0),          
  //          CarrierCompanyId: this.fb.control(''),
  //          CarrierName: this.fb.control(''),
  //          TotalOrders: this.fb.control(''),
  //          AssignedCarrierId: this.fb.control(''),
  //          CompanyId: this.fb.control(''),
  //      });
  //  }

  ngOnInit() {
  }

  //  checkDuplicateCarrierId(carrierDetail) {
  //      this.carrierService.checkDuplicateCarrierId(carrierDetail).subscribe(data => {
  //          if (data.StatusCode == 0) {
  //              this.submitForm(carrierDetail);
  //          }

  //          if (data.StatusCode == 2) {
  //              Declarations.msgerror(data.StatusMessage, undefined, undefined);
  //          }
  //      });
  //  }

  //  onSubmit() {
  //      var CarrierDetail =
  //      {
  //          CarrierCompanyId: this.CarrierForm.get("CarrierCompanyId").value,
  //          CarrierName: this.CarrierForm.get("CarrierName").value,
  //          AssignedCarrierId: this.CarrierForm.get("AssignedCarrierId").value,
  //          Id: this.CarrierForm.get("Id").value,
  //          CompanyId: this.CarrierForm.get("CompanyId").value
  //      }
  //      this.checkDuplicateCarrierId(CarrierDetail);
  //  }

  //  submitForm(CarrierDetail) {
  //      this.isShowLoader = true;
  //      this.carrierService.saveAndUpdateCarrierMapping(CarrierDetail).subscribe(data => {
  //          this.isShowLoader = false;
  //          if (data.StatusCode == 0) {
  //              Declarations.msgsuccess(data.StatusMessage, undefined, undefined);
  //              closeSlidePanel();
  //              this.clearForm();
  //          }
  //          else if (data.StatusCode == 2) {
  //              Declarations.msgwarning(data.StatusMessage, undefined, undefined);
  //          }
  //          else {
  //              Declarations.msgerror(data == null ? 'Failed' : data.StatusMessage, undefined, undefined);
  //          }
  //      });
  //  }

  //  clearForm() {
  //      this.CarrierForm.reset();
  //      $("#carrier-grid-datatable").DataTable().clear().destroy();
  //      this.getCarrierData.emit();
  //  }
}
