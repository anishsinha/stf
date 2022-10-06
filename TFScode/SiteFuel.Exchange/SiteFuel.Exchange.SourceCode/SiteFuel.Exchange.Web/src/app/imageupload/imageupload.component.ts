import { Component, OnInit, Input, EventEmitter, Output, forwardRef, OnChanges, SimpleChanges } from '@angular/core';
import { FormGroup, FormBuilder, NG_VALUE_ACCESSOR } from '@angular/forms';
import { ImageserviceService } from '../invoice/services/imageservice.service';
import { InvoiceService } from '../invoice/services/invoice.service';
import { IsValidFileSize } from 'src/app/app.constants';
import { Declarations } from '../declarations.module';


@Component({
	selector: 'app-imageupload',
	templateUrl: './imageupload.component.html',
	styleUrls: ['./imageupload.component.css'],
	providers: [
		{
			provide: NG_VALUE_ACCESSOR,
			useExisting: forwardRef(() => ImageuploadComponent),
			multi: true
		}
	]
})

export class ImageuploadComponent implements OnInit, OnChanges {

	public files = new Array<FileInfo>();
	public sas: string;
	public funcs = [];
	public imageForm: FormGroup;
	@Input() invoice: boolean;
	@Input() additional: boolean;
	@Input() signature: boolean;
	@Input() isBol: boolean;
	@Input() isLift: boolean;
	@Input() fileType: string;
	@Input() orderId: string;
	@Input() completeInvoiceViewModel: any;
	@Input() inputFile = new Array<FileInfo>();
	@Input() TaxAffidavit: boolean;
	@Input() BDNImage : boolean;
	@Input() CoastGuardInspection: boolean;
	@Input() InspectionRequestVoucher: boolean;
	public ContainerName = "invoicepdffiles";
	@Output() onImageUpload: EventEmitter<any> = new EventEmitter<any>();
	public currentInstance: any;
	public uploadFunc: any;
	public FileUrls: string;

	constructor(private imageservice: ImageserviceService, private fb: FormBuilder, private invoiceservice: InvoiceService) { }

	ngOnInit() {
		this.imageForm = this.fb.group({
			Image: this.fb.control(''), // actural image content 
			FileUrl: this.fb.control('') // hidden form control to set the value for file url received
		});
		this.currentInstance = this;
		this.uploadFunc = this.UploadFile;
		this.FileUrls = "";
	}

	ngOnChanges(change: SimpleChanges) {
		if (change.inputFile && change.inputFile.currentValue != null) {
			this.files = (change.inputFile.currentValue as FileInfo[]);
		}
	}

	onImageSelect(event) {
		this.files = new Array<FileInfo>();
		if (event.target.files && event.target.files[0]) {
			var fileNumber = event.target.files.length;		
			//validate file size for each file(jpg/pdf both). Continue only if all files are of valid size.
			for (let i = 0; i < fileNumber; i++) {
				var fileSize = event.target.files[i].size;
				var currentFileName = event.target.files[i].name;
				if (!IsValidFileSize(fileSize)) {
					Declarations.msgerror("File Size excceds 5 MB for file " + currentFileName, undefined, undefined);
					this.imageForm.reset();

					return false;
                }
            }
			for (let i = 0; i < fileNumber; i++) {
				var currentFileName = event.target.files[i].name;
				var fileSize = event.target.files[i].size;
					var extension = currentFileName.split('.').pop();
					var image = event.target.files[i];
					var reader = new FileReader();
					reader["currentFileName"] = currentFileName;
					reader["image"] = image;
					this.funcs[i] = this.createOnloadFunction(currentFileName, image);
					reader.onload = (event2: any) => this.funcs[i](event2);
					reader.readAsDataURL(event.target.files[i]);
			}
		}
	}

	//ngOnChanges(change: SimpleChanges) {
	//    if (change.orderId && change.orderId.currentValue != null) {

	//    }
	//}

	public createOnloadFunction(currentFileName, image) {
		var self = this;
		return function (event2) {
			var file = new FileInfo();
			file.Name = currentFileName;
			file.Image = image;
			file.Extension = file.Name.split('.').pop();
			file.IsNewFile = true;
			if (file.Extension.toLowerCase() == 'pdf') {
				file.IsPdf = true;
			}
			else {
				file.IsPdf = false;
			}
			file.AzurePath = event2.target.result;
			self.files.push(file);
		};
	}

	public GetSasForBlob() {
		return this.invoiceservice.getSASforblobStorage().toPromise().then(x => {
			this.sas = x
			return x;
		});
	}

	public UploadFile(entireInvoiceModel, sas) {
		if (this.fileType == "BDN" || this.fileType == "TaxAffidavit" || this.fileType == "CoastGuardInspection" || this.fileType == "InspectionRequestVoucher") {
			//this.ContainerName = "bdn";
			var mnth=new Date().getMonth() + 1;
			this.ContainerName = "bdn" + "-" + mnth + "-" + new Date().getFullYear();
		}
		else
			this.ContainerName = "invoicepdffiles"
		//else
		//this.ContainerName=this.ContainerName+"_"+new Date().getMonth()+"_"+new Date().getFullYear();
		var self = this;
		return self.imageservice.uploadImageToBlob(sas, this.files, this.files, this.fileType, this.orderId, entireInvoiceModel, this.ContainerName);
	}


	// control value accessors 
	public onTouched: () => void = () => { };

	writeValue(val: any): void {
		val && this.imageForm.setValue(val, { emitEvent: true });
	}
	registerOnChange(fn: any): void {
		this.imageForm.valueChanges.subscribe(fn);
	}
	registerOnTouched(fn: any): void {
		this.onTouched = fn;
	}
	setDisabledState?(isDisabled: boolean): void {
		isDisabled ? this.imageForm.disable() : this.imageForm.enable();
	}
}

export class FileInfo {
	public Name: string;
	public Extension: string;
	public Image: any;
	public IsPdf: boolean;
	public AzurePath: string;
	public FileUrl: string = "";
	public IsNewFile: boolean;
	public url: string;
	public FilePath: string;
}


