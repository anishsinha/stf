import { Injectable } from '@angular/core';
import { BlobServiceClient, AnonymousCredential, BlockBlobClient } from '@azure/storage-blob';
import * as moment from 'moment';
import { InvoiceDetailModel, ImageModel, BolDetail } from '../models/DropDetail';
import { FileInfo } from 'src/app/imageupload/imageupload.component';


@Injectable({
    providedIn: 'root'
})
export class ImageserviceService {

    constructor() { }
    public content: any;
    public imageurl: string;



    public uploadImageToBlob(sasToken: string, files: FileInfo[], image: any, fileType: string, prefixId: string, entireInvoiceModel: InvoiceDetailModel, containerName: string) :Promise<any> {
        var fileUrls = "";
        const anonymouscredentials = new AnonymousCredential();
        var fileNames = [];
        var promises = [];
        for (var i = 0; i < files.length; i++) {
            
            var file = files[i];
            if (file.IsNewFile) {
                var ftype=fileType;
                var fileName = fileType + "_" + prefixId + "_" + this.GenerateRandomNumber(10000) + moment(moment.now()).format("MMDDYYhhmmss.") + file.Extension;
                file.Name = fileName;
                fileNames.push(fileName)
                // Container name is also going as folder inside container
               // file.url =  fileName;
                file.url = containerName + "/" + fileName;
                if (fileUrls == "") {
                    fileUrls = file.url;
                }
                else
                    fileUrls = fileUrls + "||" + file.url;
            }
            else {
                if (fileUrls == "") {
                    fileUrls = file.FilePath;
                }
                else {
                    fileUrls = fileUrls + file.FilePath;
                }
            }

        }

        if (fileUrls != "") {
            if (fileType == "InvFile") {
                entireInvoiceModel.InvoiceImage = new ImageModel();
                entireInvoiceModel.InvoiceImage.FilePath = fileUrls;
            }
            if (fileType == "additionalFile") {
                entireInvoiceModel.AdditionalImage = new ImageModel();
                entireInvoiceModel.AdditionalImage.FilePath = fileUrls;
            }
            if (fileType == "bolFile") {
                var bolDetail = entireInvoiceModel.BolDetails.find(x => x.BolNumber == prefixId);
                if (bolDetail != undefined) {
                    bolDetail.Images = new ImageModel();
                    bolDetail.Images.FilePath = fileUrls;
                }
            }

            if (fileType == "liftFile") {
                var liftDetail = entireInvoiceModel.TicketDetails.find(x => x.LiftTicketNumber == prefixId);
                if (liftDetail != undefined) {
                    liftDetail.Images = new ImageModel();
                    liftDetail.Images.FilePath = fileUrls;
                }
            }

            if (fileType == "signatureFile") {
                entireInvoiceModel.SignatureImage = new ImageModel();
                entireInvoiceModel.SignatureImage.FilePath = fileUrls;
            }
            if (fileType == "TaxAffidavit") {
                entireInvoiceModel.TaxAffidavitImage = new ImageModel();
                entireInvoiceModel.TaxAffidavitImage.FilePath = fileUrls;
            }
         
            if (fileType == "BDNImage") {
                entireInvoiceModel.BDNImage = new ImageModel();
                entireInvoiceModel.BDNImage.FilePath = fileUrls;
            }

            if (fileType == "CoastGuardInspection") {
                entireInvoiceModel.CoastGuardInspectionImage = new ImageModel();
                entireInvoiceModel.CoastGuardInspectionImage.FilePath = fileUrls;
            }
            if (fileType == "InspectionRequestVoucher") {
                entireInvoiceModel.InspectionRequestVoucherImage = new ImageModel();
                entireInvoiceModel.InspectionRequestVoucherImage.FilePath = fileUrls;
            }
        }

        files.forEach(f => {

            if (f.Image != undefined) {
                const blobserviceclient = new BlobServiceClient(
                    sasToken, anonymouscredentials
                );

                const containerClient = blobserviceclient.getContainerClient(containerName);
                const blockBlobClient = containerClient.getBlockBlobClient(f.Name);

                // Split the url string to get img src 
                const uploadBlobResponse = blockBlobClient.upload(f.Image, f.Image.size);
                promises.push(uploadBlobResponse);
            }
        });

         return Promise.all(promises);
        // return this.imageurl;
    }


    GenerateRandomNumber(maxNumber: number) {
        return Math.floor((Math.random() * maxNumber) + 1);
    }

    


}

