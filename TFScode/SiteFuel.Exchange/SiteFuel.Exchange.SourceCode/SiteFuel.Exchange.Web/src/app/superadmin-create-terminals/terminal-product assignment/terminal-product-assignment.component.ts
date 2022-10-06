import { Component, Input, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { IDropdownSettings } from 'ng-multiselect-dropdown';
import { Subject } from 'rxjs';
import { DropDownItem } from '../../buyer-wally-board/Models/BuyerWallyBoard';
import { Declarations } from '../../declarations.module';
import { CreateterminalsService } from '../createterminals.service';
import { TerminalMappedProductsGridModel } from './../models'


@Component({
  selector: 'app-terminal-product-assignment',
  templateUrl: './terminal-product-assignment.component.html',
  styleUrls: ['./terminal-product-assignment.component.css']
})
export class TerminalProductAssignmentComponent implements OnInit, OnChanges{

    public IsLoading: boolean;
    @Input() SelectedCountryId: any;

    dtOptions: any = {};
    dtTrigger: Subject<any> = new Subject();

    public terminalMappingDetails: TerminalMappedProductsGridModel[] = [];



    public _opened: boolean = false;
    public _animate: boolean = true;
    public _positionNum: number = 1;
    public _POSITIONS: Array<string> = ['left', 'right', 'top', 'bottom'];
    public terminalMappingForm: FormGroup;
    public terminalMappingDetail: TerminalMappedProductsGridModel;
    public productList: any[];
    public multiselectSettingsById: IDropdownSettings;
    public Products: DropDownItem[];

    constructor(private terminalService: CreateterminalsService, private fb: FormBuilder) { }
    

    ngOnInit(): void {
        this.dtOptions = {
            dom: '<"html5buttons"B>lTfgitp',
            buttons: [               
            ],
            pagingType: 'first_last_numbers',
            pageLength: 10,
            ordering: true,
            order: [],
            lengthMenu: [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]]
        };

        this.multiselectSettingsById = {
            singleSelection: false,
            idField: "Id",
            textField: "Name",
            selectAllText: "Select All",
            unSelectAllText: "UnSelect All",
            itemsShowLimit: 5,
            allowSearchFilter: true
        };
        this.getAllProductsDDL();
        this.initializeTerminalMappingCreationForm(this.terminalMappingDetail);

    }

    ngOnChanges(changes: SimpleChanges): void {
        if (changes.SelectedCountryId && changes.SelectedCountryId.currentValue) {
            this.getTerminalMappingDetailsData()
        }
    }

    getTerminalMappingDetailsData() {
        let countryId = this.SelectedCountryId;
        this.IsLoading = true;
        this.terminalService.getTerminalProductMappingDetails(countryId).subscribe((data) => {
            if (data) {
                jQuery("#terminal-products-datatable").DataTable().clear().destroy();
                this.terminalMappingDetails = data;
                this.dtTrigger.next();
                this.IsLoading = false;
            }

        });
    }
    initializeTerminalMappingCreationForm(terminalMapping: TerminalMappedProductsGridModel) {
        if (terminalMapping == null || terminalMapping == undefined) {
            terminalMapping = new TerminalMappedProductsGridModel();
        }
        this.terminalMappingForm = this.fb.group({
            TerminalId: this.fb.control(terminalMapping.TerminalId),
            TerminalControlNumber: this.fb.control(terminalMapping.TerminalControlNumber),
            MappedProducts: this.fb.control(terminalMapping.MappedProducts, [Validators.required]),
            TerminalName: this.fb.control(terminalMapping.TerminalName)
        });
    }


    formatProductsForGridDisplay(assignedProducts:any[]) {
        let formattedString = "";
        if (assignedProducts != null && assignedProducts.length > 0) {
            let displayCount = assignedProducts.length - 3;
            if (assignedProducts.length > 3)
            {
                assignedProducts.forEach(function (product, index) {
                    if (index <= 2) {
                        if (product.Name) {
                            formattedString = index == 2 ? formattedString.concat(product.Name, "     ", "+" + displayCount+" other") : formattedString.concat(product.Name,", ");
                        }
                    }

                });
            }
            else 
            {
                assignedProducts.forEach(function (product, index) {
                        if (product.Name) {
                            formattedString =  index == assignedProducts.length-1? formattedString.concat(product.Name, " ") : formattedString.concat(product.Name, ", ");
                        }

                });
            }
        }
        return formattedString;
    }


    _toggleOpened(shouldOpen: boolean) {
        if (shouldOpen) {            
            this._opened = true;
        }
        else {
            this._opened = !this._opened;
            this.terminalMappingForm.reset();
        }
    }
    getAllProductsDDL() {
        this.IsLoading = true;
        this.terminalService.getAllProductsForMapping().subscribe((data) => {
            this.IsLoading = false;
            this.productList = data;
        });
    }

    editMapping(terminalMapping: TerminalMappedProductsGridModel) {
        if (terminalMapping != null && terminalMapping != undefined) {
            this.terminalMappingForm.get('TerminalId').setValue(terminalMapping.TerminalId);
            this.terminalMappingForm.get('TerminalControlNumber').setValue(terminalMapping.TerminalControlNumber);
            this.terminalMappingForm.get('MappedProducts').setValue(terminalMapping.MappedProducts);
            this.terminalMappingForm.get('TerminalName').setValue(terminalMapping.TerminalName);
        }
        
    }


    SubmitForm() {
        this.terminalMappingForm.markAllAsTouched();
        if (this.terminalMappingForm.valid) {
            let model = this.terminalMappingForm.value;
            if (model) {
                let input = new TerminalMappedProductsGridModel();
                input.TerminalId = model.TerminalId;
                input.TerminalControlNumber = model.TerminalControlNumber;
                input.MappedProducts = model.MappedProducts;
                input.TerminalName = model.TerminalName;
                this.IsLoading = true;
                this.terminalService.saveTerminalProductMapping(input).subscribe((data) => {
                    if (data.StatusCode == 0) {
                        Declarations.msgsuccess(data.StatusMessage, undefined, undefined);
                        this._toggleOpened(false);
                        this.getTerminalMappingDetailsData();
                    }
                    else if (data.StatusCode == 1) {
                        Declarations.msgerror(data.StatusMessage, undefined, undefined);
                    }
                    this.IsLoading = false;
                });
            }
            
        }
        
    }
}
