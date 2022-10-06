import { Component, OnInit, Input, SimpleChanges, Output, EventEmitter } from '@angular/core';
import { FormGroup } from '@angular/forms';

@Component({
  selector: 'app-tank-view-master',
  templateUrl: './tank-view-master.component.html',
  styleUrls: ['./tank-view-master.component.css']
})
export class TankViewMasterComponent implements OnInit {

    @Input() salesTabFilterForm: FormGroup;

    constructor() { }

    ngOnInit() {
    }
}
