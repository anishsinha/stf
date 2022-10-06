import { Component, OnInit, Output, EventEmitter, Input, SimpleChanges } from '@angular/core';
import { FormArray, FormBuilder, Validators, FormGroup, Form, AbstractControl } from '@angular/forms';
import { map, tap, debounceTime, distinctUntilChanged, switchMap, catchError } from 'rxjs/operators';
import { Observable, of, from, iif } from 'rxjs';
import { ActivatedRoute } from '@angular/router';
import { Declarations } from 'src/app/declarations.module';

@Component({
  selector: 'app-bdr-detail',
  templateUrl: './bdr-detail.component.html',
  styleUrls: ['./bdr-detail.component.css']
})
export class BdrDetailComponent implements OnInit {
    public _opened: boolean = false;
    public _animate: boolean = true;
    public _loading: boolean = false;
    public _positionNum: number = 1;
    public _POSITIONS: Array<string> = ['left', 'right', 'top', 'bottom'];
    public EditIndex: number = -1;

    public BdrDetailForm: FormGroup;

    @Output() onBdrDetailsAdded: EventEmitter<any> = new EventEmitter<any>();
    @Output() onBdrDetailsUpdated: EventEmitter<any> = new EventEmitter<any>();

    public IsEngineerInvitedToWitnessSample: boolean;
    public IsNoticeToProtestIssued: boolean;

    constructor(private fb: FormBuilder, private route: ActivatedRoute) { }

    ngOnInit() {
        this.BdrDetailForm = this.buildForm();
    }

    ngOnChanges(change: SimpleChanges) {
        
    }

    toggleOpen(shouldOpen: boolean, event: any) {
        this._opened = shouldOpen;
        if (shouldOpen) {
            if (event != null) {
                this.EditIndex = event.index;
                this.BdrDetailForm.patchValue(event.bdrDetails);
            }
        } else {
            this.BdrDetailForm.reset();
            this.EditIndex = -1;
        }
    }

    onBdrDetailAdd() {
        this.BdrDetailForm.markAllAsTouched();
        if (this.BdrDetailForm.valid) {
            this.onBdrDetailsAdded.emit(this.BdrDetailForm.value);
            this.toggleOpen(false, null);
        }
        else {
            this.BdrDetailForm.markAllAsTouched();
        }
    }

    onBdrDetailUpdate() {
        this.BdrDetailForm.markAllAsTouched();
        if (this.BdrDetailForm.valid) {
            var eventData = {
                bdrDetail: this.BdrDetailForm.value,
                index: this.EditIndex
            };
            this.onBdrDetailsUpdated.emit(eventData);
            this.toggleOpen(false, null);
            this.EditIndex = -1;
        }
        else {
            this.BdrDetailForm.markAllAsTouched();
        }
    }

    onBdrCancel() {
        this.BdrDetailForm.reset();
        this.toggleOpen(false, null);
    }

    isInvalid(name: string): boolean {
        var result = this.BdrDetailForm.get(name).invalid
            &&
            (
            this.BdrDetailForm.get(name).dirty
                ||
            this.BdrDetailForm.get(name).touched
            )
        return result;
    }

    isRequired(name: string): boolean {
        return this.BdrDetailForm.get(name).errors.required;
    }

    buildForm() {
        return this.fb.group({
            Id: this.fb.control(''),
            PumpingStartTime: this.fb.control('', [Validators.pattern(/(?:[0][1-9]|[1][0-2]):(?:[0-5]\d):(?:[0-5]\d) ?([AaPp][Mm])/)]),
            PumpingStopTime: this.fb.control('', [Validators.pattern(/(?:[0][1-9]|[1][0-2]):(?:[0-5]\d):(?:[0-5]\d) ?([AaPp][Mm])/)]),
            OpenMeterReading: this.fb.control(''),
            CloseMeterReading: this.fb.control(''),
            MarpolSampleNumbers: this.fb.control(''),
            MVMarpolSampleNumbers: this.fb.control(''),
            Viscosity: this.fb.control('',[Validators.min(0.00001)]),
            SulphurContent: this.fb.control('',[Validators.min(0.00001)]),
            FlashPoint: this.fb.control('',[Validators.min(0.00001)]),
            DensityInVaccum: this.fb.control('', [Validators.min(0.00001)]),
            ObservedTemperature: this.fb.control('',[Validators.min(0.00001)]),
            MeasuredVolume: this.fb.control(''),
            StandardVolume: this.fb.control(''),
            IsEngineerInvitedToWitnessSample: this.fb.control(''),
            IsNoticeToProtestIssued: this.fb.control(''),
        });
    }

    setStandardVolume() {
        let measuredVolume = this.BdrDetailForm.get('MeasuredVolume').value;
        this.BdrDetailForm.get('StandardVolume').setValue(measuredVolume);
    }
}
