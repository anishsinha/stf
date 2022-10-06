import { Component, OnInit, Output, EventEmitter, Input, SimpleChanges, OnChanges } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { BDRModel } from '../models/DropDetail';

@Component({
    selector: 'app-show-bdr-detail',
    templateUrl: './show-bdr-detail.component.html',
    styleUrls: ['./show-bdr-detail.component.css']
})
export class ShowBdrDetailComponent implements OnInit, OnChanges{

    @Input() public Parent: FormGroup; // drop formgroup and not top level invoice form as we want BDR per drop
    @Input() public BdrDetails: FormGroup;
    @Input() public BdrModel: BDRModel;

    public IsBDRAdded: boolean = false;

    @Output() public OnBDREdit: EventEmitter<any> = new EventEmitter<any>();
    @Output() public OnBDRDelete: EventEmitter<any> = new EventEmitter<any>();
   

  constructor() { }

  ngOnInit() {
    }
    ngOnChanges(change: SimpleChanges) {
        if (change.BdrModel && change.BdrModel.currentValue) {            
            var bdrDetails = change.BdrModel.currentValue;
            if (bdrDetails) {
                this.buildForm(bdrDetails, this.BdrDetails);
            }
        }
    }

    editBdrDetail(event: FormGroup) {
        this.OnBDREdit.emit({ bdrDetails: event.value, index: 1 });
    }

    deleteBdr(bdrDetails: FormGroup) {
        Object.keys(bdrDetails.controls).forEach(field => {
            bdrDetails.removeControl(field);
        });
        this.IsBDRAdded = false;
        this.OnBDRDelete.emit();
    }
    buildForm(model: BDRModel, bdrDetailsFormGroup: FormGroup) {
        if (model != null && model != undefined) {
            for (const property in model) {
                bdrDetailsFormGroup.addControl(property, new FormControl(model[property]));

            }
            this.IsBDRAdded = true;
        } else { this.IsBDRAdded = false; }
    }

    bdrDetailAdded(eventData: BDRModel) {
        this.buildForm(eventData, this.BdrDetails);
    }

    bdrDetailUpdated(event: any) {      
        this.BdrDetails.patchValue(event.bdrDetail);
    }

}
